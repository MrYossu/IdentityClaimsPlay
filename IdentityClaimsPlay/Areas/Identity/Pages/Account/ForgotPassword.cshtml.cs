using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityClaimsPlay.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordModel : PageModel {
  private readonly UserManager<User> _userManager;

  public ForgotPasswordModel(UserManager<User> userManager) =>
    _userManager = userManager;

  [BindProperty]
  public InputModel Input { get; set; } = new();

  [TempData]
  public string Email { get; set; } = "";

  public class InputModel {
    [Required(ErrorMessage = "Required")]
    [EmailAddress(ErrorMessage = "Not a valid email")]
    public string Email { get; set; } = "";
  }

  public async Task<IActionResult> OnPostAsync() {
    if (ModelState.IsValid) {
      User? user = await _userManager.FindByEmailAsync(Input.Email);
      if (user == null || !await _userManager.IsEmailConfirmedAsync(user)) {
        // Don't reveal that the user does not exist or is not confirmed
        return RedirectToPage("ForgotPasswordConfirmation");
      }
      //string body = await _emailTemplatesService.GetForgottenPasswordEmailBody(user, Request.GetDisplayUrl());
      string url = Helpers.UrlHelper.CleanUrl(Request.GetDisplayUrl());
      if (url.EndsWith("/")) {
        url = url.Substring(0, url.Length - 1);
      }
      string code = await _userManager.GeneratePasswordResetTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
      string callbackUrl = $"{url}{RouteHelper.ResetPassword}?code={code}&email={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Input.Email))}";
      Email = user.Email!;
      return RedirectToPage("ForgotPasswordConfirmation");
    }
    return Page();
  }
}