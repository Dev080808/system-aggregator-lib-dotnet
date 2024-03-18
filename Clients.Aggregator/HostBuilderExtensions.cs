using Microsoft.Extensions.Hosting;

namespace SystemAggregator.Clients.Aggregator
{
    public static class HostBuilderExtensions
    {
        public static void AddAgregatorClient(this IHostBuilder builder)
        {
            builder.AddApiClient<IAggregatorClient, AggregatorClient>(AggregatorClient.Name);
        }
    }
}
