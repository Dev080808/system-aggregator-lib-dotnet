using SystemAggregator.Clients.Enums;

namespace SystemAggregator.Services.Mapping
{
    public static class SearchMappings
    {
        public static bool IsValid(this SearchResultCode code)
        {
            return
                code != SearchResultCode.InvalidFilter &&
                code != SearchResultCode.InvalidOrder &&
                code != SearchResultCode.InvalidPage &&
                code != SearchResultCode.InvalidParam;
        }
    }
}
