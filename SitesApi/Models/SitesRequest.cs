namespace SitesApi.Models;

// Request model for the POST /api/sites/search endpoint
public class SitesRequest
{
    // Page number (1-based, default: 1)
    public int Page { get; set; } = 1;

    // Page size (default: 10, max: 1000)
    public int Size { get; set; } = 10;

    // Filter criteria
    public SiteFilter Filter { get; set; } = new();
}
