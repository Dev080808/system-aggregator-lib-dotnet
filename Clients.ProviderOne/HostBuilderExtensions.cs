using Microsoft.Extensions.Hosting;

namespace SystemAggregator.Clients.ProviderOne
{
    public static class HostBuilderExtensions
    {
        public static void AddProviderOneClient(this IHostBuilder builder)
        {
            builder.AddApiClient<IProviderOneClient, ProviderOneClient>(ProviderOneClient.Name);
        }
    }
}
