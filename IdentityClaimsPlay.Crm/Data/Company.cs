using System.Collections.ObjectModel;

namespace IdentityClaimsPlay.Crm.Data; 

public class Company : EntityBase {
  public string Name { get; set; } = "";
  public string Domain { get; set; } = "";
  public virtual ObservableCollection<Charity> Charities { get; set; } = new();
}