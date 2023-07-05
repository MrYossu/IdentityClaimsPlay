namespace IdentityClaimsPlay.Asp.Helpers;

public class RouteHelper {
  public const string Home = @"/";

  public const string NotAuthorised = "/not-authorised";
  public const string Login = "/log-in";
  public const string Logout = "/log-out";
  public const string Profile = "/profile";
  public const string ForgotPassword = "/forgot-password"; // For them to request a link
  public const string ChangePassword = "/change-password"; // The link they got from the previous, where they set their new password
  public const string ResetPassword = "/reset-password"; // For logged-in users to change their password. Doesn't require a link

  public const string CharityList = "/charities";
  public const string CharityDetails = "/charity/";
  public const string CharityDetailsId = "/charity/{id}";
  public const string CompanyList = "/companies";
  public const string CompanyDetails = "/company/{id}";
  public const string DonorList = "/donors";
  public const string DonorDetails = "/donor/{id}";
  public const string UserList = "/users";
  public const string UserDetailsId = "/user/{id}";
  public const string UserDetails = "/user/";
}