using System.Net;

namespace SystemAggregator.Clients
{
    /// <summary>
    /// An API client's exception.
    /// </summary>
    public class ApiClientException : ApplicationException
    {
        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="responseStatus">Response status.</param>
        /// <param name="message">Exception's message.</param>
        /// <param name="responseBody">A body of a response.</param>
        public ApiClientException(
            HttpStatusCode responseStatus,
            string message,
            string? requestUri = null,
            string? responseBody = null,
            Uri? redirectLocation = null,
            bool isPermanentRedirect = false,
            bool isTemporaryRedirect = false)
            : base(message)
        {
            RequestUri = requestUri;
            ResponseStatus = responseStatus;
            ResponseBody = responseBody;
            RedirectLocation = redirectLocation;
            IsPermanentRedirect = isPermanentRedirect;
            IsTemporaryRedirect = isTemporaryRedirect;
        }

        /// <summary>
        /// A request's URI.
        /// </summary>
        public string? RequestUri { get; private set; }

        /// <summary>
        /// A response status.
        /// </summary>
        public HttpStatusCode ResponseStatus { get; private set; }

        /// <summary>
        /// A body of a response.
        /// </summary>
        public string? ResponseBody { get; private set; }

        /// <summary>
        /// An address of a redirection.
        /// </summary>
        public Uri? RedirectLocation { get; private set; }

        /// <summary>
        /// Did a permanent redirect happen.
        /// </summary>
        public bool IsPermanentRedirect { get; private set; }

        /// <summary>
        /// Did a temporary redirect happen.
        /// </summary>
        public bool IsTemporaryRedirect { get; private set; }
    }
}
