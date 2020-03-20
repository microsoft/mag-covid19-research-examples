# Filtering Coronavirus Papers from MAG

This folder contains U-SQL sample for extracting coronavirus papers from MAG. It demonstrates filtering papers by matching title, abstracts, and fields of study.

# Prerequisites

Complete following steps in [Compute author h-index sample](https://docs.microsoft.com/academic-services/graph/tutorial-azure-data-lake-hindex).
* [Prerequisites](https://docs.microsoft.com/academic-services/graph/tutorial-azure-data-lake-hindex#prerequisites)
* [Gather the information that you need](https://docs.microsoft.com/academic-services/graph/tutorial-azure-data-lake-hindex#gather-the-information-that-you-need)
* [Define functions to extract MAG data](https://docs.microsoft.com/academic-services/graph/tutorial-azure-data-lake-hindex#define-functions-to-extract-mag-data)

# Run Sample script

1. Download [CoronavirusPapers.usql](CoronavirusPapers.usql) to your local drive.

1. Go to the Azure Data Lake Analytics (ADLA) service that you created, and select **Overview > New job > Open file**. Select `CoronavirusPapers.usql` in your local drive.

1. Replace `<AzureStorageAccount>`, and `<MagContainer>` placeholder values with the values that you collected while completing the prerequisites of this sample.

   |Value  |Description  |
   |---------|---------|
   |**`<AzureStorageAccount>`** | The name of your Azure Storage (AS) account containing MAG dataset. |
   |**`<MagContainer>`** | The container name in Azure Storage (AS) account containing MAG dataset, usually in the form of **mag-yyyy-mm-dd**. |

1. Select **Submit**.
