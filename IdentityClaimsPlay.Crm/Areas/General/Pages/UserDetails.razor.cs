using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

[AuthoriseByRole(Roles.CardIssuerAdmin)]
public partial class UserDetails {
  [Parameter]
  public string Id { get; set; } = "";

  [Inject]
  public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

  [Inject]
  public UserManager<User> UserManager { get; set; } = null!;

  [Inject]
  public NavigationManager NavigationManager { get; set; } = null!;

  private bool _loaded;
  private User? _user;

  private readonly UserModel _model = new();

  private List<NameValuePair> _companies = new();
  private List<RoleDto> _roles = new();

  protected override async Task OnInitializedAsync() {
    _roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(r => r != Roles.Admin).Select(p => new RoleDto(p, p.ToString().SplitCamelCase())).ToList();
    _companies = (await Context.Companies.OrderBy(c => c.Name).ToListAsync()).Select(c => new NameValuePair(c.Name, c.Id)).ToList();
  }

  protected override async Task OnParametersSetAsync() {
    if (Id == "new") {
      // TODO AYS - We need to add a UserCompanyRole with the company Id pulled from CompanyInfo. The Role will be set when the data is submitted
      _user = new();
      _model.Role = Roles.CardIssuerUser;
      _model.Permissions = ClaimsHelper.AllPermissions.Select(p => new PermissionDto(p, false)).ToList();
    } else {
      _user = await Context.Users.SingleOrDefaultAsync(u => u.Id == Id);
      if (_user is not null) {
        _model.Email = _user.Email ?? "";
        List<ClaimDto> claims = (await UserManager.GetClaimsAsync(_user)).Select(c => new ClaimDto(c.Type, c.Value)).ToList();
        _model.Role = Enum.TryParse(claims.Single(c => c.Type == ClaimsHelper.UserRole).Value, out Roles role) ? role : Roles.CardIssuerUser;
        _model.Permissions = ClaimsHelper.AllPermissions.Select(p => new PermissionDto(p, claims.Any(c => c.Value == p))).ToList();
      }
    }
    _loaded = true;
  }

  private async Task Save() {
    if (Id == "new") {
      User user = new() {
        UserName = _model.Email,
        Email = _model.Email,
        EmailConfirmed = true,
      };
      await UserManager.CreateAsync(user, "1");
      await UserManager.AddClaimAsync(user, new Claim(ClaimsHelper.UserRole, _model.Role.ToString()));
      if (_model.Role == Roles.CardIssuerUser) {
        foreach (string permission in _model.Permissions.Where(p => p.HasPermission).Select(p => p.Value)) {
          await UserManager.AddClaimAsync(user, new Claim(ClaimsHelper.UserPermission, permission));
        }
      }
    } else {
      Context.Users.Update(_user);
      await Context.SaveChangesAsync();
      IList<Claim> claims = await UserManager.GetClaimsAsync(_user);
      Claim role = claims.Single(c => c.Type == ClaimsHelper.UserRole);
      await UserManager.ReplaceClaimAsync(_user, role, new Claim(ClaimsHelper.UserRole, _model.Role.ToString()));
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
    }
    NavigationManager.NavigateTo("/users");
  }

  public class RoleDto {
    public RoleDto(Roles value, string name) {
      Value = value;
      Name = name;
    }

    public Roles Value { get; set; }
    public string Name { get; set; } = "";
  }

  private class UserModel {
    public string Email { get; set; } = "";
    public Roles Role { get; set; }
    public List<PermissionDto> Permissions = new();
  }
}