namespace IdentityClaimsPlay.Helpers; 

public static class UrlHelper {
  public static string CleanUrl(string url) {
    if (url.IndexOf("/", 9) > 0) {
      url = url[..url.IndexOf("/", 9)];
    }
    if (!url.EndsWith("/")) {
      url += "/";
    }
    return url;
  }

}