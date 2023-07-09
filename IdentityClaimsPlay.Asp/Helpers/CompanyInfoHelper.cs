using Microsoft.EntityFrameworkCore;

namespace IdentityClaimsPlay.Asp.Helpers;

public class CompanyInfoHelper {
  private readonly AppDbContext _context;
  private readonly IDbContextFactory<AppDbContext> _contextFactory;

  public CompanyInfoHelper(AppDbContext context, IDbContextFactory<AppDbContext> contextFactory) {
    _context = context;
    _contextFactory = contextFactory;
  }

  public async Task<CompanyInfo> GetCompanyInfo(string uri) {
    string domain = uri.Replace("https://", "").Replace("/", "");
    Company? company = await (await _contextFactory.CreateDbContextAsync()).Companies.SingleOrDefaultAsync(c => domain.Contains(c.Domain));
    if (company is not null) {
      return new(company.Id, company.Name, company.Domain);
    }
    return new("", "", "");
  }
}