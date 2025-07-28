Feature: Sites API Filtering
    As an API consumer
    I want to filter sites by name
    So that I can find specific sites efficiently

Background:
    Given the following sites exist in the system:
        | SiteId | SiteName        | SiteUrl                    | SiteX | SiteY |
        | 1      | Alpha Site      | https://alpha.example.com  | 10.5  | 20.3  |
        | 2      | Beta Testing    | https://beta.example.com   | 15.2  | 25.7  |
        | 3      | Gamma Ray       | https://gamma.example.com  | 12.8  | 18.9  |
        | 4      | Alpha Beta      | https://alphabeta.com      | 22.1  | 30.4  |
        | 5      | Production Site | https://prod.example.com   | 8.7   | 14.2  |
        | 6      | Test Environment| https://test.example.com   | 19.3  | 27.6  |
        | 7      | Development     | https://dev.example.com    | 11.9  | 21.8  |
        | 8      | Staging Area    | https://staging.com        | 16.4  | 23.1  |

Scenario: Filter by exact site name match
    When I request sites with filter '{"siteName": "Alpha Site"}'
    Then the response should contain 1 sites
    And the first site should have name "Alpha Site"

Scenario: Filter by partial site name match (case insensitive)
    When I request sites with filter '{"siteName": "alpha"}'
    Then the response should contain 2 sites
    And the sites should contain names:
        | SiteName   |
        | Alpha Site |
        | Alpha Beta |

Scenario: Filter by partial site name match (different case)
    When I request sites with filter '{"siteName": "BETA"}'
    Then the response should contain 2 sites
    And the sites should contain names:
        | SiteName     |
        | Beta Testing |
        | Alpha Beta   |

Scenario: Filter with no matches
    When I request sites with filter '{"siteName": "NonExistent"}'
    Then the response should contain 0 sites
    And the response should have totalCount of 0

Scenario: Filter combined with pagination
    When I request sites with filter '{"siteName": "Site"}' and page 1 and page size 1
    Then the response should contain 1 sites
    And the response should have totalCount of 2
    And the response should have pageNumber of 1
    And the response should have pageSize of 1
    And the response should have totalPages of 2
    And the response should have hasNext as true

Scenario: Filter with empty string returns all sites
    When I request sites with filter '{"siteName": ""}'
    Then the response should contain 8 sites
    And the response should have totalCount of 8

Scenario: Invalid JSON filter returns bad request
    When I request sites with invalid filter 'invalid-json'
    Then the response should return status code 400
    And the response should contain error message about invalid JSON

Scenario: POST search endpoint with filter
    When I POST to search endpoint with:
        """
        {
            "page": 1,
            "size": 10,
            "filter": {
                "siteName": "test"
            }
        }
        """
    Then the response should contain 2 sites
    And the sites should contain names:
        | SiteName         |
        | Beta Testing     |
        | Test Environment |

Scenario: POST search endpoint with pagination
    When I POST to search endpoint with:
        """
        {
            "page": 2,
            "size": 3,
            "filter": {}
        }
        """
    Then the response should contain 3 sites
    And the response should have pageNumber of 2
    And the response should have pageSize of 3
