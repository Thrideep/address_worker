using System;
using System.Threading.Tasks;

namespace Api.Abstractions.DataProviders
{
    public abstract class DataClientBase<T> : IDataClient where T : class, IDataProvider, new()
    {
        private T _provider;
        private T Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = (T)Activator.CreateInstance(typeof(T));
                }

                return _provider;
            }
        }

        public async Task<string> GetDataAsync(string address)
        {
            Uri uri = Provider.GetUrl(address);
            string result = await Provider.GetResultAsync(uri);
            return result;
        }

        public async Task<string> PostDataAsync<TData>(TData payload) where TData : class, new()
        {
            //if (payload == null)
            //{
            //    throw new ArgumentNullException(nameof(payload));
            //}

            Uri uri = Provider.GetUrl();
            string result = await Provider.PostDataAsync(uri, payload);

            return result;
        }
    }
}
