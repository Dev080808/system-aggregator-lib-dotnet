using SystemAggregator.Clients.Aggregator.Models;

namespace SystemAggregator.Clients.Aggregator
{
    /// <inheritdoc />
    internal class AggregatorClient : ApiClientBase, IAggregatorClient
    {
        /// <summary>
        /// A client's name.
        /// </summary>
        public const string Name = "AggregatorApi";

        /// <summary>
        /// A constructor.
        /// </summary>
        public AggregatorClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<SearchResponse?> Search(SearchRequest request)
        {
            return await Post<SearchResponse>($"api/v1/search", request, null, null, camelCase: true);
        }
    }
}
