# PAK CORD-19 closure API

* [COVID-19 Open Research Dataset (CORD-19)](https://pages.semanticscholar.org/coronavirus-research)

The Microsoft Academic Services team in collaboration with the CORD-19 group are generating a CORD-19 closure graph. This graph contains Microsoft Academic Graph (MAG) paper IDs derived by iteratively expanding the citations of CORD-19 mapped MAG paper IDs. Each iteration is called a "hop", with hop "0" reflecting the original CORD-19 mapped MAG papers.

The raw MAG paper ID to hop mappings are available for download on our [official releases](../data/releases.md) page.

In addition to the raw closure graph mappings, we also expose a version of the Project Academic Knowledge (PAK) API that only includes entities in the MAG closure graph.

This API is identical to the standard [PAK API](https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/introduction) with the addition of a new "Hops" attribute that enables [query expressions](https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-query-expression-syntax) to be created that only look at papers derived at specific levels of the closure graph.

To use this version of PAK, include an additional "model=cord19" query parameter in your API requests, i.e.:

https://api.labs.cognitive.microsoft.com/academic/v1.0/method?model=cord19

## Examples

For each of the following examples, replace "subscription_key" with your official [PAK subscription key](https://msr-apis.portal.azure-api.net/products/project-academic-knowledge).

### Count of papers at each "hop" level

https://api.labs.cognitive.microsoft.com/academic/v1.0/calchistogram?subscription-key=subscription_key&count=20&attributes=Hops&expr=Hops%3E=0&model=cord19

### Retrieve top 10 "machine learning" papers from original CORD-19 papers

https://api.labs.cognitive.microsoft.com/academic/v1.0/evaluate?subscription-key=subscription_key&count=10&attributes=Id,Ti,F.FN&expr=And(Composite(F.FN==%27machine%20learning%27),Hops=0)&model=cord19