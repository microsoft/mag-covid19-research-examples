# Databricks notebook source
# MAGIC %md # MAG Subgraph Generation
# MAGIC 
# MAGIC To run this notebook:
# MAGIC   - [Create an Azure Databricks service](https://azure.microsoft.com/en-us/services/databricks/).
# MAGIC   - [Create a cluster for the Azure Databricks service](https://docs.azuredatabricks.net/user-guide/clusters/create.html).
# MAGIC   - [Import](https://docs.databricks.com/user-guide/notebooks/notebook-manage.html#import-a-notebook) samples/PySparkMagClass.py under Workspace **Shared** folder.
# MAGIC   - [Import](https://docs.databricks.com/user-guide/notebooks/notebook-manage.html#import-a-notebook) this notebook.
# MAGIC   - [Create new table](https://docs.databricks.com/data/tables.html) contains specified PaperIds.
# MAGIC   - Replace **`<AzureStorageAccount>`**. This is the Azure Storage account containing MAG dataset.
# MAGIC   - Replace **`<AzureStorageAccessKey>`**. This is the Access Key of the Azure Storage account.
# MAGIC   - Replace **`<MagContainer>`**. This is the container name in Azure Storage account containing MAG dataset, Usually in forms of mag-yyyy-mm-dd.
# MAGIC   - Replace **`<OutputContainer>`**. This is the container name in Azure Storage account where the output goes to.
# MAGIC   - Attach this notebook to the cluster and run.

# COMMAND ----------

# DBTITLE 1,1. Initialization
# MAGIC %run "/Shared/PySparkMagClass"

# COMMAND ----------

AzureStorageAccount = '<AzureStorageAccount>'     # Azure Storage (AS) account containing MAG dataset
AzureStorageAccessKey = '<AzureStorageAccessKey>' # Access Key of the Azure Storage (AS) account
MagContainer = '<MagContainer>'                   # The container name in Azure Storage (AS) account containing MAG dataset, Usually in forms of mag-yyyy-mm-dd
OutputContainer = '<OutputContainer>'             # The container name in Azure Storage (AS) account where the output goes to
SubgraphDescription = 'cord_uid_2020_06_14_mappedto_2020_06_05_mag_id'                     # The name which could describe the subgraph
SpecifiedPaperIdsTable = spark.table("cord_uid_2020_06_14_mappedto_2020_06_05_mag_id_csv") # The table which contains the filtered PaperIds

# COMMAND ----------

# create a MicrosoftAcademicGraph instance to access MAG dataset
MAG = MicrosoftAcademicGraph(container=MagContainer, account=AzureStorageAccount, key=AzureStorageAccessKey)

# Create a AzureStorageUtil instance to access other Azure Storage files
ASU = AzureStorageUtil(container=OutputContainer, account=AzureStorageAccount, key=AzureStorageAccessKey)

# COMMAND ----------

# load MAG data
magAffiliations = MAG.getDataframe('Affiliations')
magAuthors = MAG.getDataframe('Authors')
magConferenceInstances = MAG.getDataframe('ConferenceInstances')
magConferenceSeries = MAG.getDataframe('ConferenceSeries')
magFieldsOfStudy = MAG.getDataframe('FieldsOfStudy')
magFieldOfStudyChildren = MAG.getDataframe('FieldOfStudyChildren')
magRelatedFieldOfStudy = MAG.getDataframe('RelatedFieldOfStudy')
magJournals = MAG.getDataframe('Journals')
magPaperAuthorAffiliations = MAG.getDataframe('PaperAuthorAffiliations')
magPaperFieldsOfStudy = MAG.getDataframe('PaperFieldsOfStudy')
magPaperRecommendations = MAG.getDataframe('PaperRecommendations')
magPaperReferences = MAG.getDataframe('PaperReferences')
magPapers = MAG.getDataframe('Papers')
magPaperUrls = MAG.getDataframe('PaperUrls')

# COMMAND ----------

#Get filtered PaperIds
SpecifiedPaperIds = SpecifiedPaperIdsTable \
                    .select(SpecifiedPaperIds.mag_id).distinct()

# COMMAND ----------

#Papers
Papers = magPapers \
         .join(SpecifiedPaperIds, magPapers.PaperId == SpecifiedPaperIds.mag_id, 'inner') \
         .select(magPapers.columns)

# COMMAND ----------

