using SystemAggregator.Clients.ProviderTwo.Models;

namespace SystemAggregator.Clients.ProviderTwo
{
    /// <inheritdoc />
    internal class ProviderTwoClient : ApiClientBase, IProviderTwoClient
    {
        /// <summary>
        /// A client's name.
        /// </summary>
        public const string Name = "ProviderTwoApi";

        /// <summary>
        /// A constructor.
        /// </summary>
        public ProviderTwoClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ProviderTwoSearchResponse?> SearchAsync(ProviderTwoSearchRequest request)
        {
            return await Post<ProviderTwoSearchResponse>($"api/v1/search", request, null, null, camelCase: true);
        }

        public async Task PingAsync()
        {
            await GetEmpty($"api/v1/ping", null, null);
        }
    }
}
