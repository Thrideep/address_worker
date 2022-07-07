using Api.Abstractions.Contracts;
using Api.Abstractions.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Abstractions.Workers
{
    public abstract class AnalysisWorkerBase<T> where T : AnalysisResultBase, new()
    {
        protected string SourceUrl { get; }

        protected AnalysisWorkerBase(string url)
        {
            SourceUrl = url ?? throw new ArgumentNullException(nameof(url));
        }

        private readonly DataProviderService _restService = new(new System.Net.Http.HttpClient());

        public async Task<T> FetchDataAsync()
        {
            string apiResult = await _restService.GetResultAsync(SourceUrl);

            if (apiResult == Constants.SERVER_OFFLINE || apiResult.Contains(Constants.ERROR_PREFIX) || apiResult.Contains(Constants.RATE_LIMITED))
            {
                AnalysisResultBase result = new();
                result.FailureReasons.Add(apiResult);
                result.Message = null;
                result.IsSuccess = false;

                return result as T;
            }

            return JsonSerializer.Deserialize<T>(apiResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
