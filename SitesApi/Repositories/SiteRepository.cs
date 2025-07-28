using Dapper;
using MySql.Data.MySqlClient;
using SitesApi.Models;
using System.Text;

namespace SitesApi.Repositories;

// MySQL implementation of the site repository using Dapper
public class SiteRepository : ISiteRepository
{
    private readonly string _connectionString;

    public SiteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    // Retrieves sites with pagination and optional filtering
    public async Task<(IEnumerable<Site> Sites, int TotalCount)> GetSitesAsync(int page, int size, SiteFilter? filter = null)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var whereClause = new StringBuilder();
        var parameters = new DynamicParameters();

        // Build WHERE clause based on filter
        if (filter?.SiteName != null && !string.IsNullOrWhiteSpace(filter.SiteName))
        {
            whereClause.Append("WHERE site_name LIKE @SiteName");
            parameters.Add("@SiteName", $"%{filter.SiteName}%");
        }

        // Get total count
        var countQuery = $"SELECT COUNT(*) FROM Sites {whereClause}";
        var totalCount = await connection.QuerySingleAsync<int>(countQuery, parameters);

        // Get paginated data
        var dataQuery = $@"
            SELECT site_id as SiteId, site_name as SiteName, site_url as SiteUrl, 
                   site_x as SiteX, site_y as SiteY 
            FROM Sites 
            {whereClause}
            ORDER BY site_id
            LIMIT @Size OFFSET @Offset";

        parameters.Add("@Size", size);
        parameters.Add("@Offset", (page - 1) * size);

        var sites = await connection.QueryAsync<Site>(dataQuery, parameters);

        return (sites, totalCount);
    }
}
