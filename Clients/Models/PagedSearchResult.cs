namespace SystemAggregator.Clients.Models
{
    public class PagedSearchResult<TItem> : BaseSearchResult
    {
        public List<TItem>? Items { get; set; }

        public int TotalCount { get; set; }
    }
}
