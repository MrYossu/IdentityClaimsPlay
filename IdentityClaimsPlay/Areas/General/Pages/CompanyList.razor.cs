namespace IdentityClaimsPlay.Areas.General.Pages;

[Authorize(Policy = ClaimsHelper.UserRoleAdmin)]
public partial class CompanyList {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  private List<Company> _companies = new();

  protected override async Task OnInitializedAsync() =>
    _companies = await Context.Companies.OrderBy(c => c.Name).ToListAsync();
}