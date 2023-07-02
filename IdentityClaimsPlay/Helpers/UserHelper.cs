using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Helpers;

public class UserHelper {
  public UserHelper(AuthenticationStateProvider authenticationStateProvider, AppDbContext context) {
    ClaimsPrincipal principal = authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    IsAuthed = principal.Identity?.IsAuthenticated ?? false;
    if (IsAuthed) {
      Email = principal.Identity!.Name;
      Role = principal.FindFirst(ClaimsHelper.UserRole)?.Value ?? "";
      GlobalAdmin = Role == ClaimsHelper.UserRoleAdmin;
      Claims = principal.Claims.Where(c => c.Type == ClaimsHelper.UserPermission).ToList();
      CompanyId = principal.FindFirst(ClaimsHelper.CompanyId)?.Value ?? "";
      CompanyName = principal.FindFirst(ClaimsHelper.CompanyName)?.Value ?? "";
    }
  }

  public bool IsAuthed { get; set; }
  public string Email { get; set; } = "";
  public string Role { get; set; } = "";
  public bool GlobalAdmin { get; set; }

  public List<Claim> Claims { get; set; } = new();
  public string CompanyId { get; set; } = "";
  public string CompanyName { get; set; } = "";
}