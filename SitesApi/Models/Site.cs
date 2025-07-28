namespace SitesApi.Models;

// Represents a site entity
public class Site
{
    // Unique identifier for the site
    public int SiteId { get; set; }

    // Name of the site
    public string SiteName { get; set; } = string.Empty;

    // URL of the site
    public string SiteUrl { get; set; } = string.Empty;

    // X coordinate of the site
    public double SiteX { get; set; }

    // Y coordinate of the site
    public double SiteY { get; set; }
}
