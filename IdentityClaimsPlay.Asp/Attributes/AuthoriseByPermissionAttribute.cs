using IdentityClaimsPlay.Data.Data;
using Microsoft.AspNetCore.Authorization;

namespace IdentityClaimsPlay.Asp.Attributes;

public class AuthoriseByPermissionAttribute : AuthorizeAttribute {
  public AuthoriseByPermissionAttribute(params Permissions[] permissions) =>
    Policy = permissions.Select(r => r.ToString()).JoinStr();
}