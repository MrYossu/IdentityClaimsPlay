using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Areas.General.Pages;

[Authorize(Policy = ClaimsHelper.UserCanViewCharities)]
public partial class CharityList {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  private string _companyId = "";
  private List<Charity> _charities = new();

  protected override async Task OnInitializedAsync() {
    ClaimsPrincipal me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
    // As this page is authed, we know that the user must be logged in, so are safe with the ! operator on the next line
    User meUser = await Context.Users.Include(u => u.Company).SingleAsync(u => u.Email == me.Identity!.Name);
    _companyId = meUser.CompanyId ?? "";
    _charities = await Context.Charities.Include(c => c.Company).Where(c => c.CompanyId.Contains(_companyId)).OrderBy(c => c.Name).ToListAsync();
  }
}