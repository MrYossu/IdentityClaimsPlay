using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityClaimsPlay.Admin.Areas.General.Pages;

[Authorize]
public partial class UserDetails {
  #region Parameters, inject and class variables

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
  private List<RoleDto> _companyRoles = new();

  #endregion

  protected override async Task OnInitializedAsync() {
    _companyRoles = ClaimsHelper.AllCompanyRoles.Select(p => new RoleDto(p, p.ToString().SplitCamelCase())).ToList();
    _companies = (await Context.Companies.OrderBy(c => c.Name).ToListAsync()).Select(c => new NameValuePair(c.Name, c.Id)).ToList();
  }

  protected override async Task OnParametersSetAsync() {
    if (Id == "new") {
      _user = new();
      _model.Companies = _companies.Select(c => new UserCompanyModel {
        CompanyId = c.Value,
        CompanyName = c.Name,
        Permissions = ClaimsHelper.AllPermissions.Select(p => new PermissionDto(p, false)).ToList()
      }).ToList();
    } else {
      _user = await Context.Users.Include(u => u.UserCompanyRoles).SingleOrDefaultAsync(u => u.Id == Id);
      if (_user is not null) {
        _model.Email = _user.Email ?? "";
        _model.Admin = _user.UserCompanyRoles.Any(r => r.Role == Roles.Admin.ToString());
        _model.Companies = _companies.Select(c => {
          return new UserCompanyModel {
            IsUser = _user.UserCompanyRoles.Any(r => r.CompanyId == c.Value && r.Role != Roles.Accountant.ToString()),
            CompanyId = c.Value,
            CompanyName = c.Name,
            Role = _user.UserCompanyRoles.Any(r => ClaimsHelper.IsCompanyRole(r.Role)) ? ClaimsHelper.StringToRole(_user.UserCompanyRoles.Single(r => ClaimsHelper.IsCompanyRole(r.Role)).Role) : Roles.CardIssuerAdmin,
            IsAccountant = _user.UserCompanyRoles.Any(r => r.CompanyId == c.Value && r.Role == Roles.Accountant.ToString()),
            IsDonor = _user.UserCompanyRoles.Any(r => r.CompanyId == c.Value && r.Role == Roles.Donor.ToString()),
            Permissions = ClaimsHelper.AllPermissions.Select(p => new PermissionDto(p, _user.UserCompanyRoles.Any(r => r.CompanyId == c.Value && r.Role == p))).ToList()
          };
        }).ToList();
      }
    }
    _loaded = true;
  }

  private List<string> _errors = new();
  private async Task Save() {
    // TODO AYS - Should really check to make sure they set at least one role. If role is card issuer user, then they should have set at least one permission
    if (Id == "new") {
      _user = new() {
        UserName = _model.Email,
        Email = _model.Email,
        EmailConfirmed = true
      };
      // TODO AYS - We should set his password to some random string and send hm an email advising him that he's now a user, with a link to reset his password
      IdentityResult result=await UserManager.CreateAsync(_user, "1");
      if (result.Succeeded) {
        _user = await Context.Users.Include(u => u.UserCompanyRoles).SingleAsync(u => u.Id == _user.Id);
        _user.UserCompanyRoles = CreateUserCompanyRoles();
      } else {
        _errors = result.Errors.Select(e => $"({e.Code}) {e.Description}").ToList();
        return;
      }
    } else {
      _user.UserCompanyRoles = CreateUserCompanyRoles();
    }
    await Context.SaveChangesAsync();
    NavigationManager.NavigateTo("/users");
  }

  private List<UserCompanyRole> CreateUserCompanyRoles() {
    List<UserCompanyRole> roles = new();
    if (_model.Admin) {
      roles.Add(new UserCompanyRole {
        Role = Roles.Admin.ToString()
      });
    }
    foreach (UserCompanyModel uc in _model.Companies) {
      if (uc.IsUser) {
        // Is he a company admin or flunky?
        roles.Add(new UserCompanyRole {
          CompanyId = uc.CompanyId,
          Role = uc.Role.ToString()
        });
        // If a flunky, set his permissions
        if (uc.Role == Roles.CardIssuerUser) {
          foreach (PermissionDto perm in uc.Permissions.Where(p => p.HasPermission)) {
            roles.Add(new UserCompanyRole {
              CompanyId = uc.CompanyId,
              Role = perm.Value
            });
          }
        }
      }
      // Is he a donor?
      if (uc.IsDonor) {
        roles.Add(new UserCompanyRole {
          CompanyId = uc.CompanyId,
          Role = Roles.Donor.ToString()
        });
      }
      // Is he a company accountant?
      if (uc.IsAccountant) {
        roles.Add(new UserCompanyRole {
          CompanyId = uc.CompanyId,
          Role = Roles.Accountant.ToString()
        });
      }
    }
    return roles;
  }

  #region Models

  public class UserModel {
    public string Email { get; set; } = "";
    public bool Admin { get; set; }
    public List<UserCompanyModel> Companies { get; set; } = new();
  }

  public class UserCompanyModel {
    public bool IsUser { get; set; }
    public bool IsDonor { get; set; }
    public string? CompanyId { get; set; }
    public string CompanyName { get; set; } = "";
    public Roles Role { get; set; } = Roles.CardIssuerAdmin;
    public bool IsAccountant { get; set; }
    public List<PermissionDto> Permissions = new();
  }

  public class RoleDto {
    public RoleDto(Roles value, string name) {
      Value = value;
      Name = name;
    }

    public Roles Value { get; set; }
    public string Name { get; set; } = "";
  }

  #endregion
}