namespace IdentityClaimsPlay.Areas.General.Pages;

[Authorize(Policy = ClaimsHelper.UserCanViewCharities)]
public partial class CharityList {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public UserHelper UserHelper { get; set; } = null!;

  private string _companyId = "";
  private List<Charity> _charities = new();

  protected override async Task OnInitializedAsync() {
    _companyId = UserHelper.CompanyId;
    _charities = await Context.Charities.Include(c => c.Company).Where(c => c.CompanyId.Contains(_companyId)).OrderBy(c => c.Name).ToListAsync();
  }
}