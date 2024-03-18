using SystemAggregator.Clients.ProviderOne.Models;

namespace SystemAggregator.Clients.ProviderOne
{
    public interface IProviderOneClient
    {
        Task<ProviderOneSearchResponse?> SearchAsync(ProviderOneSearchRequest request);

        Task PingAsync();
    }
}
