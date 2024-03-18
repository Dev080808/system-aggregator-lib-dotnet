using SystemAggregator.Clients.Models;

namespace SystemAggregator.Clients.ProviderOne.Models
{
    public class ProviderOneSearchResponse : BaseSearchResult
    {
        // Mandatory
        // Array of routes
        public ProviderOneRoute[] Routes { get; set; }
    }
}
