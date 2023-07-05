using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Asp.Components;

public class AuthorizeViewPermission : AuthorizeView {
  [Parameter]
  public Permissions Permission { get; set; }

  protected override void OnParametersSet() =>
    Policy = Permission.ToString();
}