#PaperAuthorAffiliations
PaperAuthorAffiliations = magPaperAuthorAffiliations \
                          .join(SpecifiedPaperIds, magPaperAuthorAffiliations.PaperId == SpecifiedPaperIds.mag_id, 'inner') \
                          .select(magPaperAuthorAffiliations.columns)

# COMMAND ----------

#PaperReferences
PaperReferences_step1 = magPaperReferences \
                        .join(SpecifiedPaperIds, magPaperReferences.PaperId == SpecifiedPaperIds.mag_id, 'inner') \
                        .select(magPaperReferences.columns)

PaperReferences = PaperReferences_step1 \
                  .join(SpecifiedPaperIds, PaperReferences_step1.PaperReferenceId == SpecifiedPaperIds.mag_id, 'inner') \
                  .select(PaperReferences_step1.columns)

# COMMAND ----------

#PaperFieldsOfStudy
PaperFieldsOfStudy = magPaperFieldsOfStudy \
                     .join(SpecifiedPaperIds, magPaperFieldsOfStudy.PaperId == SpecifiedPaperIds.mag_id, 'inner') \
                     .select(magPaperFieldsOfStudy.columns)

# COMMAND ----------

#PaperRecommendations
PaperRecommendations_step1 = magPaperRecommendations \
                             .join(SpecifiedPaperIds, magPaperRecommendations.PaperId == SpecifiedPaperIds.mag_id, 'inner') \
                             .select(magPaperRecommendations.columns)

PaperRecommendations = PaperRecommendations_step1 \
                       .join(SpecifiedPaperIds, PaperRecommendations_step1.RecommendedPaperId == SpecifiedPaperIds.mag_id, 'inner') \
                       .select(PaperRecommendations_step1.columns)

# COMMAND ----------

#PaperUrls
PaperUrls = magPaperUrls \
            .join(SpecifiedPaperIds, magPaperUrls.PaperId == SpecifiedPaperIds.mag_id, 'inner') \
            .select(magPaperUrls.columns)

# COMMAND ----------

#Authors
FilteredAuthorIds = PaperAuthorAffiliations \
                    .select(PaperAuthorAffiliations.AuthorId).distinct() \
                    .withColumnRenamed("AuthorId", "FilteredAuthorId")

Authors = magAuthors \
          .join(FilteredAuthorIds, magAuthors.AuthorId == FilteredAuthorIds.FilteredAuthorId, 'inner') \
          .select(magAuthors.columns)

# COMMAND ----------

#Affiliations
FilteredAffiliationsIds = PaperAuthorAffiliations \
                          .select(PaperAuthorAffiliations.AffiliationId).where(PaperAuthorAffiliations.AffiliationId.isNotNull()) \
                          .distinct().withColumnRenamed("AffiliationId", "FilteredAffiliationId")

Affiliations = magAffiliations \
               .join(FilteredAffiliationsIds, magAffiliations.AffiliationId == FilteredAffiliationsIds.FilteredAffiliationId, 'inner') \
               .select(magAffiliations.columns)

# COMMAND ----------

#ConferenceInstances
FilteredConferenceInstanceIds = Papers \
                                .select(Papers.ConferenceInstanceId).where(Papers.ConferenceInstanceId.isNotNull()) \
                                .distinct().withColumnRenamed("ConferenceInstanceId", "FilteredConferenceInstanceId")

ConferenceInstances = magConferenceInstances \
                      .join(FilteredConferenceInstanceIds, magConferenceInstances.ConferenceInstanceId == FilteredConferenceInstanceIds.FilteredConferenceInstanceId, 'inner') \
                      .select(magConferenceInstances.columns)

# COMMAND ----------

#ConferenceSeries
FilteredConferenceSeriesIds = Papers \
                              .select(Papers.ConferenceSeriesId).where(Papers.ConferenceSeriesId.isNotNull()) \
                              .distinct().withColumnRenamed("ConferenceSeriesId", "FilteredConferenceSeriesId")

ConferenceSeries = magConferenceSeries \
                   .join(FilteredConferenceSeriesIds, magConferenceSeries.ConferenceSeriesId == FilteredConferenceSeriesIds.FilteredConferenceSeriesId, 'inner') \
                   .select(magConferenceSeries.columns)

# COMMAND ----------

#Journals
FilteredJournalIds = Papers \
                     .select(Papers.JournalId).where(Papers.JournalId.isNotNull()) \
                     .distinct().withColumnRenamed("JournalId", "FilteredJournalId")

