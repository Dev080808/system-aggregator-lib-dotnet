using Microsoft.Extensions.Hosting;

namespace SystemAggregator.Clients.ProviderTwo
{
    public static class HostBuilderExtensions
    {
        public static void AddProviderTwoClient(this IHostBuilder builder)
        {
            builder.AddApiClient<IProviderTwoClient, ProviderTwoClient>(ProviderTwoClient.Name);
        }
    }
}
