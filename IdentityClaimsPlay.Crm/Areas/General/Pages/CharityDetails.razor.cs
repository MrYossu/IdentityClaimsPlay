﻿namespace IdentityClaimsPlay.Crm.Areas.General.Pages;

[AuthoriseByPermission(Permissions.CanViewCharities)]
public partial class CharityDetails {
  [Parameter]
  public string Id { get; set; } = "";

  [Inject]
  public AppDbContext Context { get; set; } = null!;

  [Inject]
  public NavigationManager NavigationManager { get; set; } = null!;

  private bool _loaded;
  private Company _company = null!;
  private Charity? _charity;
  private bool _canEdit;

  protected override async Task OnInitializedAsync() {
    string email = UserHelper.Email;
    User me = await Context.Users.SingleAsync(u => u.Email == email);
    List<string> myPermissions = UserHelper.Permissions.Select(c => c.Value).ToList();
    _canEdit = myPermissions.Any(p => p == Permissions.CanEditCharities.ToString());
    _company = await Context.Companies.SingleAsync(c => c.Id == CompanyInfo.Id);
  }

  protected override async Task OnParametersSetAsync() {
    _charity = Id == "new" ? new() { Company = _company } : await Context.Charities.FirstOrDefaultAsync(c => c.Id == Id);
    _loaded = true;
  }

  private async Task Save() {
    if (Id == "new") {
      Context.Charities.Add(_charity!);
    }
    await Context.SaveChangesAsync();
    NavigationManager.NavigateTo(RouteHelper.CharityList);
  }
}