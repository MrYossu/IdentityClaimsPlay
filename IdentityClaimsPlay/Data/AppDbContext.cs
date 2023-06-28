using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityClaimsPlay.Data;

public class AppDbContext : IdentityDbContext<User> {
  public AppDbContext(DbContextOptions options) : base(options) {
  }

  public DbSet<Charity> Charities { get; set; } = null!;
  public DbSet<Company> Companies { get; set; } = null!;
}