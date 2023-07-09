# IdentityClaimsPlay

## Playing around with claims in ASP.NET Core identity

Part of a project I'm starting requires us to allow the users to set permissions to control what other users can do on the site. I had previously used roles for this, but it was getting a bit cumbersome, and Microsoft seem to recommend going with claims anyway, so I decided to have a play around with this and see if I could get it all working.

### Solution overview
The project is a set of web sites that will be used by many companies. Each company will have the following portals (see the notes lower down about how to simulate different companies):
- A global admin portal to be used by the company that runs the whole operation. Global admin suers would be able to add, edit and delete the individual companies who will use the other portals
- A CRM, where they can see and modify their own company data and their customers' data
- A customer portal, where customers can log in and see thier data
- An accountant portal, where their accountant can log in and generate reports

In addition, there will be a global admin portal, where the global company running this whole operation can manage the individual companies.

The fun bit is that an individual user may need multiple roles. For example, an accountant may do the accounts for more than one company, so needs to be able to log in to multiple accountant portals (but not all of them). At the same time, the accountant may well be a customer of one of the companies, so needs to log in to a customer portal, but be recognised there as a customer, not an accountant.

I am making the assumption that only company users will log in to the CRM, only customers will log in to the customer portal, and only accountants will log in to the accountants portal. This means that the log-in page of each portal can look for a user with the email supplied, and a known role, based on the individual portal.

I will also need to account for the fact that the same code base will serve all companies, so I will need to know which company is relevant when logging in. This will be picked up from the domain name.

**Note:** That for reasons not worth droning on about here, companies are also referred to as card issuers, and customers are also referred to as donors. I used "customers" and "companies" in this document, as these are more familiar to most people, but used "card issuers" and "donors" in various places in the code as these more accurately reflect the real scenario.

### User types
Following on from this, we need the following user types...
- Global admin users, who can see and edit all data, including adding and editing companies
- Company admins, who can see and edit data relevant to their company, but not global data, nor data relevant to other companies
- Regular company users (referred to round here as _[flunkies](https://dictionary.cambridge.org/dictionary/english/flunky)_), who have limited permissions over their own company data. They may be able to view but not edit data, or they may be able to edit certain sets of data but not others. Their specific permissions are to be set by their company admin.
- Customers of individual companies, confusingly called donors in the code (don't ask, it's a long and not very interesting story!)

I'm approaching this by setting up claims:
- Each user will have at least one claim whose name is one of the `Roles` enum values. As exlained above, the reason they can have multiple roles is that they may need to log in to different portals in different roles, eg into the CRM as a company user and into an accountant portal as an accountant.
- Depending on the type of user, they may also have claims whose name is one of the `Permissions` enum values. This is probably only going to be relevant to flunkies, as global admins and company adminshave full control over their relevant portals. Accountants will only be generating reports, not changing data, so will pobably not need individual permissions.
- There will also need to be claims for the company info, so we know which company is relevant. **Update:** Looks like this might not be necessary, as I have just added a cascading parameter that makes the company info available to all components in the portal, so we won't need to set this as a user claim.

The code will add a user with email `admin@a.com` who is a global admin user. Once logged in with that email (password is `1`), that user can add other users.

### Things to do
- General
  - All pages on all portals should require auth
- Admin portal (done)
- CRM portal
  - Rebrand with the company colours
  - Remove non-CRM fucntionality (eg the global admin functionality)
- Customer (donor) portal
  - Add some functionality, including branding
- Accountant portal
  - Add the appropriate functionality

### Notes for anyone intending to clone this repo
I use the rather excellent [Telerik UI for Blazor](https://www.telerik.com/blazor-ui) components(*), which are referenced in this sample project. If you don't have a licence for this, you can either get a free trial, or just modify the pages that use Telerik controls to use regular Blazor controls. As I intend to lift large parts of this code into the real application, I started off using Telerik in the sample.

As this is a sample, all users will have the stupendously secure password of `1`. If you copy any of this code into a real app, don't forget to tighten up the password rules in `Program.cs`! Specifically, set `options.Password.RequiredLength` to a [sensible length](https://blog.codinghorror.com/password-rules-are-bullshit/). Actually, I changed it so that logging in doesn't even require you to enter a password. This was done for ease of switching users, so don't try it on a production site!

When you first run the global admin site, you'll need to add at least one company, and set the `Domain` property to something like `companya`, where `companya` is the fake company domain you'll add to your `hosts` file (see below).

In order to play wih this properly, you need to simulate multiple sites. The easiest way to do this is to add some lines to your `hosts` file, which you can find in `C:\Windows\System32\drivers\etc`. You'll need to run Notepad as administrator to be able to edit this file.For each company that you want to simulate, add a line like...

`127.0.0.1 companya`

...to the file. Then when the each portal runs, change `localhost` to `companya` (or whatever you named it), leaving the port number intact. This should run the appropriate portal for that company. You should be able to navigate to `http://companya:5153` in your web browser (using the domain name and port you specified earlier).

(*) No, I don't work for Telerik, nor do I have any affiliation with them. I'm just a satisifed customer!