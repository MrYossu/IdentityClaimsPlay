using IdentityClaimsPlay.Crm.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Crm.Areas.General.Shared;

public class RedirectToLogin : ComponentBase {
  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  [Inject]
  protected NavigationManager NavigationManager { get; set; } = null!;

  protected override async Task OnAfterRenderAsync(bool firstRender) {
    ClaimsPrincipal me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
    if (me.Identity!.IsAuthenticated) {
      NavigationManager.NavigateTo(RouteHelper.NotAuthorised, false);
      return;
    }
    NavigationManager.NavigateTo(RouteHelper.Login, true);
  }
}