# Impact of COVID-19 on the Computer Science Research Community

Illustrates how the Microsoft Academic Graph is used to generate the data and graphs in the [Microsoft Academic Graph blog](https://www.microsoft.com/en-us/research/project/academic/articles/impact-of-covid-19-on-computer-science-research-community/). 

## Prerequisites
* [Set up provisioning of Microsoft Academic Graph to an Azure blob storage account](https://docs.microsoft.com/en-us/academic-services/graph/get-started-setup-provisioning?branch=index-build-commands-launch)
* [Set up Azure Data Lake Analytics for Microsoft Academic Graph](https://docs.microsoft.com/en-us/academic-services/graph/get-started-setup-azure-data-lake-analytics?branch=index-build-commands-launch)

## Gather the information that you need

Before you begin, you should have these items of information:

  :heavy_check_mark:  The name of your Azure Storage (AS) account containing MAG dataset from [Get Microsoft Academic Graph on Azure storage](get-started-setup-provisioning.md#note-azure-storage-account-name-and-primary-key).

   :heavy_check_mark:  The name of your Azure Data Lake Analytics (ADLA) service from [Set up Azure Data Lake Analytics](get-started-setup-azure-data-lake-analytics.md#create-azure-data-lake-analytics-account).

   :heavy_check_mark:  The name of your Azure Data Lake Storage (ADLS) from [Set up Azure Data Lake Analytics](get-started-setup-azure-data-lake-analytics.md#create-azure-data-lake-analytics-account).

   :heavy_check_mark:  The name of the container in your Azure Storage (AS) account containing MAG dataset.
   
## Upload the data that this analysis needed
1. We have picked the 105 most impactful CS conferences for this analysis. Upload the list of top CS conferences  [TopCSConferences.txt](TopCSConferences.txt) to a Azure Storage, it can be the same storage containing MAG dataset.
2. The authors' affiliation locations are used as the paper locations. For your convenience, we included the affiliation-region mapping in [AffilicationRegions.txt](AffilicationRegions.txt). This file has all the affiliations involved in this analysis. If you wish to get more affiliations location, MAG Affiliation.txt contains latitude and longitude for each affiliation. You can use [Bing Map API](https://docs.microsoft.com/en-us/bingmaps/rest-services/locations/find-a-location-by-point) to get the region/country from the coordinates.



[TopCSConferencesByRegion.usql](TopCSConferencesByRegion.usql)
[TopCSConf_CreateFunctions.usql](TopCSConf_CreateFunctions.usql)
