using IdentityClaimsPlay.Crm.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Crm.Areas.General.Shared;

public class AuthorizeViewRole : AuthorizeView {
  [Parameter]
  public Roles Role { get; set; }

  protected override void OnParametersSet() =>
    Policy = Role.ToString();
}