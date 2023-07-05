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

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<UserHelper>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
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

app.Run();
