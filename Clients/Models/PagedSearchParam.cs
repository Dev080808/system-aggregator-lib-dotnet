namespace SystemAggregator.Clients.Models
{
    public class PagedSearchParam<TFilter, TOrder>
        where TOrder : struct, Enum
    {
        public TFilter? Filter { get; set; }

        public TOrder? Order { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
