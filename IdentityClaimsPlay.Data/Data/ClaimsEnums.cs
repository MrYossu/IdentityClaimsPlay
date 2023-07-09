namespace IdentityClaimsPlay.Data.Data;

public enum Roles {
  Admin,
  CardIssuerAdmin,
  CardIssuerUser,
  Accountant
}

public enum Permissions {
  CanViewCharities,
  CanEditCharities,
  CanViewDonors,
  CanEditDonors
}