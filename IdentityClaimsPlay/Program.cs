using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

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
  options.AddPolicy(ClaimsHelper.UserRoleAdmin, policyBuilder => policyBuilder.RequireAssertion(ctx => ctx.User.HasClaim(claim => claim is { Type: ClaimsHelper.UserRole, Value: ClaimsHelper.UserRoleAdmin })));
  // The next policy is for global and company admin users. This way we only need add the one policy to the [Authorize] attribute, but still allow both types of users
  options.AddPolicy(ClaimsHelper.UserRoleCardIssuerAdmin, policyBuilder => 
    policyBuilder.RequireAssertion(ctx => ctx.User.HasClaim(claim => claim is { Type: ClaimsHelper.UserRole, Value: ClaimsHelper.UserRoleAdmin } or { Type: ClaimsHelper.UserRole, Value: ClaimsHelper.UserRoleCardIssuerAdmin })));
  // Add a policy for each permission
  // TODO AYS - This isn't right. If someone has permission to edit something, we can assume they are allowed to view it, whereas this approach would prevent us from allowing them to view and edit
  foreach (string permission in ClaimsHelper.AllPermissions) {
    options.AddPolicy(permission, policyBuilder =>
      policyBuilder.RequireAssertion(ctx => ctx.User.HasClaim(claim => (claim.Type == ClaimsHelper.UserRole && claim.Value != ClaimsHelper.UserRoleCardIssuerUser) || claim.Value == permission)));
  }
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

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
await AddUser("admin@a.com", "1", ClaimsHelper.UserRoleAdmin);
// Card issuer access, access to most areas, can edit any data for his card issuer, but cannot see or edit data for other card issuers
await AddUser("companyadmin@a.com", "1", ClaimsHelper.UserRoleCardIssuerAdmin);
// Regular card issuer user, will only have access to specific areas as set up by the card issuer admin. Initially he can view donors and charities, but can't edit
await AddUser("flunky1@a.com", "1", ClaimsHelper.UserRoleCardIssuerUser, ClaimsHelper.UserCanViewCharities, ClaimsHelper.UserCanViewDonors);
// A more privileged regular card issuer user, can view and edit both donors and charities
await AddUser("flunky2@a.com", "1", ClaimsHelper.UserRoleCardIssuerUser, ClaimsHelper.UserCanViewCharities, ClaimsHelper.UserCanEditCharities, ClaimsHelper.UserCanViewDonors, ClaimsHelper.UserCanEditDonors);

app.Run();

// Identity methods

async Task AddUser(string email, string password, string userRole, params string[] userPermissions) {
  if (await userManager.FindByEmailAsync(email) != null) {
    return;
  }
  User user = new() {
    UserName = email,
    Email = email,
    EmailConfirmed = true
  };
  await userManager.CreateAsync(user, password);
  await userManager.AddClaimAsync(user, new Claim(ClaimsHelper.UserRole, userRole));
  foreach (string permission in userPermissions) {
    await userManager.AddClaimAsync(user, new Claim(ClaimsHelper.UserPermission, permission));
  }
}