using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityClaimsPlay.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordModel : PageModel {
  private readonly UserManager<User> _userManager;
  private readonly SignInManager<User> _signInManager;

  public ResetPasswordModel(UserManager<User> userManager, SignInManager<User> signInManager) {
    _userManager = userManager;
    _signInManager = signInManager;
  }

  [BindProperty]
  public InputModel Input { get; set; } = new();

  public string PasswordRules { get; set; } = "";
  public string Reset { get; set; } = "";
  public string ResetCapital { get; set; } = "";

  public class InputModel {
    [Required(ErrorMessage = "Required")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [StringLength(100, ErrorMessage = "Must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The two passwords do not match")]
    public string ConfirmPassword { get; set; } = "";

    public string Code { get; set; } = "";
  }

  public IActionResult OnGet(string code, string email, string? r = null) {
    if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(email)) {
      ViewData["Error"] = true;
      return Page();
    }
    Input = new InputModel {
      Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
      Email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(email))
    };
    Reset = string.IsNullOrWhiteSpace(r) ? "reset" : "set";
    ResetCapital = string.IsNullOrWhiteSpace(r) ? "Reset" : "Set";
    return Page();
  }

  public async Task<IActionResult> OnPostAsync() {
    if (!ModelState.IsValid) {
      return Page();
    }
    User? user = await _userManager.FindByEmailAsync(Input.Email);
    if (user == null) {
      //ViewData["Error"] = true;
      ModelState.AddModelError("Email", "No such User");
      return Page();
    }
    IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
    if (result.Succeeded) {
      await _signInManager.SignInAsync(user, true);
      return Redirect("/reset-password-confirmation");
    }
    //ViewData["Error"] = true;
    ViewData["Msg"] = string.Join(", ", result.Errors.Select(e => e.Description));
    foreach (IdentityError error in result.Errors) {
      ModelState.AddModelError(string.Empty, error.Description);
    }
    return Page();
  }
}