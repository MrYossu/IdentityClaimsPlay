using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityClaimsPlay.Data;

public class User : IdentityUser {
  public string? CompanyId { get; set; }
  public virtual Company? Company { get; set; }

  [NotMapped]
  public string Role { get; set; } = "";

  [NotMapped]
  public List<string> Claims { get; set; } = new();
}