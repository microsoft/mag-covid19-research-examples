# Multi-Sense Related Entities for Fields of Study

This folder contains sample code for computing multi-sense related entities for fields of study. It includes PySpark and U-SQL versions.

## PySpark Notebook

### Prerequisites

Complete following steps in [Network Similarity Sample (PySpark)](https://docs.microsoft.com/academic-services/graph/network-similarity-databricks).
* [Prerequisites](https://docs.microsoft.com/academic-services/graph/network-similarity-databricks#prerequisites)
* [Gather the information that you need](https://docs.microsoft.com/academic-services/graph/network-similarity-databricks#gather-the-information-that-you-need)
* [Import PySparkMagClass.py shared notebook](https://docs.microsoft.com/academic-services/graph/network-similarity-databricks#import-pysparkmagclass-shared-notebook)
* [Import PySparkNetworkSimilarityClass shared notebook](https://docs.microsoft.com/academic-services/graph/network-similarity-databricks#import-pysparknetworksimilarityclass-shared-notebook)

### Run Sample notebook

In this section, you import PySpark notebook in Azure Databricks workspace and run the notebook.

1. Save [FosMultiSenseRelatedEntities.py](FosMultiSenseRelatedEntities.py) to local drive.

1. In Azure Databricks workspace portal, from the **Workspace** > **Users** > **Your folder** drop-down, select **Import**.

1. Drag and drop `FosMultiSenseRelatedEntities.py` to the **Import Notebook** dialog box.

1. Replace following placeholder values with the values that you collected while completing the prerequisites of this sample.

   |Value  |Description  |
   |---------|---------|
   |**`<AzureStorageAccount>`** | The name of your Azure Storage account. |
   |**`<AzureStorageAccessKey>`** | The access key of your Azure Storage account. |
   |**`<MagContainer>`** | The container name in Azure Storage account containing MAG dataset, usually in the form of **mag-yyyy-mm-dd**. |

1. Click **Run All** button.

## U-SQL Script

### Prerequisites

Complete following steps in [Network Similarity Sample (U-SQL)](https://docs.microsoft.com/academic-services/graph/network-similarity-analytics).
* [Prerequisites](https://docs.microsoft.com/academic-services/graph/network-similarity-analytics#prerequisites)
* [Gather the information that you need](https://docs.microsoft.com/academic-services/graph/network-similarity-analytics#gather-the-information-that-you-need)
* [Define functions to extract MAG data](https://docs.microsoft.com/academic-services/graph/network-similarity-analytics#define-functions-to-extract-mag-data)
* [Define network similarity functions](https://docs.microsoft.com/academic-services/graph/network-similarity-analytics#define-network-similarity-functions)

### Run Sample script

1. Download [FosMultiSenseRelatedEntities.usql](FosMultiSenseRelatedEntities.usql) to your local drive.

1. Go to the Azure Data Lake Analytics (ADLA) service that you created, and select **Overview > New job > Open file**. Select `FosMultiSenseRelatedEntities.usql` in your local drive.

1. Replace `<AzureStorageAccount>`, and `<MagContainer>` placeholder values with the values that you collected while completing the prerequisites of this sample.

   |Value  |Description  |
   |---------|---------|
   |**`<AzureStorageAccount>`** | The name of your Azure Storage (AS) account containing MAG dataset. |
   |**`<MagContainer>`** | The container name in Azure Storage (AS) account containing MAG dataset, usually in the form of **mag-yyyy-mm-dd**. |

1. Select **Submit**.
