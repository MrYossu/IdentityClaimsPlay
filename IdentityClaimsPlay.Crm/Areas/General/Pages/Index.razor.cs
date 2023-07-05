﻿using IdentityClaimsPlay.Crm.Helpers;

namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

public partial class Index {
  [CascadingParameter]
  public CompanyInfo CompanyInfo { get; set; } = null!;

  [Inject]
  public UserHelper UserHelper { get; set; } = null!;

}