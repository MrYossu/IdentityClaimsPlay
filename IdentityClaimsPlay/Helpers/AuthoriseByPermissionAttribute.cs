namespace IdentityClaimsPlay.Helpers;

public class AuthoriseByPermissionAttribute : AuthorizeAttribute {
  public AuthoriseByPermissionAttribute(params Permissions[] permissions) =>
    Policy = permissions.Select(r => r.ToString()).JoinStr();
}