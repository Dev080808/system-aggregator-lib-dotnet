using System.Net;
using System.Net.Http.Headers;
using System.Text;

using SystemAggregator.Core.Extensions;

namespace SystemAggregator.Clients
{
    /// <summary>
    /// A base class for an API client.
    /// </summary>
    public abstract class ApiClientBase
    {
        private const string JsonMimeType = "application/json";

        private const string Bearer = "Bearer";

        private static readonly MediaTypeWithQualityHeaderValue AcceptHeaderJson = new(JsonMimeType);

        private readonly HttpClient _httpClient;

        protected ApiClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region GET

        protected async Task<TResult?> Get<TResult>(
            string uri,
            object? query,
            string? token,
            bool camelCase = true,
            bool nullFor404 = false,
            HttpCompletionOption? httpCompletionOption = null)
        {
            return await Invoke(
                HttpMethod.Get,
                uri,
                x => HandleJsonResponse<TResult>(x, camelCase, nullFor404),
                query: query,
                token: token,
                acceptHeader: AcceptHeaderJson,
                httpCompletionOption: httpCompletionOption);
        }

        protected async Task GetEmpty(
            string uri,
            object? query,
            string? token)
        {
            await Invoke(
                HttpMethod.Get,
                uri,
                HandleEmptyResponse,
                query: query,
                token: token);
        }

        #endregion

        #region POST

        protected async Task<TResult?> Post<TResult>(
            string uri,
            object param,
            string? token,
            object? query = null,
            string? tokenScheme = null,
            bool camelCase = true)
        {
            return await Invoke(
                HttpMethod.Post,
                uri,
                x => HandleJsonResponse<TResult>(x, camelCase, false),
                param: param,
                query: query,
                tokenScheme: tokenScheme,
                token: token,
                acceptHeader: AcceptHeaderJson);
        }

        #endregion

        #region Private

        private async Task HandleEmptyResponse(HttpResponseMessage response)
        {
            await ValidateResponseStatus(response, HttpStatusCode.NoContent, HttpStatusCode.OK);
        }

        private async Task<TResult?> HandleJsonResponse<TResult>(HttpResponseMessage response, bool camelCase, bool nullFor404)
        {
            if (!nullFor404)
            {
                await ValidateResponseStatus(response, HttpStatusCode.NoContent, HttpStatusCode.OK);
            }
            else
            {
                await ValidateResponseStatus(response, HttpStatusCode.NoContent, HttpStatusCode.OK, HttpStatusCode.NotFound);
            }

            if (response.StatusCode == HttpStatusCode.NoContent ||
                (nullFor404 && response.StatusCode == HttpStatusCode.NotFound))
            {
                return default(TResult?);
            }

            var resultJson = await response.Content.ReadAsStringAsync();

            var result = camelCase
                ? resultJson.FromCamelCaseJson<TResult>()
                : resultJson.FromJson<TResult>();

            return result;
        }

        private async Task ValidateResponseStatus(HttpResponseMessage response, params HttpStatusCode[] validStatusCodes)
        {
            if (validStatusCodes.Contains(response.StatusCode))
            {
                return;
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            var message = $"An invalid HTTP response status {response.StatusCode}. Expected: {string.Join(", ", validStatusCodes)}";

            throw new ApiClientException(
                response.StatusCode,
                message,
                requestUri: response.RequestMessage?.RequestUri?.AbsoluteUri,
                responseBody: responseBody,
                redirectLocation: response.Headers.Location,
                isPermanentRedirect: response.StatusCode == HttpStatusCode.MovedPermanently,
                isTemporaryRedirect: response.StatusCode == HttpStatusCode.TemporaryRedirect);
        }

        private async Task Invoke(
            HttpMethod method,
            string uri,
            Func<HttpResponseMessage, Task> handleResponse,
            object? query = null,
            MediaTypeWithQualityHeaderValue? acceptHeader = null,
            IDictionary<string, string>? headers = null,
            string? tokenScheme = null,
            string? token = null,
            object? param = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            await Invoke(
                method,
                uri,
                async response => { await handleResponse(response); return true; },
                query,
                acceptHeader,
                headers,
                tokenScheme,
                token,
                param,
                httpCompletionOption);
        }

        private async Task<TResult> Invoke<TResult>(
            HttpMethod method,
            string uri,
            Func<HttpResponseMessage, Task<TResult>> handleResponse,
            object? query = null,
            MediaTypeWithQualityHeaderValue? acceptHeader = null,
            IDictionary<string, string>? headers = null,
            string? tokenScheme = null,
            string? token = null,
            object? param = null,
            HttpCompletionOption? httpCompletionOption = null,
            MultipartFormDataContent? multipartFormDataContent = null)
        {
            using var request = CreateRequest(method, uri, query, acceptHeader, headers, tokenScheme, token, param, multipartFormDataContent);

            var responseTask = httpCompletionOption.HasValue
                ? _httpClient.SendAsync(request, httpCompletionOption.Value)
                : _httpClient.SendAsync(request);

            using var response = await responseTask;

            var result = await handleResponse(response);

            return result;
        }

        private HttpRequestMessage CreateRequest(
            HttpMethod method,
            string uri,
            object? query,
            MediaTypeWithQualityHeaderValue? acceptHeader,
            IDictionary<string, string>? headers,
            string? tokenScheme,
            string? token,
            object? param,
            MultipartFormDataContent? multipartFormDataContent)
        {
            var request = new HttpRequestMessage
            {
                Method = method
            };

            if (query != null)
            {
                uri += query.ToQueryString();
            }

            request.RequestUri = _httpClient.BaseAddress != null
                ? new Uri(_httpClient.BaseAddress, uri)
                : new Uri(uri);

            if (acceptHeader != null)
            {
                request.Headers.Accept.Add(acceptHeader);
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(tokenScheme ?? Bearer, token);
            }

            if ((headers?.Count ?? 0) > 0)
            {
                foreach (var header in headers!)
                {
                    if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Value))
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
            }

            if (param != null)
            {
                request.Content = new StringContent(param.ToCamelCaseJson(), Encoding.UTF8, JsonMimeType);
            }

            if (multipartFormDataContent != null)
            {
                request.Content = multipartFormDataContent;
            }

            return request;
        }

        #endregion
    }
}
