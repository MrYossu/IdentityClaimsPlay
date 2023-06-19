﻿using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Areas.General.Pages;

public partial class Index {
  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  private ClaimsPrincipal _me = null!;

  protected override async Task OnInitializedAsync() =>
    _me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
}