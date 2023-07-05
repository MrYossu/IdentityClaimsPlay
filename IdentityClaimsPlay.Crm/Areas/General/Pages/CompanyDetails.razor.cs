namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

[AuthoriseByRole(Roles.Admin)]
public partial class CompanyDetails {
  [Parameter]
  public string Id { get; set; } = "";

  [Inject]
  public NavigationManager NavigationManager { get; set; } = null!;

  private bool _loaded;
  private Company? _company;

  protected override async Task OnParametersSetAsync() {
    _company = Id == "new" ? new() : await Context.Companies.FirstOrDefaultAsync(c => c.Id == Id);
    _loaded = true;
  }

  private async Task Save() {
    if (Id == "new") {
      Context.Companies.Add(_company!);
    }
    await Context.SaveChangesAsync();
    NavigationManager.NavigateTo("/companies");
  }
}