using SitesApi.Models;
using SitesApi.Repositories;

namespace SitesApi.Services;

// Service implementation for site business logic
public class SiteService : ISiteService
{
    private readonly ISiteRepository _siteRepository;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 1000;

    public SiteService(ISiteRepository siteRepository)
    {
        _siteRepository = siteRepository;
    }

    // Retrieves sites with pagination and optional filtering
    public async Task<PaginatedResult<Site>> GetSitesAsync(int page, int size, SiteFilter? filter = null)
    {
        // Validate and correct parameters
        var correctedPage = Math.Max(1, page);
        var correctedSize = size switch
        {
            <= 0 => DefaultPageSize,
            > MaxPageSize => MaxPageSize,
            _ => size
        };

        // Get data from repository
        var (sites, totalCount) = await _siteRepository.GetSitesAsync(correctedPage, correctedSize, filter);

        // Calculate pagination metadata
        var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / correctedSize);
        var hasPrevious = correctedPage > 1;
        var hasNext = correctedPage < totalPages;

        return new PaginatedResult<Site>
        {
            Items = sites,
            TotalCount = totalCount,
            PageNumber = correctedPage,
            PageSize = correctedSize,
            TotalPages = totalPages,
            HasPrevious = hasPrevious,
            HasNext = hasNext
        };
    }
}
