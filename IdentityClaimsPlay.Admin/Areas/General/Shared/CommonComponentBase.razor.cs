namespace IdentityClaimsPlay.Admin.Areas.General.Shared; 

public partial class CommonComponentBase {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public UserHelper UserHelper { get; set; } = null!;

}