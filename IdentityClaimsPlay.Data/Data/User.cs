using Microsoft.AspNetCore.Identity;

namespace IdentityClaimsPlay.Data.Data;

public class User : IdentityUser {

  public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = null!;
  public virtual ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = null!;
}