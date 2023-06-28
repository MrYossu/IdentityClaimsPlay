using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Areas.General.Pages;

public partial class Index {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  //[Inject]


  private ClaimsPrincipal _me = null!;
  private string _companyName = "";
  private string Role { get; set; } = "";

  protected override async Task OnInitializedAsync() {
    _me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
    _companyName = _me.FindFirst(ClaimsHelper.CompanyName)?.Value ?? "";
    if (_me.Identity.IsAuthenticated) {
      Role = _me.Claims.Single(c => c.Type == ClaimsHelper.UserRole).Value;
    }
  }
}