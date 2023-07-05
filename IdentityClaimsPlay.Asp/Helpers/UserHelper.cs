using System.Security.Claims;
using IdentityClaimsPlay.Data.Data;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Asp.Helpers;

public class UserHelper {
  public UserHelper(AuthenticationStateProvider authenticationStateProvider) {
    ClaimsPrincipal principal = authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    IsAuthed = principal.Identity?.IsAuthenticated ?? false;
    if (IsAuthed) {
      Email = principal.Identity!.Name;
      Role = Enum.TryParse(principal.FindFirst(ClaimsHelper.UserRole)?.Value ?? "", out Roles role) ? role : Roles.CardIssuerUser;
      GlobalAdmin = Role == Roles.Admin;
      Permissions = principal.Claims.Where(c => c.Type == ClaimsHelper.UserPermission).ToList();
      CompanyId = principal.FindFirst(ClaimsHelper.CompanyId)?.Value ?? "";
      CompanyName = principal.FindFirst(ClaimsHelper.CompanyName)?.Value ?? "";
    }
  }

  public bool IsAuthed { get; set; }
  public string Email { get; set; } = "";
  public Roles Role { get; set; }
  public string RoleStr => Role.ToString().SplitCamelCase();
  public bool GlobalAdmin { get; set; }

  public List<Claim> Permissions { get; set; } = new();
  public string CompanyId { get; set; } = "";
  public string CompanyName { get; set; } = "";
}