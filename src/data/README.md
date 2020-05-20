---
page_type: data
description: Official WHO and CORD-19 data releases linked to MAG
---

# Official WHO COVID-19 and CORD-19 data releases linked to MAG

This page documents the files containing metadata to map Microsoft Academic Graph (MAG) paper entities to paper references collected for two different efforts:

* World Health Organization: [Global research on coronavirus disease (COVID-19)](https://www.who.int/emergencies/diseases/novel-coronavirus-2019/global-research-on-novel-coronavirus-2019-ncov)
* [COVID-19 Open Research Dataset (CORD-19)](https://pages.semanticscholar.org/coronavirus-research)

## CORD-19 metadata with supplemented MAG ID mapping

This file mirrors the official CORD-19 metadata.csv schema and data, with the addition of supplemented MAG ID mapping in the "mag_id" column w/ approximately 90% coverage.

The most recent mapping file can be downloaded from:
[https://aka.ms/magcord19mappingbackfill](https://aka.ms/magcord19mappingbackfill)

The downloaded file name will contain both the official CORD-19 version (by date) and the official MAG version (by date) that were used to generate the mapping.

### Schema

Column # | Name | Source | Description
--- | --- | --- | ---
1 | cord_uid | CORD-19 | Unique ID for CORD-19 data set
2 | sha | CORD-19 | 40-character sha1 of the PDF
3 | source_x | CORD-19 | Paper source
4 | title | CORD-19 | Paper title
5 | doi | CORD-19 | Paper Digital Object Identifier (DOI)
6 | pmcid | CORD-19 | PubMed Central reference number (PMCID)
7 | pubmed_id | CORD-19 | PubMed reference number (PMID)
8 | license | CORD-19 | Fulltext licensing (see [CORD-19 site](https://pages.semanticscholar.org/coronavirus-research))
9 | abstract | CORD-19 | Paper abstract text
10 | publish_time | CORD-19 | Paper publication date
11 | authors | CORD-19 | List of paper author names
12 | journal | CORD-19 | Paper publication journal
13 | mag_id | MAG | MAG paper entity ID if available
14 | who_covidence_id | CORD-19 | Unique paper ID associated with WHO COVID-19 data set
15 | arxiv_id | CORD-19 | Arxiv ID
16 | pdf_json_files | CORD-19 | Location of metadata extracted from PDF in JSON format
17 | pmc_json_files | CORD-19 | Location of metadata extracted from PMC in JSON format
18 | url | CORD-19 | Paper full-text URL
19 | s2_id | AI2 | Semantic Scholar paper ID

## CORD-19 metadata with additional MAG ID mapping metadata

This file mirrors the official CORD-19 metadata.csv schema and data, with the addition of MAG mapping information in 4 additional columns (see below).

The most recent mapping file can be downloaded from:
[https://aka.ms/magcord19mappingextra](https://aka.ms/magcord19mappingextra)

The downloaded file name will contain both the official CORD-19 version (by date) and the official MAG version (by date) that were used to generate the mapping.

### Schema

Column # | Name | Source | Description
--- | --- | --- | ---
1 | cord_uid | CORD-19 | Unique ID for CORD-19 data set
2 | sha | CORD-19 | 40-character sha1 of the PDF
3 | source_x | CORD-19 | Paper source
4 | title | CORD-19 | Paper title
5 | doi | CORD-19 | Paper Digital Object Identifier (DOI)
6 | pmcid | CORD-19 | PubMed Central reference number (PMCID)
7 | pubmed_id | CORD-19 | PubMed reference number (PMID)
8 | license | CORD-19 | Fulltext licensing (see [CORD-19 site](https://pages.semanticscholar.org/coronavirus-research))
9 | abstract | CORD-19 | Paper abstract text
10 | publish_time | CORD-19 | Paper publication date
11 | authors | CORD-19 | List of paper author names
12 | journal | CORD-19 | Paper publication journal
13 | mag_id | CORD-19 | MAG paper entity ID supplied by CORD-19
14 | who_covidence_id | CORD-19 | Unique paper ID associated with WHO COVID-19 data set
15 | arxiv_id | CORD-19 | Arxiv ID
16 | pdf_json_files | CORD-19 | Location of metadata extracted from PDF in JSON format
17 | pmc_json_files | CORD-19 | Location of metadata extracted from PMC in JSON format
18 | url | CORD-19 | Paper full-text URL
19 | s2_id | AI2 | Semantic Scholar paper ID
20 | MagMappingScore | MAG | Score that reflects the accuracy of the mapping based on how much of the paper metadata could be linked to MAG
21 | MagPaperId | MAG | MAG paper entity ID supplied by Microsoft
22 | MagPaperFamilyId | MAG | MAG paper entity family ID
23 | MagMappedLabels | MAG | String used for mapping the paper metadata to MAG paper entity with embedded XML labels indicating what terms were mapped to what fields, and the confidence of that mapping

## CORD UID mapped to MAG ID

This file maps the CORD-19 UID (cord_uid) to the corresponding MAG ID.

The most recent mapping file can be downloaded from:
[https://aka.ms/magcord19mapping](https://aka.ms/magcord19mapping)

The downloaded file name will contain both the official CORD-19 version (by date) and the official MAG version (by date) that were used to generate the mapping.

### Schema
Column # | Name | Source | Description
--- | --- | --- | ---
1 | cord_uid | CORD-19 | Unique ID for CORD-19 data set
2 | mag_id | MAG | MAG paper entity ID

## WHO COVID-19 mapping

Posted | Type | File/folder | Description
--- | --- | --- | ---
03/26/2020 | CSV | [2020-03-23-WHO-MappedTo-2020-03-13-MicrosoftAcademicGraph.csv](./2020-03-23-WHO-MappedTo-2020-03-13-MicrosoftAcademicGraph.csv) | 2020-03-23 [WHO COVID-19 database](https://www.who.int/emergencies/diseases/novel-coronavirus-2019/global-research-on-novel-coronavirus-2019-ncov) linked to 2020-03-13 MAG

## Linked WHO COVID-19 data schema

Column # | Name | Source | Description
--- | --- | --- | ---
1 | Title | WHO | Paper title
2 | Authors | WHO | List of paper author names
3 | Abstract | WHO | Paper abstract text
4 | Published Year | WHO | Paper publication year
5 | Published Month | WHO | Paper publication month
6 | Journal | WHO | Paper publication journal
7 | Volume | WHO | Paper publication journal volume
8 | Issue | WHO | Paper publication journal issue
9 | Pages | WHO | Paper publication journal pages
10 | Accession Number | WHO | Accession number
11 | DOI | WHO | Paper Digital Object Identifier (DOI)
12 | Ref | WHO | Reference number
13 | Covidence # | WHO | Unique paper ID associated with WHO COVID-19 data set
14 | Study | WHO | Study name
15 | Notes | WHO | WHO specific notes for the paper
16 | Tags | WHO | WHO specific tags for the paper
17 | MagScore | MAG | Score that reflects the accuracy of the mapping based on how much of the paper metadata could be linked to MAG
18 | MagMappedLabels | MAG | String used for mapping the paper metadata to MAG paper entity with embedded XML labels indicating what terms were mapped to what fields, and the confidence of that mapping
19 | MagPaperId | MAG | MAG paper entity ID supplied by Microsoft
20 | MagDoi | MAG | MAG paper entity DOI
21 | MagPaperFamilyId | MAG | MAG paper entity family ID
22 | MagPubmedId | MAG | MAG paper entity pubmed ID

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
