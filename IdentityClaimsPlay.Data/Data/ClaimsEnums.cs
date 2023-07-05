namespace IdentityClaimsPlay.Data.Data;

public enum Roles
{
    Admin,
    CardIssuerAdmin,
    CardIssuerUser
}

public enum Permissions
{
    CanViewCharities,
    CanEditCharities,
    CanViewDonors,
    CanEditDonors
}