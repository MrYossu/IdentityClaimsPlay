namespace IdentityClaimsPlay.Helpers;

public enum Roles {
  Admin,
  CardIssuerAdmin,
  CardIssuerUser
}

public enum Permissions {
  CanViewCharities,
  CanEditCharities,
  CanViewDonors,
  CanEditDonors
}

public static class ClaimsHelper {
  // The claim name for the user role (saves us checking individual claims if they are an admin user
  public const string UserRole = "UserRole";

  public static string[] AllRoles =>
    Enum.GetNames<Roles>();

  public const string UserPermission = "UserPermission";
  public static string[] AllPermissions =>
    Enum.GetNames<Permissions>();

  // Claims for their company
  public const string CompanyId = "CompanyId";
  public const string CompanyName = "CompanyName";
}