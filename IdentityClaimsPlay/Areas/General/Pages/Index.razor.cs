namespace IdentityClaimsPlay.Areas.General.Pages;

public partial class Index {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public UserHelper UserHelper { get; set; } = null!;

}