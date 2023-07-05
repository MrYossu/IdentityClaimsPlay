using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityClaimsPlay.Data.Data;

public class AppDbContext : IdentityDbContext<User> {
  public AppDbContext(DbContextOptions options) : base(options) {
  }

  public DbSet<Charity> Charities { get; set; } = null!;
  public DbSet<Company> Companies { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>(b => {
      b.HasMany(e => e.Claims)
        .WithOne()
        .HasForeignKey(uc => uc.UserId)
        .IsRequired();
    });
  }
}