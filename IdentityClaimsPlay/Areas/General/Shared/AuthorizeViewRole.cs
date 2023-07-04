using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Areas.General.Shared;

public class AuthorizeViewRole : AuthorizeView {
  [Parameter]
  public Roles Role { get; set; }

  protected override void OnParametersSet() =>
    Policy = Role.ToString();
}