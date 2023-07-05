namespace IdentityClaimsPlay.Data.Data;

public class UserCompanyRole : EntityBase {
  public string UserId { get; set; } = "";
  public virtual User User { get; set; } = null!;
  public string CompanyId { get; set; } = "";
  public virtual Company Company { get; set; } = null!;
  public string Role { get; set; } = "";
}