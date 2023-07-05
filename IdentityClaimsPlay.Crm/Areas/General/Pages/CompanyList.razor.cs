namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

[AuthoriseByRole(Roles.Admin)]
public partial class CompanyList {

  private List<Company> _companies = new();

  protected override async Task OnInitializedAsync() =>
    _companies = await Context.Companies.OrderBy(c => c.Name).ToListAsync();
}