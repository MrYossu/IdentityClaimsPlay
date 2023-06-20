using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IdentityClaimsPlay.Areas.General.Pages;

[Authorize(Policy = ClaimsHelper.UserRoleCardIssuerAdmin)]
public partial class UserDetails {
  [Parameter]
  public string Id { get; set; } = "";

  [Inject]
  private AppDbContext Context { get; set; } = null!;

  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  private ClaimsPrincipal _me = null!;
  private string Role { get; set; } = "";

  [Inject]
  public UserManager<User> UserManager { get; set; } = null!;

  [Inject]
  public NavigationManager NavigationManager { get; set; } = null!;

  private bool _loaded;
  private User? _user;

  private UserModel _model = new();

  private List<NameValuePair> _companies = new();
  private readonly List<NameValuePair> _roles = ClaimsHelper.AllRoles.Select(p => new NameValuePair(p, p)).ToList();

  protected override async Task OnInitializedAsync() {
    _me = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
    Role = _me.Claims.Single(c => c.Type == ClaimsHelper.UserRole).Value;
    _companies = (await Context.Companies.OrderBy(c => c.Name).ToListAsync()).Select(c => new NameValuePair(c.Name, c.Id)).ToList();
  }

  protected override async Task OnParametersSetAsync() {
    _user = await Context.Users.Include(u => u.Company).SingleOrDefaultAsync(u => u.Id == Id);
    if (_user is not null) {
      List<ClaimDto> claims = (await UserManager.GetClaimsAsync(_user)).Select(c => new ClaimDto(c.Type, c.Value)).ToList();
      _model = new UserModel {
        Email = _user.Email ?? "",
        CompanyId = _user.CompanyId,
        Role = claims.Single(c => c.Type == ClaimsHelper.UserRole).Value,
        Permissions = ClaimsHelper.AllPermissions.Select(p => new PermissionDto(p, claims.Any(c => c.Value == p))).ToList()
      };
    }
    _loaded = true;
  }

  private async Task Save() {
    _user!.CompanyId = _model.Role == ClaimsHelper.UserRoleAdmin ? null : _model.CompanyId;
    Context.Users.Update(_user);
    await Context.SaveChangesAsync();
    IList<Claim> claims = await UserManager.GetClaimsAsync(_user);
    Claim role = claims.Single(c => c.Type == ClaimsHelper.UserRole);
    await UserManager.ReplaceClaimAsync(_user, role, new Claim(ClaimsHelper.UserRole, _model.Role));
    List<Claim> permissions = claims.Where(c => c.Type != ClaimsHelper.UserRole).ToList();
    foreach (PermissionDto dto in _model.Permissions) {
      switch (dto.HasPermission) {
        case true when permissions.All(p => p.Value != dto.Value):
          await UserManager.AddClaimAsync(_user, new Claim(ClaimsHelper.UserPermission, dto.Value));
          break;
        case false when permissions.Any(p => p.Value == dto.Value):
          await UserManager.RemoveClaimAsync(_user, permissions.First(p => p.Value == dto.Value));
          break;
      }
    }
    NavigationManager.NavigateTo("/users");
  }

  private class UserModel {
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public string? CompanyId { get; set; }
    public List<PermissionDto> Permissions = new();
  }
}