Journals = magJournals \
           .join(FilteredJournalIds, magJournals.JournalId == FilteredJournalIds.FilteredJournalId, 'inner') \
           .select(magJournals.columns)

# COMMAND ----------

#FieldsOfStudy
FilteredFieldOfStudyIds = PaperFieldsOfStudy \
                          .select(PaperFieldsOfStudy.FieldOfStudyId).distinct() \
                          .withColumnRenamed("FieldOfStudyId", "FilteredFieldOfStudyId")

FieldsOfStudy = magFieldsOfStudy \
                .join(FilteredFieldOfStudyIds, magFieldsOfStudy.FieldOfStudyId == FilteredFieldOfStudyIds.FilteredFieldOfStudyId, 'inner') \
                .select(magFieldsOfStudy.columns)

# COMMAND ----------

#FieldsOfStudyChildren
FieldOfStudyChildren_step1 = magFieldOfStudyChildren \
                             .join(FilteredFieldOfStudyIds, magFieldOfStudyChildren.FieldOfStudyId == FilteredFieldOfStudyIds.FilteredFieldOfStudyId, 'inner') \
                             .select(magFieldOfStudyChildren.columns)

FieldOfStudyChildren = FieldOfStudyChildren_step1 \
                       .join(FilteredFieldOfStudyIds, FieldOfStudyChildren_step1.ChildFieldOfStudyId == FilteredFieldOfStudyIds.FilteredFieldOfStudyId, 'inner') \
                       .select(FieldOfStudyChildren_step1.columns)

# COMMAND ----------

#RelatedFieldOfStudy
RelatedFieldOfStudy_step1 = magRelatedFieldOfStudy \
                            .join(FilteredFieldOfStudyIds, magRelatedFieldOfStudy.FieldOfStudyId1 == FilteredFieldOfStudyIds.FilteredFieldOfStudyId, 'inner') \
                            .select(magRelatedFieldOfStudy.columns)

RelatedFieldOfStudy = RelatedFieldOfStudy_step1 \
                      .join(FilteredFieldOfStudyIds, RelatedFieldOfStudy_step1.FieldOfStudyId2 == FilteredFieldOfStudyIds.FilteredFieldOfStudyId, 'inner') \
                      .select(RelatedFieldOfStudy_step1.columns)

# COMMAND ----------

#Save subgraph to Azure Blob Storage (Stage1)
ASU.save(Papers, 'MAG-subgraph/' + SubgraphDescription + '/Papers.csv')
ASU.save(PaperAuthorAffiliations, 'MAG-subgraph/' + SubgraphDescription + '/PaperAuthorAffiliations.csv')
ASU.save(PaperReferences, 'MAG-subgraph/' + SubgraphDescription + '/PaperReferences.csv')
ASU.save(PaperFieldsOfStudy, 'MAG-subgraph/' + SubgraphDescription + '/PaperFieldsOfStudy.csv')
ASU.save(PaperRecommendations, 'MAG-subgraph/' + SubgraphDescription + '/PaperRecommendations.csv')
ASU.save(PaperUrls, 'MAG-subgraph/' + SubgraphDescription + '/PaperUrls.csv')

# COMMAND ----------

#Save subgraph to Azure Blob Storage (Stage2)
ASU.save(Authors, 'MAG-subgraph/' + SubgraphDescription + '/Authors.csv')
ASU.save(Affiliations, 'MAG-subgraph/' + SubgraphDescription + '/Affiliations.csv')
ASU.save(ConferenceInstances, 'MAG-subgraph/' + SubgraphDescription + '/ConferenceInstances.csv')
ASU.save(ConferenceSeries, 'MAG-subgraph/' + SubgraphDescription + '/ConferenceSeries.csv')
ASU.save(Journals, 'MAG-subgraph/' + SubgraphDescription + '/Journals.csv')

# COMMAND ----------

#Save subgraph to Azure Blob Storage (Stage3)
ASU.save(FieldsOfStudy, 'MAG-subgraph/' + SubgraphDescription + '/FieldsOfStudy.csv')
ASU.save(FieldOfStudyChildren, 'MAG-subgraph/' + SubgraphDescription + '/FieldOfStudyChildren.csv')
ASU.save(RelatedFieldOfStudy, 'MAG-subgraph/' + SubgraphDescription + '/RelatedFieldOfStudy.csv')
