using SystemAggregator.Clients.ProviderOne.Models;

namespace SystemAggregator.Clients.ProviderOne
{
    /// <inheritdoc />
    internal class ProviderOneClient : ApiClientBase, IProviderOneClient
    {
        /// <summary>
        /// A client's name.
        /// </summary>
        public const string Name = "ProviderOneApi";

        /// <summary>
        /// A constructor.
        /// </summary>
        public ProviderOneClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ProviderOneSearchResponse?> SearchAsync(ProviderOneSearchRequest request)
        {
            return await Post<ProviderOneSearchResponse>($"api/v1/search", request, null, null, camelCase: true);
        }

        public async Task PingAsync()
        {
            await GetEmpty($"api/v1/ping", null, null);
        }
    }
}
