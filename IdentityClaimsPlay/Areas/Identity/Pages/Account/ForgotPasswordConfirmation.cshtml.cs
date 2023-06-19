using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityClaimsPlay.Areas.Identity.Pages.Account; 

[AllowAnonymous]
public class ForgotPasswordConfirmation : PageModel {
  [TempData]
  public string Email { get; set; } = "";

  public void OnGet() {
  }
}