namespace IdentityClaimsPlay.Data;

public class User : IdentityUser {
  public string? CompanyId { get; set; }
  public virtual Company? Company { get; set; }
}