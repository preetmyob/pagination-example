namespace SitesApi.Models;

// Filter criteria for site queries
public class SiteFilter
{
    // Filter by site name (partial match, case-insensitive)
    public string? SiteName { get; set; }
}
