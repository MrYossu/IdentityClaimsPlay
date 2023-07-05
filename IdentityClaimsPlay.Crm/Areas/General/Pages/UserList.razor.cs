namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

[AuthoriseByRole(Roles.CardIssuerAdmin)]
public partial class UserList {
  private List<User> _users = new();

  protected override async Task OnInitializedAsync() =>
    _users = (await Context.Users.Include(u => u.Claims).Where(u => u.UserCompanyRoles.Any(c => c.CompanyId == CompanyInfo.Id)).ToListAsync()).OrderBy(u => u.Email).ToList();
}