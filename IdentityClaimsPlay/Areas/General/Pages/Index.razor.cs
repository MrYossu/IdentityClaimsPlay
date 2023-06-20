using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Areas.General.Pages;

public partial class Index {
  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  private ClaimsPrincipal _me = null!;
  private string Role { get; set; } = "";

  protected override async Task OnInitializedAsync() {
    _me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
    if (_me.Identity.IsAuthenticated) {
      Role = _me.Claims.Single(c => c.Type == ClaimsHelper.UserRole).Value;
    }
  }
}