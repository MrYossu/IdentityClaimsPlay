using Microsoft.AspNetCore.Authorization;
using Pixata.Extensions;

namespace IdentityClaimsPlay.Data.Helpers; 

public class AuthoriseByRoleAttribute : AuthorizeAttribute {
  public AuthoriseByRoleAttribute(params Roles[] roles) =>
    Policy = roles.Select(r => r.ToString()).JoinStr();
}