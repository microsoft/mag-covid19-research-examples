---
page_type: sample
languages:
- csharp
products:
- dotnet
description: "Add 150 character max description"
urlFragment: "update-this-to-unique-url-stub"
---

# Official Microsoft Sample

<!-- 
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

The code samples provided here provide WHO / PubMed ID -> MAG ID mapping data as well 
as code examples showing how to perform COVID-19 related analysis against the 
[MAG](https://www.microsoft.com/en-us/research/project/microsoft-academic-graph/) Dataset 
and [Project Academic Knowledge API](https://www.microsoft.com/en-us/research/project/academic-knowledge/) 
or [MAKES API](https://docs.microsoft.com/en-us/academic-services/knowledge-exploration-service/).


## Contents

| File/folder       | Description                                |
|-------------------|--------------------------------------------|
| `src`             | Sample source code.                        |
| `src\data`        | Data files used to link MAG and external data. |
| `src\MAG-Samples`  | Sample source code that can be run against the MAG dataset. |
| `src\MAKES-Samples` | Sample source code that can be run agains the MAKES API. |
| `.gitignore`      | Define what to ignore at commit time.      |
| `CHANGELOG.md`    | List of changes to the sample.             |
| `CONTRIBUTING.md` | Guidelines for contributing to the sample. |
| `README.md`       | This README file.                          |
| `LICENSE`         | The license for the sample.                |

## Prerequisites

Depending on the samples you are using, you will either need a subscription to the 
[MAG](https://www.microsoft.com/en-us/research/project/microsoft-academic-graph/) dataset or 
the [Project Academic Knowledge API](https://www.microsoft.com/en-us/research/project/academic-knowledge/) 
or [MAKES API](https://docs.microsoft.com/en-us/academic-services/knowledge-exploration-service/).


## Key concepts

[MAG](https://www.microsoft.com/en-us/research/project/microsoft-academic-graph/) - The Microsoft Academic Graph
 
[Project Academic Knowledge API](https://www.microsoft.com/en-us/research/project/academic-knowledge/) - Knowledge API (throttled)

[MAKES API](https://docs.microsoft.com/en-us/academic-services/knowledge-exploration-service/) - Knowledge API (self-hosted on Azure)

## DATA

- [WHO, PubMed Id to MAG ID mapping file](/data/COVID-19MappedToMAG.tsv)

## MAG Samples

- [Network Similarity package](/MAG-Samples/NetworkSimilaritySample/readme.md)
- [COVID-19 impact on Computer Science and related conferences](/MAG-Samples/impact-of-covid19-on-the-computer-science-research-community/readme.md) - [(Blog post)](https://www.microsoft.com/en-us/research/project/academic/articles/impact-of-covid-19-on-computer-science-research-community)


## MAKES Samples

- TBD

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
