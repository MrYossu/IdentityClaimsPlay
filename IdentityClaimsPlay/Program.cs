WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<AppDbContext>(options => {
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
  options.EnableSensitiveDataLogging();
  options.EnableDetailedErrors();
}, lifetime: ServiceLifetime.Scoped);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
  })
  .AddDefaultTokenProviders()
  .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(options => {
  // Add two policies to check for admin users. First one is for pages that are only visible to global admin users...
  options.AddPolicy(Roles.Admin.ToString(), policyBuilder => policyBuilder.RequireAssertion(ctx => ctx.User.HasClaim(claim => claim.Type == ClaimsHelper.UserRole && claim.Value == Roles.Admin.ToString())));

  // The next policy is for global and company admin users. This way we only need add the one policy to the [Authorize] attribute, but still allow both types of users
  options.AddPolicy(Roles.CardIssuerAdmin.ToString(), policyBuilder =>
    policyBuilder.RequireAssertion(ctx => ctx.User.HasClaim(claim =>
      claim.Type == ClaimsHelper.UserRole
      && (claim.Value == Roles.Admin.ToString() || claim.Value == Roles.CardIssuerAdmin.ToString())))
  );

  // Add a policy for each permission
  foreach (string permission in ClaimsHelper.AllPermissions) {
    options.AddPolicy(permission, policyBuilder =>
      policyBuilder.RequireAssertion(ctx => ctx.User.HasClaim(claim => (claim.Type == ClaimsHelper.UserRole && claim.Value != Roles.CardIssuerUser.ToString()) || claim.Value == permission)));
  }
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddTransient<UserHelper>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToAreaPage("/_Host", "General");

// Identity
using IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
UserManager<User> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
// Overall admin, access to all areas, can do anything
await AddUser("admin@a.com", "1", Roles.Admin);
// Card issuer access, access to most areas, can edit any data for his card issuer, but cannot see or edit data for other card issuers
await AddUser("companyadmin@a.com", "1", Roles.CardIssuerAdmin);
// Regular card issuer user, will only have access to specific areas as set up by the card issuer admin. Initially he can view donors and charities, but can't edit
await AddUser("flunky1@a.com", "1", Roles.CardIssuerUser, Permissions.CanViewCharities, Permissions.CanViewDonors);
// A more privileged regular card issuer user, can view and edit both donors and charities
await AddUser("flunky2@a.com", "1", Roles.CardIssuerUser, Permissions.CanViewCharities, Permissions.CanEditCharities, Permissions.CanViewDonors, Permissions.CanEditDonors);

app.Run();

// Identity methods

async Task AddUser(string email, string password, Roles userRole, params Permissions[] userPermissions) {
  if (await userManager.FindByEmailAsync(email) != null) {
    return;
  }
  User user = new() {
    UserName = email,
    Email = email,
    EmailConfirmed = true
  };
  await userManager.CreateAsync(user, password);
  await userManager.AddClaimAsync(user, new Claim(ClaimsHelper.UserRole, userRole.ToString()));
  foreach (Permissions permission in userPermissions) {
    await userManager.AddClaimAsync(user, new Claim(ClaimsHelper.UserPermission, permission.ToString()));
  }
}