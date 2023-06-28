namespace IdentityClaimsPlay.Helpers; 

public class ClaimsHelper {
  // Note that I have used the terms role and permission here. These are both claims, but I'm using the former to specify his overall role on the site and the latter to specify fine-grained permissions over areas in the site
  // // I'm assuming every user will have exactly one claim of type UserRole, and zero-n claims of type UserPermission

  // First the claim name for the user type (saves us checking individual claims if they are an admin user
  public const string UserRole = "UserRole";
  // Now the values
  public const string UserRoleAdmin = "Admin";
  public const string UserRoleCardIssuerAdmin = "CardIssuerAdmin";
  public const string UserRoleCardIssuerUser = "CardIssuerUser";

  public static List<string> AllRoles =>
    new() {
      UserRoleAdmin,
      UserRoleCardIssuerAdmin,
      UserRoleCardIssuerUser
    };

  // Claim name for regular user permissions
  public const string UserPermission = "UserPermission";
  // Now the individual user permissions
  public const string UserCanViewCharities = "CanViewCharities";
  public const string UserCanEditCharities = "CanEditCharities";
  public const string UserCanViewDonors = "CanViewDonors";
  public const string UserCanEditDonors = "CanEditDonors";

  public static List<string> AllPermissions =>
    new() {
      UserCanViewCharities,
      UserCanEditCharities,
      UserCanViewDonors,
      UserCanEditDonors
    };

  // Claims for their company
  public const string CompanyId = "CompanyId";
  public const string CompanyName = "CompanyName";
}