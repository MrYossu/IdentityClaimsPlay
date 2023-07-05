using System.Collections.ObjectModel;

namespace IdentityClaimsPlay.Data.Data; 

public class Company : EntityBase {
  public string Name { get; set; } = "";
  public string Address { get; set; } = "";
  public string Domain { get; set; } = "";
  public virtual ObservableCollection<Charity> Charities { get; set; } = new();
  public virtual ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = null!;
}