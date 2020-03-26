# Map Academic Reference Strings to Microsoft Academic paper entities

This folder contains sample code for generating academic reference strings from tabular input data and mapping the reference strings to [Microsoft Academic paper entities](https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-paper-entity-attributes).

## Prerequisites

* One of the following:
  * Free subscription for the [Project Academic Knowledge (PAK)](https://msr-apis.portal.azure-api.net/products/project-academic-knowledge) Academic Search API 
  * [Microsoft Academic Knowledge Exploration Service (MAKES)](https://docs.microsoft.com/en-us/academic-services/knowledge-exploration-service/) API instance 
* C# compiler (Visual Studio or Visual Studio Code recommended)

## Building the project in Visual Studio

1. Navigate to the MapReferenceToPaper directory
1. Open the MapReferenceToPaper.sln file
1. Once Visual Studio opens, select Build->Build Solution from the top menu.

## Mapping sample WHO data

The project is pre-configured use the sample-input.txt file, which is a snapshot of COVID-19 research taken from [WHO's website](https://www.who.int/emergencies/diseases/novel-coronavirus-2019/global-research-on-novel-coronavirus-2019-ncov).

### To generate mappings using PAK

1. Open a Windows command prompt by opening the Start menu, typing "cmd" and clicking the "Command Prompt" application
1. Navigate to the directory where you downloaded/cloned the sample project
1. Modify the who-covid-19-pak-config.json file and replace "{subscription_key}" with your PAK subscription key (found at https://msr-apis.portal.azure-api.net/developer) in both the InterpretBaseUrl and EvaluateBaseUrl properties
1. Navigate to the bin/Release directory and execute the following command

```
MapReferenceToPaper.exe ..\..\sampleData\sample-input.txt mapped.txt ..\..\who-covid-19-pak-config.json
```

### To generate mappings using MAKES

1. Open a Windows command prompt by opening the Start menu, typing "cmd" and clicking the "Command Prompt" application
1. Navigate to the directory where you downloaded/cloned the sample project
1. Modify the who-covid-19-makes-config.json file and replace "{instance_name}" with your MAKES instance name in both the InterpretBaseUrl and EvaluateBaseUrl properties
1. Navigate to the bin/Release directory and execute the following command

```
MapReferenceToPaper.exe ..\..\sampleData\sample-input.txt mapped.txt ..\..\who-covid-19-makes-config.json
```

The program processes one row at a time from the sample data, echoing the mapping to both the console and the mapped.txt file in bin/Release. It adds the following columns:

Name | Description
--- | ---
MagMappingScore | A confidence value that represents the % of reference string that was able to be mapped to the top entity result
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
Configuration file | JSON file containing configuration options (see below) | config.json

## Configuration options

Option | Description
--- | ---  | ---
InterpretBaseUrl | The base URL to use for calling the Interpret method
EvaluateBaseUrl | The base URL to use for calling the Interpret method
InterpretTimeout | Maximum interpret API method call duration
ApiRequestConcurrency | Number of concurrent API requests to make when (i.e. number of rows to process in parallel)
InputColumnMapping | Defines the different types of academic data available in the input TSV file and their column index (zero-based). Available types: title, authors, year, venue, volume, issue, firstPage, lastPage, doi
OutputColumsn | The academic paper attributes to add to the output TSV. Available attributes (see https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-paper-entity-attributes): confidence, mapping, id, familyId, pubmedId, title, authors, year, venue, volume, issue, firstPage, lastPage, doi

For example configurations see [who-covid-19-makes-config.json](who-covid-19-makes-config.json) and [who-covid-19-pak-config.json](who-covid-19-pak-config.json).