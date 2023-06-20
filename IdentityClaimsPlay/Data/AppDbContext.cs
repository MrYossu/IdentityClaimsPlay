using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityClaimsPlay.Data; 

public class AppDbContext :IdentityDbContext<User> {
  public AppDbContext(DbContextOptions options) : base(options) {
  }

  public DbSet<Company> Companies { get; set; } = null!;
}