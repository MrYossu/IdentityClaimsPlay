using System.ComponentModel.DataAnnotations;
using IdentityClaimsPlay.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityClaimsPlay.Crm.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel {
  private readonly SignInManager<User> _signInManager;
  private readonly AppDbContext _appDbContext;
  private readonly CompanyInfoHelper _companyInfoHelper;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public LoginModel(SignInManager<User> signInManager, AppDbContext appDbContext, CompanyInfoHelper companyInfoHelper, IHttpContextAccessor httpContextAccessor) {
    _signInManager = signInManager;
    _appDbContext = appDbContext;
    _companyInfoHelper = companyInfoHelper;
    _httpContextAccessor = httpContextAccessor;
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
    // TODO AYS - Would be helpful to include the user's company and roles, so we can test easily
    Users = await _appDbContext.Users.OrderBy(u => u.Email).ToListAsync();

  public async Task<IActionResult> OnPostAsync() {
    if (ModelState.IsValid) {
      CompanyInfo companyInfo = await _companyInfoHelper.GetCompanyInfo(_httpContextAccessor.HttpContext?.Request.Host.ToString() ?? "Jim"); // Jim added just in case someone managed to sneak a company into the database without specifying the domain
      User? user = await _appDbContext.Users.SingleOrDefaultAsync(u => u.Email == Input.Email && u.UserCompanyRoles.Any(c => c.CompanyId == companyInfo.Id && (c.Role == Roles.CardIssuerAdmin.ToString() || c.Role == Roles.CardIssuerUser.ToString())));
      if (user == null) {
        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        await SetUsers();
        return Page();
      }
      SignInResult credsCorrect = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);
      if (credsCorrect.Succeeded) {
        Claim[] customClaims = { new(ClaimsHelper.CompanyId, companyInfo.Id), new(ClaimsHelper.CompanyName, companyInfo.Name) };
        await _signInManager.SignInWithClaimsAsync(user, true, customClaims);
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