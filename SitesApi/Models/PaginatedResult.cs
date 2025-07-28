namespace SitesApi.Models;

// Represents a paginated result set
public class PaginatedResult<T>
{
    // The items in the current page
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    // Total number of items across all pages
    public int TotalCount { get; set; }

    // Current page number (1-based)
    public int PageNumber { get; set; }

    // Number of items per page
    public int PageSize { get; set; }

    // Total number of pages
    public int TotalPages { get; set; }

    // Indicates if there is a previous page
    public bool HasPrevious { get; set; }

    // Indicates if there is a next page
    public bool HasNext { get; set; }
}
