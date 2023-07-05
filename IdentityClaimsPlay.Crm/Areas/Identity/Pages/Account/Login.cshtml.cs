using System.ComponentModel.DataAnnotations;
using IdentityClaimsPlay.Crm.Data;
using IdentityClaimsPlay.Crm.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityClaimsPlay.Crm.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel {
  private readonly SignInManager<User> _signInManager;
  private readonly AppDbContext _appDbContext;
  private readonly ILogger<LoginModel> _logger;

  public LoginModel(SignInManager<User> signInManager, AppDbContext appDbContext, ILogger<LoginModel> logger) {
    _signInManager = signInManager;
    _appDbContext = appDbContext;
    _logger = logger;
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
    Users = await _appDbContext.Users.Include(u => u.Company).OrderBy(u => u.Company.Name).ThenBy(u => u.Email).ToListAsync();
  }

  public async Task<IActionResult> OnPostAsync() {
    if (ModelState.IsValid) {
      User? user = await _appDbContext.Users.Include(u => u.Company).SingleOrDefaultAsync(u => u.Email == Input.Email);
      if (user == null) {
        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return Page();
      }
      SignInResult credsCorrect = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);
      if (credsCorrect.Succeeded) {
        Claim[] customClaims = { new(ClaimsHelper.CompanyId, user.CompanyId ?? ""), new(ClaimsHelper.CompanyName, user.Company?.Name ?? "") };
        await _signInManager.SignInWithClaimsAsync(user, true, customClaims);
        return LocalRedirect("/");
      }
      if (credsCorrect.IsLockedOut) {
        _logger.LogWarning("User account locked out.");
        return RedirectToPage("./Lockout");
      }
      ModelState.AddModelError(string.Empty, "Invalid login attempt");
      return Page();
    }
    // If we got this far, something failed, redisplay form
    return Page();
  }
}