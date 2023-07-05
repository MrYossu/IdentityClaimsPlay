using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IdentityClaimsPlay.Data.Data;

public class User : IdentityUser {
  public string? CompanyId { get; set; }
  public virtual Company? Company { get; set; }

  [NotMapped]
  public string Role { get; set; } = "";

  public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
}