using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace WTracking.Services
{
    public class ApiFetcher : IApiFetcher
    {

        public async Task<string> FetchGoogleFitAPIAsync(string uri, string _tokenBearer, string method, string parameters = null)
        {
            if (method == "GET")
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", _tokenBearer));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.GetAsync(uri);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            else if (method == "POST")
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", _tokenBearer));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (parameters == null)
                        throw new NullReferenceException();
                    var queryString = new StringContent(parameters, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, queryString);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            else
                throw new Exception("Method not supported");
        }

        public Task FetchSamsungHealthAPI()
        {
            throw new NotImplementedException();
        }
    }

}


