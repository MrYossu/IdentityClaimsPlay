using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Areas.General.Pages;

[Authorize(Policy = ClaimsHelper.UserRoleCardIssuerAdmin)]
public partial class UserList {
  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  [Inject]
  public AppDbContext Context { get; set; } = null!;

  private List<User> _users = new();
  private string CompanyName { get; set; } = "";

  protected override async Task OnInitializedAsync() {
    ClaimsPrincipal me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
    // As this page is authed, we know that the user must be logged in, so are safe with the ! operator on the next line
    User meUser = await Context.Users.Include(u => u.Company).SingleAsync(u => u.Email == me.Identity!.Name);
    _users = (await Context.Users.Include(u => u.Company).Where(u => string.IsNullOrWhiteSpace(meUser.CompanyId) || u.CompanyId == meUser.CompanyId).ToListAsync()).OrderBy(u => u.Company?.Name ?? "").ThenBy(u => u.Email).ToList();
    foreach (User user in _users) {
      user.Role = (await Context.UserClaims.SingleAsync(c => c.UserId == user.Id && c.ClaimType == ClaimsHelper.UserRole)).ClaimValue.SplitCamelCase();
      user.Claims = await Context.UserClaims.Where(c => c.UserId == user.Id && c.ClaimType != ClaimsHelper.UserRole).Select(c => c.ClaimValue.SplitCamelCase(true)).ToListAsync();
    }
    CompanyName = meUser.Company?.Name ?? "";
  }
}