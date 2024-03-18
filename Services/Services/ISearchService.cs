using System.Security.Claims;
using System.Text.Json;

using SystemAggregator.Clients.Models;

namespace SystemAggregator.Services
{
    public interface ISearchService
    {
        string SearchCode { get; }

        Task<BaseSearchResult> Search(JsonElement param);
    }
}
