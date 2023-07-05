using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityClaimsPlay.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel {
  private readonly SignInManager<User> _signInManager;
  private readonly AppDbContext _appDbContext;

  public LoginModel(SignInManager<User> signInManager, AppDbContext appDbContext) {
    _signInManager = signInManager;
    _appDbContext = appDbContext;
  }

  public List<User> Users { get; set; } = new();

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
    public string Password { get; set; } = "1";
  }

  public async Task OnGetAsync() {
    if (!string.IsNullOrEmpty(ErrorMessage)) {
      ModelState.AddModelError(string.Empty, ErrorMessage);
    }
    await SetUsers();
  }

  private async Task SetUsers() =>
    Users = await _appDbContext.Users.OrderBy(u => u.Email).ToListAsync();

  public async Task<IActionResult> OnPostAsync() {
    if (ModelState.IsValid) {
      User? user = await _appDbContext.Users.SingleOrDefaultAsync(u => u.Email == Input.Email && u.UserCompanyRoles.Any(c => c.Role == Roles.Admin.ToString()));
      if (user == null) {
        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        await SetUsers();
        return Page();
      }
      SignInResult credsCorrect = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);
      if (credsCorrect.Succeeded) {
        // TODO AYS - Do we need any claims here? If we're logged in, then we must be a global admin user
        await _signInManager.SignInWithClaimsAsync(user, true, Array.Empty<Claim>());
        return LocalRedirect("/");
      }
      if (credsCorrect.IsLockedOut) {
        return RedirectToPage("./Lockout");
      }
      ModelState.AddModelError(string.Empty, "Invalid login attempt");
      await SetUsers();
      return Page();
    }
    // If we got this far, something failed, redisplay form
    await SetUsers();
    return Page();
  }
}