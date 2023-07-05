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
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<CompanyInfoHelper>();
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
// Global admin, access to all areas, can do anything. Other users can be added when you log in as global admin
string email = "ays@globaladmin.com";
if (await userManager.FindByEmailAsync(email) == null) {
  User user = new() {
    UserName = email,
    Email = email,
    EmailConfirmed = true
  };
  await userManager.CreateAsync(user, "1");
  await userManager.AddClaimAsync(user, new Claim(ClaimsHelper.UserRole, Roles.Admin.ToString()));
  AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
  user.UserCompanyRoles = new[] {
    new UserCompanyRole {
      Role = Roles.Admin.ToString()
    }
  };
  try {
    context.Users.Update(user);
    await context.SaveChangesAsync();
  } catch (Exception ex) {
    Console.WriteLine($"Ex adding ucr: {ex.Message}");
  }
}

app.Run();