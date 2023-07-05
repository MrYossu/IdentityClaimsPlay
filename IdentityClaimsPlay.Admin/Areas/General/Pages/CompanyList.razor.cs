namespace IdentityClaimsPlay.Admin.Areas.General.Pages;

[Authorize]
public partial class CompanyList {

  private List<Company> _companies = new();

  protected override async Task OnInitializedAsync() =>
    _companies = await Context.Companies.OrderBy(c => c.Name).ToListAsync();
}