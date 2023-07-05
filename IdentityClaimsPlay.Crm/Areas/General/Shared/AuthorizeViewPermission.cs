using IdentityClaimsPlay.Crm.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Crm.Areas.General.Shared; 

public class AuthorizeViewPermission : AuthorizeView {
  [Parameter]
  public Permissions Permission { get; set; }

  protected override void OnParametersSet() =>
    Policy = Permission.ToString();
}