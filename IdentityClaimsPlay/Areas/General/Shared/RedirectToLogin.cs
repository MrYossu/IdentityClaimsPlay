namespace IdentityClaimsPlay.Areas.General.Shared; 

public class RedirectToLogin : ComponentBase {
  [Inject]
  protected NavigationManager NavigationManager { get; set; } = null!;

  protected override void OnAfterRender(bool firstRender) =>
    NavigationManager.NavigateTo(RouteHelper.Login, true);
}