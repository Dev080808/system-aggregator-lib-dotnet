using System.Text.Json;

using SystemAggregator.Clients.Enums;
using SystemAggregator.Clients.Models;
using SystemAggregator.Core.Extensions;

namespace SystemAggregator.Services.SearchService
{
    public abstract class SearchService<TParam, TResult> : ISearchService
        where TParam : class
        where TResult : BaseSearchResult, new()
    {
        public abstract string SearchCode { get; }

        public async Task<BaseSearchResult> Search(JsonElement param)
        {
            if (!param.TryFromCamelCaseJson<TParam>(out var searchParam))
            {
                return new TResult
                {
                    Code = SearchResultCode.InvalidParam
                };
            }

            return await InvokeSearch(searchParam);
        }

        //public async Task<TResult> Search(TParam param)
        //{
        //    return await InvokeSearch(param);
        //}

        protected abstract Task<TResult> InvokeSearch(TParam param);
    }
}
