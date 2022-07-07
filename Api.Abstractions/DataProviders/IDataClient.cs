using System.Threading.Tasks;

namespace Api.Abstractions.DataProviders
{
    public interface IDataClient
    {
        Task<string> GetDataAsync(string address);
    }
}
