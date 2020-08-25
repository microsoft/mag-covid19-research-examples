## KDD 2020 hands-on tutorial
# In Search For A Cure: Recommendation with Knowledge Graph on CORD-19

This page contains the instruction for Module 1 and 2 in the [KDD 2020 hands-on tutorial: cord19recommender](https://kdd2020tutorial.github.io/cord19recommender/). 

## Module 1 - Basics of CORD-19 data set and knowledge graph
* [Get CORD-19 MAG Subgraph using PySpark](./Module1-Get-MAG-SubGraph/README.md)


## Module 2 - Understanding contents with the aid of knowledge graph

### MAG Analysis Examples (Pandas)
Use PandasMagClass.py to read MAG streams for Pandas:
1.	Store PandasMagClass.py as same location with notebook, import class by: `from PandasMagClass import MicrosoftAcademicGraph`
2.	Set root folder: `root = './data/'`
3.	Create new instance with root: `mag = MicrosoftAcademicGraph(root)`
4.	Get Dataframe by:
`df_papers = mag.get_data_frame('Papers')`
`df_paper_author_affiliations = mag.get_data_frame('PaperAuthorAffiliations')`

### Resources
* [AffiliationRegions.txt](https://github.com/microsoft/mag-covid19-research-examples/blob/master/src/MAG-Samples/impact-of-covid19-on-the-computer-science-research-community/AffiliationRegions.txt)
