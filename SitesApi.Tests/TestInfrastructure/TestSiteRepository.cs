using SitesApi.Models;
using SitesApi.Repositories;

namespace SitesApi.Tests.TestInfrastructure;

public class TestSiteRepository : ISiteRepository
{
    private readonly List<Site> _sites = new();

    public void SetTestData(IEnumerable<Site> sites)
    {
        _sites.Clear();
        _sites.AddRange(sites);
    }

    public async Task<(IEnumerable<Site> Sites, int TotalCount)> GetSitesAsync(
        int page, 
        int size, 
        SiteFilter? filter = null)
    {
        await Task.Delay(1); // Simulate async operation

        var query = _sites.AsQueryable();

        // Apply filtering
        if (filter?.SiteName != null && !string.IsNullOrWhiteSpace(filter.SiteName))
        {
            query = query.Where(s => s.SiteName.Contains(filter.SiteName, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = query.Count();

        // Apply pagination
        var sites = query
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        return (sites, totalCount);
    }
}
