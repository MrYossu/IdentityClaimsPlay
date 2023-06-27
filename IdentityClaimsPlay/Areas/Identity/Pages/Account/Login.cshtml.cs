using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityClaimsPlay.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel {
  private readonly SignInManager<User> _signInManager;
  private readonly ILogger<LoginModel> _logger;

  public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger) {
    _signInManager = signInManager;
    _logger = logger;
  }

  [BindProperty]
  public InputModel Input { get; set; } = new();

  [TempData]
  public string ErrorMessage { get; set; } = "";

  public class InputModel {
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
  }

  public void OnGet() {
    if (!string.IsNullOrEmpty(ErrorMessage)) {
      ModelState.AddModelError(string.Empty, ErrorMessage);
    }
  }

  public async Task<IActionResult> OnPostAsync() {
    if (ModelState.IsValid) {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, true);
      if (result.Succeeded) {
        _logger.LogInformation("User logged in.");
        return LocalRedirect("/");
      }
      if (result.IsLockedOut) {
        _logger.LogWarning("User account locked out.");
        return RedirectToPage("./Lockout");
      }
      ModelState.AddModelError(string.Empty, "Invalid login attempt.");
      return Page();
    }

    // If we got this far, something failed, redisplay form
    return Page();
  }
}