namespace SystemAggregator.Clients.Cfg
{
    /// <summary>
    /// Settings for an API client.
    /// </summary>
    public class ApiClientSettings
    {
        public const string Section = "ApiClients";

        public string? BaseAddress { get; set; }

        public int? TimeoutSeconds { get; set; }
    }
}
