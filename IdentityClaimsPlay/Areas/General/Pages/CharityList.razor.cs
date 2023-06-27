namespace IdentityClaimsPlay.Areas.General.Pages;

[Authorize(Policy = ClaimsHelper.UserCanViewCharities)]
public partial class CharityList {
  [Inject]
  public AppDbContext Context { get; set; } = null!;

  private List<Charity> _charities = new();

  protected override async Task OnInitializedAsync() =>
    _charities = await Context.Charities.OrderBy(c => c.Name).ToListAsync();
}