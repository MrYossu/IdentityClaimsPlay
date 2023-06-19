using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IdentityClaimsPlay.Areas.General.Pages;

public partial class Index {
  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public UserManager<User> UserManager { get; set; } = null!;

  private List<User> _users = new();
  private ClaimsPrincipal _me = null!;

  protected override async Task OnInitializedAsync() {
    _users = await Context.Users.OrderBy(u => u.Email).ToListAsync();
    _me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
  }

  private string _user = "";
  private List<ClaimDto> _claims = new();

  private async Task ShowClaims(User user) {
    _user = user.Email;
    _claims = (await UserManager.GetClaimsAsync(user)).Select(c => new ClaimDto(c.Type, c.Value)).ToList();
  }
}