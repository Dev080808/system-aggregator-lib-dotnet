namespace SystemAggregator.Site.Cfg
{
    public class SwaggerSettings
    {
        public const string Section = "Swagger";

        public string? DocName { get; set; }

        public string? DocTitle { get; set; }

        public string? DocDescription { get; set; }

        public string? XmlDocPath { get; set; } = "XmlDoc";

        public string? XmlDocFilePattern { get; set; } = "*.xml";
    }
}
