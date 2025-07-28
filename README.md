# Sites API

A production-ready ASP.NET 9 Web API for managing sites with comprehensive pagination, filtering, and BDD test coverage.

## Features

### Core API Capabilities
- **ASP.NET 9 Web API** with clean architecture pattern
- **MySQL 8** database integration using Dapper ORM
- **Two API endpoints** for maximum flexibility:
  - `GET /api/sites` - Query parameter based requests
  - `POST /api/sites/search` - Request body based searches

### Pagination Features
- **Smart pagination** with comprehensive metadata response
- **Default page size** of 10 items with customizable sizing
- **Maximum page size** enforcement (1000 items max)
- **Automatic parameter correction**:
  - Invalid page numbers (≤0) corrected to page 1
  - Invalid page sizes (≤0) corrected to default size 10
  - Oversized page requests (>1000) limited to 1000
- **Navigation metadata** including `hasPrevious`, `hasNext`, `totalPages`
- **Beyond-data handling** - graceful responses for pages beyond available data

### Filtering Capabilities
- **Case-insensitive partial matching** on site names
- **Exact name matching** support
- **Empty filter handling** - returns all sites when filter is empty
- **Combined filtering and pagination** - filters work seamlessly with pagination
- **Flexible filter formats**:
  - JSON string in query parameters: `filter={"siteName":"alpha"}`
  - JSON object in POST request body: `"filter": {"siteName": "alpha"}`

### Data Validation & Error Handling
- **JSON validation** with proper 400 Bad Request responses for malformed filters
- **Parameter validation** with automatic correction rather than rejection
- **Robust error responses** with meaningful error messages
- **Production-ready** error handling for database and system failures

### Testing & Documentation
- **Comprehensive BDD test suite** using ReqnRoll (20 test scenarios)
- **Living documentation** through Gherkin feature files
- **Integration testing** with full HTTP request/response cycle validation
- **OpenAPI/Swagger** documentation with interactive UI
- **Test coverage** for all pagination, filtering, and error scenarios

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL 8.0](https://dev.mysql.com/downloads/mysql/) or compatible
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) (optional)

## Quick Start

### 1. Database Setup

1. Install and start MySQL 8.0
2. Run the database schema script:
   ```bash
   mysql -u root -p < Database/schema.sql
   ```

### 2. Configuration

Update the connection string in `SitesApi/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sitesdb;Uid=your_username;Pwd=your_password;charset=utf8mb4;"
  }
}
```

### 3. Build and Run

```bash
# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the API
cd SitesApi
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7000`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:7000` (root path)

### 4. Run Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## API Endpoints

### GET /api/sites

Retrieve sites with pagination and optional filtering.

**Query Parameters:**
- `page` (int, optional): Page number (default: 1)
- `size` (int, optional): Page size (default: 10, max: 1000)
- `filter` (string, optional): JSON filter string

**Example:**
```bash
curl "https://localhost:7000/api/sites?page=1&size=5&filter={\"siteName\":\"alpha\"}"
```

### POST /api/sites/search

Search sites using a POST request body.

**Request Body:**
```json
{
  "page": 1,
  "size": 10,
  "filter": {
    "siteName": "test"
  }
}
```

**Example:**
```bash
curl -X POST "https://localhost:7000/api/sites/search" \
  -H "Content-Type: application/json" \
  -d '{"page":1,"size":5,"filter":{"siteName":"alpha"}}'
```

## Response Format

Both endpoints return the same paginated response format:

```json
{
  "items": [
    {
      "siteId": 1,
      "siteName": "Alpha Site",
      "siteUrl": "https://alpha.example.com",
      "siteX": 10.5,
      "siteY": 20.3
    }
  ],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 15,
  "hasPrevious": false,
  "hasNext": true
}
```

## Project Structure

```
SitesApi/
├── SitesApi.sln                       # Visual Studio solution
├── SitesApi/                          # Main API project
│   ├── Controllers/SitesController.cs # API endpoints
│   ├── Models/                        # Domain models and DTOs
│   ├── Services/                      # Business logic layer
│   ├── Repositories/                  # Data access layer
│   └── Program.cs                     # Application entry point
├── SitesApi.Tests/                    # BDD test project
│   ├── Features/                      # Gherkin feature files
│   ├── StepDefinitions/               # Test step implementations
│   └── TestInfrastructure/            # Test utilities
├── Database/schema.sql                # MySQL database schema
├── Documentation/openapi.yaml         # OpenAPI specification
└── README.md                          # This file
```

## Architecture

The solution follows **Clean Architecture** principles:

- **Controllers**: Handle HTTP requests/responses and parameter validation
- **Services**: Contain business logic and parameter correction
- **Repositories**: Provide data access abstraction
- **Models**: Define domain entities and data transfer objects

## Testing

The project includes comprehensive BDD tests using **ReqnRoll** (the successor to SpecFlow):

### Test Coverage

- **Pagination scenarios**: Default behavior, custom page sizes, navigation, edge cases
- **Filtering scenarios**: Exact/partial matching, case-insensitive search, no results
- **Integration tests**: Full HTTP request/response cycle testing
- **Parameter validation**: Automatic correction of invalid parameters

### Running Specific Tests

```bash
# Run only pagination tests
dotnet test --filter "TestCategory=Pagination"

# Run with coverage (requires coverlet)
dotnet test --collect:"XPlat Code Coverage"
```

## Database Schema

The `Sites` table structure:

```sql
CREATE TABLE Sites (
    site_id INT AUTO_INCREMENT PRIMARY KEY,
    site_name VARCHAR(255) NOT NULL,
    site_url VARCHAR(500) NOT NULL,
    site_x DOUBLE NOT NULL,
    site_y DOUBLE NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE INDEX idx_sites_name ON Sites(site_name);
```

## Configuration Options

### Connection Strings

Configure in `appsettings.json` or environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sitesdb;Uid=username;Pwd=password;charset=utf8mb4;"
  }
}
```

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to `Development`, `Staging`, or `Production`
- `ConnectionStrings__DefaultConnection`: Override connection string

## Error Handling

The API provides consistent error responses:

- **400 Bad Request**: Invalid filter JSON or malformed requests
- **500 Internal Server Error**: Database connection issues or unexpected errors

## Performance Considerations

- **Database indexing**: Index on `site_name` for efficient filtering
- **Pagination limits**: Maximum page size of 1000 to prevent large result sets
- **Connection management**: Scoped database connections through dependency injection
- **Async operations**: All database operations are asynchronous

## Development

### Adding New Filters

1. Update the `SiteFilter` model
2. Modify the repository query logic
3. Add corresponding BDD test scenarios
4. Update the OpenAPI documentation

### Extending the API

The clean architecture makes it easy to:
- Add new endpoints
- Implement additional filtering criteria
- Support different data sources
- Add caching layers

## Production Deployment

### Docker Support

Create a `Dockerfile` for containerized deployment:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SitesApi/SitesApi.csproj", "SitesApi/"]
RUN dotnet restore "SitesApi/SitesApi.csproj"
COPY . .
WORKDIR "/src/SitesApi"
RUN dotnet build "SitesApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SitesApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SitesApi.dll"]
```

### Environment Configuration

For production, consider:
- Using Azure Key Vault or AWS Secrets Manager for connection strings
- Implementing health checks
- Adding application insights/monitoring
- Configuring CORS policies
- Setting up reverse proxy (nginx/IIS)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Add BDD tests for new functionality
4. Implement the feature following TDD principles
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions or issues:
- Create an issue in the repository
- Check the OpenAPI documentation at the root URL when running
- Review the BDD test scenarios for usage examples
