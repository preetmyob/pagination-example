using Microsoft.AspNetCore.Mvc;
using SitesApi.Models;
using SitesApi.Services;
using System.Text.Json;

namespace SitesApi.Controllers;

// Controller for managing sites
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SitesController : ControllerBase
{
    private readonly ISiteService _siteService;
    private readonly ILogger<SitesController> _logger;

    public SitesController(ISiteService siteService, ILogger<SitesController> logger)
    {
        _siteService = siteService;
        _logger = logger;
    }

    // Retrieves sites with pagination and optional filtering
    // Parameters:
    // - page: Page number (default: 1)
    // - size: Page size (default: 10, max: 1000)
    // - filter: JSON filter string (optional)
    // Returns: Paginated list of sites
    // Response codes:
    // - 200: Returns the paginated list of sites
    // - 400: If the filter JSON is invalid
    // - 500: If there was an internal server error
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<Site>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedResult<Site>>> GetSites(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? filter = null)
    {
        try
        {
            SiteFilter? siteFilter = null;

            // Parse filter if provided
            if (!string.IsNullOrWhiteSpace(filter))
            {
                try
                {
                    siteFilter = JsonSerializer.Deserialize<SiteFilter>(filter, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Invalid JSON filter provided: {Filter}", filter);
                    return BadRequest(new { error = "Invalid JSON filter format" });
                }
            }

            var result = await _siteService.GetSitesAsync(page, size, siteFilter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sites");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    // Searches sites using POST request with complex filtering
    // Parameters:
    // - request: Search request containing pagination and filter parameters
    // Returns: Paginated list of sites
    // Response codes:
    // - 200: Returns the paginated list of sites
    // - 400: If the request is invalid
    // - 500: If there was an internal server error
    [HttpPost("search")]
    [ProducesResponseType(typeof(PaginatedResult<Site>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedResult<Site>>> SearchSites([FromBody] SitesRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { error = "Request body is required" });
            }

            var result = await _siteService.GetSitesAsync(request.Page, request.Size, request.Filter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching sites");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
}
