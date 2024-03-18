using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using SystemAggregator.Clients.Cfg;

namespace SystemAggregator.Clients
{
    /// <summary>
    /// A class presents additional feartures for API clients.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Register a named HTTP client to a DI container.
        /// </summary>
        /// <typeparam name="TClient">An interface of a client.</typeparam>
        /// <typeparam name="TImplementation">An implementation of a client.</typeparam>
        /// <param name="builder">A host builder.</param>
        /// <param name="name">A name of a HTTP client in a DI container.</param>
        public static void AddApiClient<TClient, TImplementation>(
            this IHostBuilder builder,
            string name)
            where TClient : class
            where TImplementation : class, TClient
        {
            builder.ConfigureServices((context, services) =>
            {
                var section = context.Configuration.GetSection($"{ApiClientSettings.Section}:{name}");

                services.Configure<ApiClientSettings>(name, section);

                services
                    .AddHttpClient<TClient, TImplementation>(
                        name,
                        (services, httpClient) => ConfigureHttpClient(name, services, httpClient))
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        UseCookies = false,
                        AllowAutoRedirect = false,
                    });
            });
        }

        private static void ConfigureHttpClient(string name, IServiceProvider services, HttpClient httpClient)
        {
            using var scope = services.CreateScope();

            var options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ApiClientSettings>>();

            var settings = options.Get(name);

            if (settings.BaseAddress == null)
            {
                throw new ApplicationException($"API client's base address is null for \"{name}\"");
            }

            Uri baseAddress;

            try
            {
                baseAddress = new Uri(settings.BaseAddress);
            }
            catch (Exception)
            {
                throw new ApplicationException($"API client's base address is invalid for \"{name}\"");
            }

            httpClient.BaseAddress = baseAddress;

            if (settings.TimeoutSeconds.HasValue && settings.TimeoutSeconds.Value > 0)
            {
                httpClient.Timeout = TimeSpan.FromTicks(settings.TimeoutSeconds.Value * TimeSpan.TicksPerSecond);
            }
        }
    }
}
