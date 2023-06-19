# IdentityClaimsPlay

## Playing around with claims in ASP.NET Core identity

Part of a project I'm starting requires us to allow the users to set permissions to control what other users can do on the site. I had previously used roles for this, but it was getting a bit cumbersome, and Microsoft seem to recommend going with claims anyway, so I decided to have a play around with this and see if I could get it all working.

The project is a web site that will be used by many companies. We need three user types...
- a global admin user role, who can see and edit all data
- company admins, who can see and edit data relevant to their company, but not global data, nor data relevant to other companies
- regular company users (referred to round here as flunkies), who have limited permissions over their own company data. They may be able to view but not edit data, or they may be able to edit certain sets of data but not others.

My first stab at this is to have two types of claims, one confusingly named `UserRole` (not to be confused with ASP.NET Core `Roles`), and one named `UserPermission`. The former specifies which of the three user roles is applicable. A user should always have exactly one of these claims. The latter type of claims is only relevant for flunkies, and specifies which specific actions they can perform.

### Objectives
- Set up the three types of user described above
- Set up user roles and permissions
- Allow a global admin to add, edit and delete users of any type
- Allow company admins to add, edit and delete company admins and flunkies for their company
- Ensure that company-specific users can only see or do what they are supposed to be allowed to see and do
