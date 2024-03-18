using SystemAggregator.Clients.Aggregator.Models;

namespace SystemAggregator.Clients.Aggregator
{
    public interface IAggregatorClient
    {
        Task<SearchResponse?> Search(SearchRequest request);
    }
}
