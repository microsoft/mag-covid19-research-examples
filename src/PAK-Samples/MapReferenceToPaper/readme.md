# Map Academic Reference Strings to Microsoft Academic paper entities

This folder contains sample code for generating academic reference strings from tabular input data and mapping the reference strings to [Microsoft Academic paper entities](https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-paper-entity-attributes).

## Prerequisites

* Free subscription for the [Project Academic Knowledge](https://msr-apis.portal.azure-api.net/products/project-academic-knowledge) Academic Search API
* Microsoft Visual Studio C#

## Building the project

1. Navigate to the MapReferenceToPaper directory
1. Open the MapReferenceToPaper.sln file
1. Once Visual Studio opens, build the project by going to Build->Build Solution

## Mapping sample WHO data

The project is pre-configured use the sample 2020-03-20-WorldHealthOrganization-COVID-19-Full-Database file, which is a snapshot of COVID-19 research taken from [WHO's website](https://www.who.int/emergencies/diseases/novel-coronavirus-2019/global-research-on-novel-coronavirus-2019-ncov).

To generate the mappings:
1. Right click on the "MapReferenceToPaper" project in the Solution Explorer and click "Properties"
1. Navigate to the "Debug" tab
1. In the "start options" text box, change "project_academic_knowledge_subscription_key" to the subscription key for Project Academic Knowledge found at https://msr-apis.portal.azure-api.net/developer.
1. Run the project by going to Debug->Start without debugging.

The program processes one row at a time from the sample data, echoing the mapping to both the console and a mapped.txt file in bin/Release.

The tool generates the following additional columns that precede the existing columns in the output rows:

Name | Description
--- | ---
MagMappingConfidence | A confidence value that represents the % of reference string that was able to be mapped to the top entity result
MagMappedLabels | A normalized version of the reference string that contains embedded XML tags that describe how different terms were mapped
MagId | The Microsoft Academic paper ID that was mapped
MagFamilyId | The Microsoft Academic paper family ID of the paper that was mapped if available
MagDoi | The Digital Object Identifier (DOI) of the paper if available
MagPubMedId | The PubMed identifier of the paper if available

To view an example see the [sample-output.txt](sampleData/sample-output.txt) file in the sampleData folder.

## Command line options

The tool has the following parameters when run from the command line:

Parameter | Description | Example
--- | --- | ---
Input | Input TSV file containing tab-seprated columns | input.txt
Output | Destination file to output mapping + original column data to | output.txt
Subscription key | The [Project Academic Knowledge subscription key](https://msr-apis.portal.azure-api.net/developer) to use | XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
Attributes to map | A comma-delimited list of [paper entity attributes](https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-paper-entity-attributes) to use when mapping the reference string | Ti,AA.AuN,C.CN,J.JN,Y,DOI,V,I,FP,LP
Additional attributes | A comma-delimted list of additonal [paper entity attributes](https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-paper-entity-attributes) to return for each mapped entity | Id,DOI,S,FamId
Columns to use | A comma-delimited list of zero-based column indexes to concatenate together from each input row for use as the reference string that is mapped | 0,1,3,5,6,7,8,10 

Example:
```
MapReferenceToPaper.exe ..\..\sampleData\2020-03-20-WorldHealthOrganization-COVID-19-Full-Database.txt mapped.txt project_academic_knowledge_subscription_key "Ti,AA.AuN,C.CN,J.JN,Y,DOI,V,I,FP,LP" "Id,DOI,S,FamId" "0,1,3,5,6,7,8,10"
```