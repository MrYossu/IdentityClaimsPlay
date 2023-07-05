using Microsoft.AspNetCore.Authorization;
using Pixata.Extensions;

namespace IdentityClaimsPlay.Data.Helpers;

public class AuthoriseByPermissionAttribute : AuthorizeAttribute {
  public AuthoriseByPermissionAttribute(params Permissions[] permissions) =>
    Policy = permissions.Select(r => r.ToString()).JoinStr();
}