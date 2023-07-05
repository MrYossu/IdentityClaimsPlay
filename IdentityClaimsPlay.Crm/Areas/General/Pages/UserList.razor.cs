using IdentityClaimsPlay.Crm.Data;
using IdentityClaimsPlay.Crm.Helpers;

namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

[AuthoriseByRole(Roles.CardIssuerAdmin)]
public partial class UserList {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public UserHelper UserHelper { get; set; } = null!;

  private List<User> _users = new();

  protected override async Task OnInitializedAsync() =>
    // As this page is authed, we know that the user must be logged in, so are safe with the ! operator on the next line
    _users = (await Context.Users.Include(u => u.Company).Include(u => u.Claims).Where(u => string.IsNullOrWhiteSpace(UserHelper.CompanyId) || u.CompanyId == UserHelper.CompanyId).ToListAsync()).OrderBy(u => u.Company?.Name ?? "").ThenBy(u => u.Email).ToList();
}