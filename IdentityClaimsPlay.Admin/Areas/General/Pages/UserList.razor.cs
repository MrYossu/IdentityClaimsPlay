namespace IdentityClaimsPlay.Admin.Areas.General.Pages;

[Authorize]
public partial class UserList {
  private List<User> _users = new();

  protected override async Task OnInitializedAsync() =>
    _users = (await Context.Users.Include(u => u.Claims).ToListAsync()).OrderBy(u => u.Email).ToList();
}