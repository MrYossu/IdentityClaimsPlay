using Microsoft.AspNetCore.Authorization;

namespace IdentityClaimsPlay.Asp.Attributes;

public class AuthoriseByRoleAttribute : AuthorizeAttribute {
  public AuthoriseByRoleAttribute(params Roles[] roles) =>
    Policy = roles.Select(r => r.ToString()).JoinStr();
}