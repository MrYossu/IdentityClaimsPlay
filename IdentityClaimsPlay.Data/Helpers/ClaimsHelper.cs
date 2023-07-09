using IdentityClaimsPlay.Data.Data;

namespace IdentityClaimsPlay.Data.Helpers;

public static class ClaimsHelper {
  // The claim name for the user role (saves us checking individual claims if they are an admin user
  public const string UserRole = "UserRole";

  public static List<Roles> AllRoles =>
    Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();

  public static List<Roles> AllCompanyRoles =>
    Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(r => r != Roles.Admin && r != Roles.Accountant).ToList();

  public static bool IsCompanyRole(Roles role) =>
    AllCompanyRoles.Contains(role);

  public static bool IsCompanyRole(string role) =>
    AllCompanyRoles.Select(r => r.ToString()).Contains(role);

  public const string UserPermission = "UserPermission";

  public static string[] AllPermissions =>
    Enum.GetNames<Permissions>();

  // Claims for their company
  public const string CompanyId = "CompanyId";
  public const string CompanyName = "CompanyName";

  // This method assumes we aren't sending in rubbish
  public static Roles StringToRole(string role) =>
    AllRoles.Single(r => r.ToString() == role);
}