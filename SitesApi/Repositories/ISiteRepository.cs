using SitesApi.Models;

namespace SitesApi.Repositories;

// Repository interface for site data access
public interface ISiteRepository
{
    // Retrieves sites with pagination and optional filtering
    // Parameters:
    // - page: Page number (1-based)
    // - size: Page size
    // - filter: Optional filter criteria
    // Returns: Tuple containing the sites and total count
    Task<(IEnumerable<Site> Sites, int TotalCount)> GetSitesAsync(int page, int size, SiteFilter? filter = null);
}
