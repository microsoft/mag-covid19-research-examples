# CORD-19 MAG Sub-graph Analysis (Pandas)
Use PandasMagClass.py to read MAG streams for Pandas:
1.	Store PandasMagClass.py as same location with notebook, import class by: `from PandasMagClass import MicrosoftAcademicGraph`
2.	Set root folder: `root = './data/'`
3.	Create new instance with root: `mag = MicrosoftAcademicGraph(root)`
4.	Get Dataframe by:
`df_papers = mag.get_data_frame('Papers')`
`df_paper_author_affiliations = mag.get_data_frame('PaperAuthorAffiliations')`

## Resources
* [AffiliationRegions.txt](https://github.com/microsoft/mag-covid19-research-examples/blob/master/src/MAG-Samples/impact-of-covid19-on-the-computer-science-research-community/AffiliationRegions.txt)
