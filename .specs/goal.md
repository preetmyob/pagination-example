# Agent Prompt: Generate Complete Sites API Solution

## Objective
Create a complete Visual Studio solution with a production-ready ASP.NET 9 Web API and comprehensive BDD test suite. The solution should be professional-grade, well-structured, and ready for immediate use.

## Technical Requirements

### Core Technology Stack
- **Framework**: ASP.NET 9 Web API
- **Database**: MySQL 8 with Dapper ORM
- **Testing**: xUnit with ReqnRoll (BDD framework)
- **Assertions**: FluentAssertions
- **Documentation**: OpenAPI/Swagger integration

### Database Context
- Table name: `Sites`
- Maximum 50,000 records
- Fields: `site_id` (int, primary key), `site_name` (varchar 255), `site_url` (varchar 500), `site_y` (double), `site_x` (double)
- Index on `site_name` for filtering performance

## Functional Requirements

### API Endpoints
1. **GET /api/sites** - Paginated sites retrieval with optional filtering
   - Query parameters: `page` (default 1), `size` (default 10, max 1000), `filter` (JSON string)
   - Filter supports: `siteName` (partial string matching, case-insensitive)
   - Returns: Paginated result with metadata

2. **POST /api/sites/search** - Alternative endpoint for complex filtering (future-ready)
   - Request body: JSON with pagination and filter parameters
   - Same response format as GET endpoint

### Response Format
```json
{
  "items": [/* array of Site objects */],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 50,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true
}
```

### Error Handling
- 400 Bad Request for invalid filter JSON
- 500 Internal Server Error for system failures
- Input validation with automatic correction (page < 1 → 1, size < 1 → 10, size > 1000 → 1000)

## Architecture Requirements

### Clean Architecture Pattern
- **Controllers**: Handle HTTP requests/responses, parameter validation
- **Services**: Business logic layer
- **Repositories**: Data access layer with interface abstraction
- **Models**: Domain entities and DTOs

### Dependency Injection
- Register all services and repositories in Program.cs
- Use interface-based dependency injection
- Scoped lifetime for database connections

### Code Quality Standards
- Enable nullable reference types
- Use implicit usings
- Follow C# naming conventions
- Implement proper async/await patterns
- Include XML documentation for public APIs

## Testing Requirements

### BDD Test Coverage with ReqnRoll
Create comprehensive feature files covering:

#### Pagination Scenarios
- Default page size behavior
- Custom page sizes
- Navigation between pages  
- Edge cases (page beyond data, invalid parameters)
- Parameter validation and correction

#### Filtering Scenarios
- Exact name matching
- Partial name matching (case-insensitive)
- No results scenarios
- Combined filtering with pagination
- Invalid JSON filter handling

#### Integration Testing
- Use WebApplicationFactory for full integration tests
- Mock repository with in-memory test data
- Test actual HTTP endpoints
- Validate complete request/response cycle

### Test Structure
- **Feature files**: Written in Gherkin syntax with business-readable scenarios
- **Step definitions**: Map Gherkin steps to C# test code
- **Test infrastructure**: Mock repositories, test data setup
- **Background sections**: Common test data setup
- **Table-driven tests**: Use Gherkin tables for test data

## Solution Structure Requirements

```
SitesApi/
├── SitesApi.sln
├── SitesApi/                          # Main API project
│   ├── SitesApi.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Controllers/
│   │   └── SitesController.cs
│   ├── Models/
│   │   ├── Site.cs
│   │   ├── PaginatedResult.cs
│   │   ├── SiteFilter.cs
│   │   └── SitesRequest.cs
│   ├── Services/
│   │   ├── ISiteService.cs
│   │   └── SiteService.cs
│   └── Repositories/
│       ├── ISiteRepository.cs
│       └── SiteRepository.cs
├── SitesApi.Tests/                    # BDD test project
│   ├── SitesApi.Tests.csproj
│   ├── reqnroll.json
│   ├── Features/
│   │   ├── SitesPagination.feature
│   │   └── SitesFiltering.feature
│   ├── StepDefinitions/
│   │   └── SitesPaginationSteps.cs
│   └── TestInfrastructure/
│       └── TestSiteRepository.cs
├── Database/
│   └── schema.sql                     # MySQL table creation script
└── Documentation/
    └── openapi.yaml                   # Complete API specification
```

## Specific Implementation Details

### MySQL Connection
- Use MySqlConnection from MySql.Data package
- Connection string template: "Server=localhost;Database=sitesdb;Uid=username;Pwd=password;charset=utf8mb4;"
- Implement proper connection management through DI

### Dapper Implementation
- Use parameterized queries to prevent SQL injection
- Implement LIMIT/OFFSET for MySQL pagination
- Use LIKE operator for partial string matching
- Build dynamic WHERE clauses based on filters

### ReqnRoll Configuration
- Configure for xUnit test runner
- Use en-US culture for consistent test execution
- Include proper JSON configuration file

### Package References
**API Project:**
- Microsoft.NET.Sdk.Web
- Dapper (latest stable)
- MySql.Data (8.4.0 or compatible)
- Swashbuckle.AspNetCore (for Swagger)

**Test Project:**
- Microsoft.NET.Test.Sdk
- Reqnroll (latest stable)
- Reqnroll.xUnit
- Microsoft.AspNetCore.Mvc.Testing
- FluentAssertions
- xunit
- xunit.runner.visualstudio

## Deliverables Expected

1. **Complete Visual Studio Solution** (.sln file)
2. **Fully functional API project** with all source files
3. **Comprehensive test suite** with BDD scenarios
4. **Database schema script** for MySQL
5. **OpenAPI specification** (YAML format)
6. **README with setup instructions**

## Quality Criteria

- Solution must compile without errors or warnings
- All tests must pass when executed
- API must be testable via Swagger UI
- Code must follow SOLID principles
- Proper error handling and logging
- Production-ready configuration management
- Clear separation of concerns
- Comprehensive test coverage of all scenarios

## Setup Instructions to Include

1. Prerequisites (.NET 9 SDK, MySQL 8)
2. Database setup steps
3. Connection string configuration
4. Package restoration commands
5. Running the API and tests
6. Accessing Swagger documentation

Generate the complete solution as individual files that can be extracted into the specified folder structure. Each file should be complete and functional, ready for immediate use in a development environment.