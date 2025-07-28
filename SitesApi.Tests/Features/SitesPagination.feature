Feature: Sites API Pagination
    As an API consumer
    I want to retrieve sites with pagination
    So that I can efficiently browse through large datasets

Background:
    Given the following sites exist in the system:
        | SiteId | SiteName        | SiteUrl                    | SiteX | SiteY |
        | 1      | Site Alpha      | https://alpha.example.com  | 10.5  | 20.3  |
        | 2      | Site Beta       | https://beta.example.com   | 15.2  | 25.7  |
        | 3      | Site Gamma      | https://gamma.example.com  | 12.8  | 18.9  |
        | 4      | Site Delta      | https://delta.example.com  | 22.1  | 30.4  |
        | 5      | Site Epsilon    | https://epsilon.example.com| 8.7   | 14.2  |
        | 6      | Site Zeta       | https://zeta.example.com   | 19.3  | 27.6  |
        | 7      | Site Eta        | https://eta.example.com    | 11.9  | 21.8  |
        | 8      | Site Theta      | https://theta.example.com  | 16.4  | 23.1  |
        | 9      | Site Iota       | https://iota.example.com   | 13.6  | 19.5  |
        | 10     | Site Kappa      | https://kappa.example.com  | 24.7  | 32.9  |

Scenario: Get first page with default page size
    When I request sites with no pagination parameters
    Then the response should contain 10 sites
    And the response should have totalCount of 10
    And the response should have pageNumber of 1
    And the response should have pageSize of 10
    And the response should have totalPages of 1
    And the response should have hasPrevious as false
    And the response should have hasNext as false

Scenario: Get first page with custom page size
    When I request sites with page size 3
    Then the response should contain 3 sites
    And the response should have totalCount of 10
    And the response should have pageNumber of 1
    And the response should have pageSize of 3
    And the response should have totalPages of 4
    And the response should have hasPrevious as false
    And the response should have hasNext as true

Scenario: Get second page with custom page size
    When I request sites with page 2 and page size 3
    Then the response should contain 3 sites
    And the response should have totalCount of 10
    And the response should have pageNumber of 2
    And the response should have pageSize of 3
    And the response should have totalPages of 4
    And the response should have hasPrevious as true
    And the response should have hasNext as true

Scenario: Get last page with remaining items
    When I request sites with page 4 and page size 3
    Then the response should contain 1 sites
    And the response should have totalCount of 10
    And the response should have pageNumber of 4
    And the response should have pageSize of 3
    And the response should have totalPages of 4
    And the response should have hasPrevious as true
    And the response should have hasNext as false

Scenario: Request page beyond available data
    When I request sites with page 10 and page size 5
    Then the response should contain 0 sites
    And the response should have totalCount of 10
    And the response should have pageNumber of 10
    And the response should have pageSize of 5
    And the response should have totalPages of 2
    And the response should have hasPrevious as true
    And the response should have hasNext as false

Scenario Outline: Parameter validation and correction
    When I request sites with page <inputPage> and page size <inputSize>
    Then the response should have pageNumber of <expectedPage>
    And the response should have pageSize of <expectedSize>

    Examples:
        | inputPage | inputSize | expectedPage | expectedSize |
        | 0         | 10        | 1            | 10           |
        | -1        | 10        | 1            | 10           |
        | 1         | 0         | 1            | 10           |
        | 1         | -5        | 1            | 10           |
        | 1         | 1500      | 1            | 1000         |

Scenario: Maximum page size enforcement
    When I request sites with page size 2000
    Then the response should have pageSize of 1000
