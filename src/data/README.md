---
page_type: data
description: Official WHO and CORD-19 data releases linked to MAG
---

# Official WHO COVID-19 and CORD-19 data releases linked to MAG

These files contain metadata to map Microsoft Academic Graph (MAG) paper entities to paper references collected for two different efforts:

* World Health Organization: [Global research on coronavirus disease (COVID-19)](https://www.who.int/emergencies/diseases/novel-coronavirus-2019/global-research-on-novel-coronavirus-2019-ncov)
* [COVID-19 Open Research Dataset (CORD-19)](https://pages.semanticscholar.org/coronavirus-research)

## Contents

Posted | Type | File/folder | Description
--- | --- | --- | ---
04/24/2020 | CSV | [Latest-CORD-19-MappedTo-MAG-Backfill.csv](./Latest-CORD-19-MappedTo-MAG-Backfill.csv) | Fixed name version of most recent [CORD-19 dataset](https://pages.semanticscholar.org/coronavirus-research) with ~90% MAG ID coverage
04/24/2020 | CSV | [Latest-CORD-19-MappedTo-MAG.csv](./Latest-CORD-19-MappedTo-MAG.csv) | Fixed name version of most recent [CORD-19 dataset](https://pages.semanticscholar.org/coronavirus-research) with ~90% MAG ID coverage, plus additional MAG metadata
04/24/2020 | TSV | [Latest-CORD-19-MappedTo-MAG.tsv](./Latest-CORD-19-MappedTo-MAG.tsv) | Fixed name version of most recent [CORD-19 dataset](https://pages.semanticscholar.org/coronavirus-research) with ~90% MAG ID coverage, plus additional MAG metadata
04/24/2020 | CSV | [Latest-CORD-UID-MappedTo-MAG-ID.csv](./Latest-CORD-UID-MappedTo-MAG-ID.csv) | Fixed name version of most recent CORD UID mapping to MAG ID
04/24/2020 | CSV | [2020-04-24-CORD-19-MappedTo-2020-04-17-MAG-Backfill.csv](./2020-04-24-CORD-19-MappedTo-2020-04-17-MAG-Backfill.csv) | 2020-04-24 [CORD-19 dataset](https://pages.semanticscholar.org/coronavirus-research) with ~90% MAG ID coverage
04/24/2020 | CSV | [2020-04-24-CORD-19-MappedTo-2020-04-17-MAG.csv](./2020-04-24-CORD-19-MappedTo-2020-04-17-MAG.csv) | 2020-04-24 [CORD-19 dataset](https://pages.semanticscholar.org/coronavirus-research) with ~90% MAG ID coverage, plus additional MAG metadata
04/24/2020 | TSV | [2020-04-24-CORD-19-MappedTo-2020-04-17-MAG.tsv](./2020-04-24-CORD-19-MappedTo-2020-04-17-MAG.tsv) | 2020-04-24 [CORD-19 dataset](https://pages.semanticscholar.org/coronavirus-research) with ~90% MAG ID coverage, plus additional MAG metadata
04/24/2020 | CSV | [2020-04-24-CORD-UID-MappedTo-2020-04-17-MAG-ID.csv](./2020-04-24-CORD-UID-MappedTo-2020-04-17-MAG-ID.csv) | CORD UID from the 2020-04-24 [CORD-19 dataset](https://pages.semanticscholar.org/coronavirus-research) mapped to MAG ID
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

## Linked CORD-19 data schema

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
13 | Microsoft Academic Paper ID | CORD-19 | MAG paper entity ID supplied by CORD-19
14 | WHO #Covidence | CORD-19 | Unique paper ID associated with WHO COVID-19 data set
15 | has_full_text | CORD-19 | Indicates if paper has full text in CORD-19 data set
16 | full_text_file | CORD-19 | Indicates licensing terms of full text in CORD-19 data set
17 | url | CORD-19 | Paper full-text URL
18 | MagMappingScore | MAG | Score that reflects the accuracy of the mapping based on how much of the paper metadata could be linked to MAG
19 | MagPaperId | MAG | MAG paper entity ID supplied by Microsoft
20 | MagPaperFamilyId | MAG | MAG paper entity family ID
21 | MagMappedLabels | MAG | String used for mapping the paper metadata to MAG paper entity with embedded XML labels indicating what terms were mapped to what fields, and the confidence of that mapping

## Linked CORD UID to MAG ID schema
Column # | Name | Source | Description
--- | --- | --- | ---
1 | cord_uid | CORD-19 | Unique ID for CORD-19 data set
2 | Microsoft Academic Paper ID | MAG | MAG paper entity ID

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
