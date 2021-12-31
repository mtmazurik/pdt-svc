
namespace pdt.svc.services
{
    public interface ISearchEngine
    {
        List<SearchResult> Search(string queryString, int searchResultsMax = 50);
    }
}