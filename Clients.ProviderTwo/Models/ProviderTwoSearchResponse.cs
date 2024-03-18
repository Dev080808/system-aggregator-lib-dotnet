using SystemAggregator.Clients.Models;

namespace SystemAggregator.Clients.ProviderTwo.Models
{
    public class ProviderTwoSearchResponse : BaseSearchResult
    {
        // Mandatory
        // Array of routes
        public ProviderTwoRoute[] Routes { get; set; }
    }
}
