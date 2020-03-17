# Databricks notebook source
# MAGIC %md # Network Similarity Sample
# MAGIC 
# MAGIC To run this notebook:
# MAGIC   - [Create an Azure Databricks service](https://azure.microsoft.com/services/databricks/).
# MAGIC   - [Create a cluster for the Azure Databricks service](https://docs.azuredatabricks.net/user-guide/clusters/create.html).
# MAGIC   - [Import](https://docs.databricks.com/user-guide/notebooks/notebook-manage.html#import-a-notebook) samples/PySparkMagClass.py under Workspace **Shared** folder.
# MAGIC   - [Import](https://docs.databricks.com/user-guide/notebooks/notebook-manage.html#import-a-notebook) ns/PySparkNetworkSimilarityClass.py under Workspace **Shared** folder.
# MAGIC   - [Import](https://docs.databricks.com/user-guide/notebooks/notebook-manage.html#import-a-notebook) this notebook.
# MAGIC   - Replace **`<AzureStorageAccount>`**. This is the Azure Storage account containing MAG dataset.
# MAGIC   - Replace **`<AzureStorageAccessKey>`**. This is the Access Key of the Azure Storage account.
# MAGIC   - Replace **`<MagContainer>`**. This is the container name in Azure Storage account containing MAG dataset, Usually in the form of mag-yyyy-mm-dd.
# MAGIC   - Attach this notebook to the cluster and run.

# COMMAND ----------

# MAGIC %run "/Shared/PySparkMagClass"

# COMMAND ----------

# MAGIC %run "/Shared/PySparkNetworkSimilarityClass"

# COMMAND ----------

# Replace following parameters
AzureStorageAccount = '<AzureStorageAccount>'     # Azure Storage (AS) account containing MAG dataset
AzureStorageAccessKey = '<AzureStorageAccessKey>' # Access Key of the Azure Storage (AS) account
MagContainer = '<MagContainer>'                   # The container name in Azure Storage (AS) account containing MAG dataset, Usually in the form of mag-yyyy-mm-dd

# COMMAND ----------

ResourcePathFosCopaper = 'ns/FosCopaper.txt'      # Network similarity resouce path for FosCopaper.
ResourcePathFosCovenue = 'ns/FosCovenue.txt'      # Network similarity resouce path for FosCovenue.
ResourcePathFosMetapath = 'ns/FosMetapath.txt'    # Network similarity resouce path for FosMetapath.
EntityId1 = 3008058167                            # Entity id of Coronavirus disease 2019 (COVID-19)
EntityId2 = 3007834351                            # Entity id of Severe acute respiratory syndrome coronavirus 2 (SARS-CoV-2)

# COMMAND ----------

# Create NetworkSimilarity instances to compute similarity
nsFosCopaper = NetworkSimilarity(container=MagContainer, account=AzureStorageAccount, key=AzureStorageAccessKey, resource=ResourcePathFosCopaper)
nsFosCovenue = NetworkSimilarity(container=MagContainer, account=AzureStorageAccount, key=AzureStorageAccessKey, resource=ResourcePathFosCovenue)
nsFosMetapath = NetworkSimilarity(container=MagContainer, account=AzureStorageAccount, key=AzureStorageAccessKey, resource=ResourcePathFosMetapath)

# COMMAND ----------

df = nsFosCopaper.getTopEntities(EntityId1, maxCount=10)
fosCopaperEntities1 = df.select(F.lit(EntityId1).alias('EntityId'), F.lit('Copaper').alias('SimilarityType'), df.EntityId.alias('SimilarEntityId'), df.Score)
#display(fosCopaperEntities1)

# COMMAND ----------

df = nsFosCovenue.getTopEntities(EntityId1, maxCount=10)
fosCovenueEntities1 = df.select(F.lit(EntityId1).alias('EntityId'), F.lit('Covenue').alias('SimilarityType'), df.EntityId.alias('SimilarEntityId'), df.Score)
#display(fosCovenueEntities1)

# COMMAND ----------

df = nsFosMetapath.getTopEntities(EntityId1, maxCount=10)
fosMetapathEntities1 = df.select(F.lit(EntityId1).alias('EntityId'), F.lit('Metapath').alias('SimilarityType'), df.EntityId.alias('SimilarEntityId'), df.Score)
#display(fosMetapathEntities1)

# COMMAND ----------

df = nsFosCopaper.getTopEntities(EntityId2, maxCount=10)
fosCopaperEntities2 = df.select(F.lit(EntityId2).alias('EntityId'), F.lit('Copaper').alias('SimilarityType'), df.EntityId.alias('SimilarEntityId'), df.Score)
#display(fosCopaperEntities2)

# COMMAND ----------

df = nsFosCovenue.getTopEntities(EntityId2, maxCount=10)
fosCovenueEntities2 = df.select(F.lit(EntityId2).alias('EntityId'), F.lit('Covenue').alias('SimilarityType'), df.EntityId.alias('SimilarEntityId'), df.Score)
#display(fosCovenueEntities2)

# COMMAND ----------

df = nsFosMetapath.getTopEntities(EntityId2, maxCount=10)
fosMetapathEntities2 = df.select(F.lit(EntityId2).alias('EntityId'), F.lit('Metapath').alias('SimilarityType'), df.EntityId.alias('SimilarEntityId'), df.Score)
#display(fosMetapathEntities2)

# COMMAND ----------

#union all results
fosRelatedEntities = \
  fosCopaperEntities1 \
  .union(fosCovenueEntities1) \
  .union(fosMetapathEntities1) \
  .union(fosCopaperEntities2) \
  .union(fosCovenueEntities2) \
  .union(fosMetapathEntities2)
#display(fosRelatedEntities)

# COMMAND ----------

# Create a MicrosoftAcademicGraph instance to access MAG dataset
mag = MicrosoftAcademicGraph(container=MagContainer, account=AzureStorageAccount, key=AzureStorageAccessKey)

# Get FieldsOfStudy dataframe
fos = mag.getDataframe('FieldsOfStudy')

# COMMAND ----------

# Join top entities with fos to show fos names
df = fosRelatedEntities \
    .join(fos, fosRelatedEntities.EntityId == fos.FieldOfStudyId, 'inner') \
    .select(fosRelatedEntities.EntityId, fos.DisplayName.alias('EntityDisplayName'), fosRelatedEntities.SimilarityType, fosRelatedEntities.Score, fosRelatedEntities.SimilarEntityId)

fosRelatedEntitiesDetail = df \
    .join(fos, df.SimilarEntityId == fos.FieldOfStudyId, 'inner') \
    .select(df.EntityId, df.EntityDisplayName, df.SimilarityType, df.Score, df.SimilarEntityId, fos.DisplayName.alias('SimilarEntityDisplayName')) \
    .sort('EntityId', 'SimilarityType', F.desc('Score'))

display(fosRelatedEntitiesDetail)
