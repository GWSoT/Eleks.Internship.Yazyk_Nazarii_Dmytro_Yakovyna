using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTracking.Services
{
    public interface IApiFetcher
    {
        Task<string> FetchGoogleFitAPIAsync(string uri, string _tokenBearer, string method, string parameters = null);
        Task FetchSamsungHealthAPI();
    }
}
