namespace IdentityClaimsPlay.Crm.Areas.General.Shared; 

public partial class CommonComponentBase {
  [CascadingParameter]
  public CompanyInfo CompanyInfo { get; set; } = null!;

  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public UserHelper UserHelper { get; set; } = null!;

}