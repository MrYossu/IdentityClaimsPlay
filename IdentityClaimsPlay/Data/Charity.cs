namespace IdentityClaimsPlay.Data; 

public class Charity : EntityBase {
  public string Name { get; set; } = "";
  public string Number { get; set; } = "";
  public string Address { get; set; } = "";
  public string CompanyId { get; set; } = "";
  public virtual Company Company { get; set; } = null!;
}