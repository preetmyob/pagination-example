using SitesApi.Models;

namespace SitesApi.Services;

// Service interface for site business logic
public interface ISiteService
{
    // Retrieves sites with pagination and optional filtering
    // Parameters:
    // - page: Page number (will be corrected if invalid)
    // - size: Page size (will be corrected if invalid)
    // - filter: Optional filter criteria
    // Returns: Paginated result containing sites and metadata
    Task<PaginatedResult<Site>> GetSitesAsync(int page, int size, SiteFilter? filter = null);
}
