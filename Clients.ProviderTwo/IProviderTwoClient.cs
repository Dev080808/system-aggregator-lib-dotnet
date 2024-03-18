using SystemAggregator.Clients.ProviderTwo.Models;

namespace SystemAggregator.Clients.ProviderTwo
{
    public interface IProviderTwoClient
    {
        Task<ProviderTwoSearchResponse?> SearchAsync(ProviderTwoSearchRequest request);

        Task PingAsync();
    }
}
