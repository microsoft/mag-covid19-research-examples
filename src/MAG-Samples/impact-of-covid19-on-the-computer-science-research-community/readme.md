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

1. Upload the list of 105 most impactful CS conferences to a Azure Storage, it can be the same storage containing MAG dataset.<br> Download  [TopCSConferences.txt](TopCSConferences.txt) to your local drive. <br> From [Azure portal](https://portal.azure.com), go to the Azure Storage account > **Containers > Upload > Select TopCSConferences.txt from your local drive > Upload**

 ![UploadFiles.png](UploadFiles.png "Upload files")
 
2. The authors' affiliation locations are used as the paper locations. For your convenience, we included the affiliation-region mapping in [AffiliationRegions.txt](AffiliationRegions.txt). This file has all the affiliations involved in this analysis. Upload this file to the same location as TopCSConferences.txt. <br>If you wish to get more affiliations location, MAG Affiliation.txt contains latitude and longitude for each affiliation. You can use [Bing Map API](https://docs.microsoft.com/en-us/bingmaps/rest-services/locations/find-a-location-by-point) to get the region/country from the coordinates.

## Define functions to extract MAG and conference data

In prerequisite [Set up Azure Data Lake Analytics](get-started-setup-azure-data-lake-analytics.md), you added the Azure Storage (AS) created for MAG provision as a data source for the Azure Data Lake Analytics service (ADLA). In this section, you submit an ADLA job to create functions extracting MAG and conference data from Azure Storage (AS).

1. Download `samples/CreateFunctions.usql` to your local drive. <br> From [Azure portal](https://portal.azure.com), go to the Azure Storage account > **Containers > [mag-yyyy-mm-dd] > samples > CreateFunctions.usql > Download**.

   ![Download CreateFunctions.usql](DownloadCreateFunctions.usql.png "Download CreateFunctions.usql")

2. Download [TopCSConf_CreateFunctions.usql](TopCSConf_CreateFunctions.usql) to your local drive.

3. Go to the Azure Data Lake Analytics (ADLA) service that you created, and select **Overview > New job > Open file**. Select `CreateFunctions.usql` in your local drive. <br> Select **Submit**.

   ![Submit CreateFunctions job](SubmitCreateFunctionsJob.png "Submit CreateFunctions job")

1. The job should finish successfully.

   ![CreateFunctions job status](https://docs.microsoft.com/en-us/academic-services/graph/media/samples-azure-data-lake-hindex/create-functions-status.png "CreateFunctions job status")
   
1. Repeat step 3-4 with TopCSConf_CreateFunctions.usql.

## Count publications by region

In this section, you submit an ADLA job to count publications for each conference and region.

1. Download [TopCSConferencesByRegion.usql](TopCSConferencesByRegion.usql) to local drive.

1. Replace placeholder values in the script using the table below.

   |Value  |Description  |
   |---------|---------|
   |**`<MagContainer>`** | The container name in Azure Storage (AS) account containing MAG dataset, usually in the form of **mag-yyyy-mm-dd**. |
   |**`<AzureStorageAccount>`** | The name of your Azure Storage (AS) account containing MAG dataset. |
   |**`<SourceFileContainer>`** | The container name in Azure Storage (AS) account containing TopCSConferences.txt and AffilicationRegions.txt dataset. |
   |**`<SourceFileStorageAccount>`** | The name of your Azure Storage (AS) account containing TopCSConferences.txt and AffilicationRegions.txt dataset. |
   
1. In the [Azure portal](https://portal.azure.com), go to the Azure Data Lake Analytics (ADLA) service that you created, and select **Overview > New Job > Open file**. <br> Select **TopCSConferencesByRegion.usql** from your local drive.

1. Change **AUs** to 10, and select **Submit**.
  
  ![Submit TopCS Job](SubmitTopCSJob.png "Submit TopCS Job")
  
1. The job should finish successfully in about 8 minutes.

  ![TopCS Job Status](JobStatus.png "TopCS Job Status")

## View output data

The output of the ADLA job in previous section goes to "/Output/TopCSConferencePaperRegions.tsv" in the Azure Data Lake Storage (ADLS). In this section, you use [Azure portal](https://portal.azure.com/) to view output content.

1. In the [Azure portal](https://portal.azure.com), go to the Azure Data Lake Storage (ADLS) service that you created, and select **Data Explorer > Output > TopCSConferencePaperRegions.tsv**.

   ![Data Explorer](TopCSConfernecePaperRegions.png "Data Explorer")
   
1. You see an output similar to the following snippet:
 ![TopCSConferencePaperRegions.tsv preview](OutputPreview.png "TopCSConferencePaperRegions.tsv preview")
 
1. TopCSConferencePaperRegions.tsv column definition as below.
   |Column #  |Description  |Example  |
   |---------|---------|---------|
   |0 | Conference Series Short Name | AAAI |
   |1 | Conference Series Long Name | National Conference on Artificial Intelligence |
   |2 | Conference Domain | Artificial Intelligence and Related |
   |3 | Conference Instance Name | AAAI 2000 |
   |4 | Conference Location | Austin, TX, USA |
   |5 | Conference Start Date | 2000-07-30T00:00:00.000 |
   |6 | Conference End Date | 2000-08-03T00:00:00.000 |
   |7 | Year | 2000 |
   |8 | First Author Affiliation Region | Canada |
   |9 | All Authors Count | 24 |
   |10 | First Author Count | 12 |
   
1. You can download this data, using Excel or any your preferred tool for further analysis.

## Next steps

If you're interested in Academic analytics and visualization, we have created U-SQL samples that use some of the same functions referenced in this tutorial.

> [!div class="nextstepaction"]
>[Analytics and visualization samples](https://docs.microsoft.com/en-us/academic-services/graph/samples-azure-data-lake-analytics)

## Resources

* [Get started with Azure Data Lake Analytics using Azure portal](https://docs.microsoft.com/azure/data-lake-analytics/data-lake-analytics-get-started-portal)
* [Data Lake Analytics](https://azure.microsoft.com/services/data-lake-analytics/)
* [U-SQL Language Reference](https://docs.microsoft.com/u-sql/)

