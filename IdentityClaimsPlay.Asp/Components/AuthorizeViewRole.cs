
namespace IdentityClaimsPlay.Asp.Components;

public class AuthorizeViewRole : AuthorizeView {
  [Parameter]
  public Roles Role { get; set; }

  protected override void OnParametersSet() =>
    Policy = Role.ToString();
}