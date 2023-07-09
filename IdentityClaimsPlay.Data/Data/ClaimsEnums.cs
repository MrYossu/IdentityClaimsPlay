namespace IdentityClaimsPlay.Data.Data;

public enum Roles {
  Admin,
  CardIssuerAdmin,
  CardIssuerUser,
  Accountant,
  Donor
}

public enum Permissions {
  CanViewCharities,
  CanEditCharities,
  CanViewDonors,
  CanEditDonors
}