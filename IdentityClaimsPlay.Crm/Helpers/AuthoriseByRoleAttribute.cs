namespace IdentityClaimsPlay.Crm.Helpers; 

public class AuthoriseByRoleAttribute : AuthorizeAttribute {
  public AuthoriseByRoleAttribute(params Roles[] roles) =>
    Policy = roles.Select(r => r.ToString()).JoinStr();
}