using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using SitesApi.Models;
using SitesApi.Repositories;
using SitesApi.Tests.TestInfrastructure;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

namespace SitesApi.Tests.StepDefinitions;

[Binding]
public class SitesPaginationSteps
{
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private TestSiteRepository? _testRepository;
    private HttpResponseMessage? _response;
    private PaginatedResult<Site>? _result;

    [BeforeScenario]
    public void BeforeScenario()
    {
        _testRepository = new TestSiteRepository();
        
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real repository registration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISiteRepository));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                
                // Add test repository
                services.AddSingleton<ISiteRepository>(_testRepository);
            });
        });
        
        _client = _factory.CreateClient();
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Given(@"the following sites exist in the system:")]
    public void GivenTheFollowingSitesExistInTheSystem(Table table)
    {
        var sites = table.Rows.Select(row => new Site
        {
            SiteId = int.Parse(row["SiteId"]),
            SiteName = row["SiteName"],
            SiteUrl = row["SiteUrl"],
            SiteX = double.Parse(row["SiteX"]),
            SiteY = double.Parse(row["SiteY"])
        }).ToList();

        _testRepository!.SetTestData(sites);
    }

    [When(@"I request sites with no pagination parameters")]
    public async Task WhenIRequestSitesWithNoPaginationParameters()
    {
        _response = await _client!.GetAsync("/api/sites");
        await ProcessResponse();
    }

    [When(@"I request sites with page size (\d+)")]
    public async Task WhenIRequestSitesWithPageSize(int pageSize)
    {
        _response = await _client!.GetAsync($"/api/sites?size={pageSize}");
        await ProcessResponse();
    }

    [When(@"I request sites with page (\d+) and page size (\d+)")]
    [When(@"I request sites with page {int} and page size {int}")]
    public async Task WhenIRequestSitesWithPageAndPageSize(int page, int pageSize)
    {
        _response = await _client!.GetAsync($"/api/sites?page={page}&size={pageSize}");
        await ProcessResponse();
    }

    [When(@"I request sites with filter '([^']*)'")]
    public async Task WhenIRequestSitesWithFilter(string filter)
    {
        var encodedFilter = HttpUtility.UrlEncode(filter);
        _response = await _client!.GetAsync($"/api/sites?filter={encodedFilter}");
        await ProcessResponse();
    }

    [When(@"I request sites with filter '([^']*)' and page (\d+) and page size (\d+)")]
    public async Task WhenIRequestSitesWithFilterAndPageAndPageSize(string filter, int page, int pageSize)
    {
        var encodedFilter = HttpUtility.UrlEncode(filter);
        _response = await _client!.GetAsync($"/api/sites?filter={encodedFilter}&page={page}&size={pageSize}");
        await ProcessResponse();
    }

    [When(@"I request sites with invalid filter '([^']*)'")]
    public async Task WhenIRequestSitesWithInvalidFilter(string filter)
    {
        var encodedFilter = HttpUtility.UrlEncode(filter);
        _response = await _client!.GetAsync($"/api/sites?filter={encodedFilter}");
        // Don't process response for invalid requests
    }

    [When(@"I POST to search endpoint with:")]
    public async Task WhenIPostToSearchEndpointWith(string requestBody)
    {
        var content = JsonContent.Create(JsonSerializer.Deserialize<object>(requestBody));
        _response = await _client!.PostAsync("/api/sites/search", content);
        await ProcessResponse();
    }

    [Then(@"the response should contain (\d+) sites")]
    public void ThenTheResponseShouldContainSites(int expectedCount)
    {
        _result.Should().NotBeNull();
        _result!.Items.Should().HaveCount(expectedCount);
    }

    [Then(@"the response should have totalCount of (\d+)")]
    public void ThenTheResponseShouldHaveTotalCountOf(int expectedTotalCount)
    {
        _result.Should().NotBeNull();
        _result!.TotalCount.Should().Be(expectedTotalCount);
    }

    [Then(@"the response should have pageNumber of (\d+)")]
    public void ThenTheResponseShouldHavePageNumberOf(int expectedPageNumber)
    {
        _result.Should().NotBeNull();
        _result!.PageNumber.Should().Be(expectedPageNumber);
    }

    [Then(@"the response should have pageSize of (\d+)")]
    public void ThenTheResponseShouldHavePageSizeOf(int expectedPageSize)
    {
        _result.Should().NotBeNull();
        _result!.PageSize.Should().Be(expectedPageSize);
    }

    [Then(@"the response should have totalPages of (\d+)")]
    public void ThenTheResponseShouldHaveTotalPagesOf(int expectedTotalPages)
    {
        _result.Should().NotBeNull();
        _result!.TotalPages.Should().Be(expectedTotalPages);
    }

    [Then(@"the response should have hasPrevious as (true|false)")]
    public void ThenTheResponseShouldHaveHasPreviousAs(bool expectedHasPrevious)
    {
        _result.Should().NotBeNull();
        _result!.HasPrevious.Should().Be(expectedHasPrevious);
    }

    [Then(@"the response should have hasNext as (true|false)")]
    public void ThenTheResponseShouldHaveHasNextAs(bool expectedHasNext)
    {
        _result.Should().NotBeNull();
        _result!.HasNext.Should().Be(expectedHasNext);
    }

    // Specific step definitions for ReqnRoll
    [Then(@"the response should have hasPrevious as true")]
    public void ThenTheResponseShouldHaveHasPreviousAsTrue()
    {
        _result.Should().NotBeNull();
        _result!.HasPrevious.Should().BeTrue();
    }

    [Then(@"the response should have hasPrevious as false")]
    public void ThenTheResponseShouldHaveHasPreviousAsFalse()
    {
        _result.Should().NotBeNull();
        _result!.HasPrevious.Should().BeFalse();
    }

    [Then(@"the response should have hasNext as true")]
    public void ThenTheResponseShouldHaveHasNextAsTrue()
    {
        _result.Should().NotBeNull();
        _result!.HasNext.Should().BeTrue();
    }

    [Then(@"the response should have hasNext as false")]
    public void ThenTheResponseShouldHaveHasNextAsFalse()
    {
        _result.Should().NotBeNull();
        _result!.HasNext.Should().BeFalse();
    }

    [Then(@"the first site should have name ""([^""]*)""")]
    public void ThenTheFirstSiteShouldHaveName(string expectedName)
    {
        _result.Should().NotBeNull();
        _result!.Items.Should().NotBeEmpty();
        _result.Items.First().SiteName.Should().Be(expectedName);
    }

    [Then(@"the sites should contain names:")]
    public void ThenTheSitesShouldContainNames(Table table)
    {
        _result.Should().NotBeNull();
        var expectedNames = table.Rows.Select(row => row["SiteName"]).ToList();
        var actualNames = _result!.Items.Select(s => s.SiteName).ToList();
        actualNames.Should().BeEquivalentTo(expectedNames);
    }

    [Then(@"the response should return status code (\d+)")]
    public void ThenTheResponseShouldReturnStatusCode(int expectedStatusCode)
    {
        _response.Should().NotBeNull();
        _response!.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }

    [Then(@"the response should contain error message about invalid JSON")]
    public async Task ThenTheResponseShouldContainErrorMessageAboutInvalidJson()
    {
        _response.Should().NotBeNull();
        var content = await _response!.Content.ReadAsStringAsync();
        content.Should().ContainEquivalentOf("invalid");
    }

    private async Task ProcessResponse()
    {
        _response.Should().NotBeNull();
        _response!.EnsureSuccessStatusCode();
        
        var content = await _response.Content.ReadAsStringAsync();
        _result = JsonSerializer.Deserialize<PaginatedResult<Site>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
