namespace IdentityClaimsPlay.Admin.Areas.General.Pages;

[Authorize]
public partial class UserList {
  private List<UserDto> _users = new();

  protected override async Task OnInitializedAsync() =>
    _users = (await Context.Users.Include(u => u.UserCompanyRoles).ThenInclude(r => r.Company).OrderBy(u => u.Email).ToListAsync())
      .Select(u => new UserDto(u.Id,
        u.Email ?? "",
        u.UserCompanyRoles.Any(r => r.Role == Roles.Admin.ToString()),
        u.UserCompanyRoles.Where(r => r.Role != Roles.Admin.ToString()).OrderBy(r => r.Company.Name ?? "").ThenBy(r => r.Role).Select(r => $"<strong>{r.Company?.Name ?? ""}</strong>: {r.Role}").JoinStr()))
      .ToList();

  private record UserDto(string Id, string Email, bool IsAdmin, string Roles);
}