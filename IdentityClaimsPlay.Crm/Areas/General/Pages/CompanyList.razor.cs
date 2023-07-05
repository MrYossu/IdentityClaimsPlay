using IdentityClaimsPlay.Crm.Helpers;

namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

[AuthoriseByRole(Roles.Admin)]
public partial class CompanyList {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  private List<Company> _companies = new();

  protected override async Task OnInitializedAsync() =>
    _companies = await Context.Companies.OrderBy(c => c.Name).ToListAsync();
}