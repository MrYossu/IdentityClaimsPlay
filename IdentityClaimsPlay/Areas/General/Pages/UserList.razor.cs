using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IdentityClaimsPlay.Areas.General.Pages;

public partial class UserList {
  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  [Inject]
  public AppDbContext Context { get; set; } = null!;

  private List<User> _users = new();

  protected override async Task OnInitializedAsync() =>
    _users = await Context.Users.OrderBy(u => u.Email).ToListAsync();
}