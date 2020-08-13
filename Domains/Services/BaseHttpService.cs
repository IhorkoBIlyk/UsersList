using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UsersList.Common.Services
{
    public class BaseHttpService
    {
        protected HttpClient client;
       // private string baseApiUri;
        protected static string baseUsersListApiUri = "https://localhost:44387";

        protected BaseHttpService(string baseApiUri)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            client = new HttpClient(clientHandler);
            //this.baseApiUri = $"asdasdasdasdsadasd";
        }

        protected Task<T> GetAsync<T>(string requestUri)
        {
            return SendAsync<T>(HttpMethod.Get, requestUri);
        }
        protected Task<T> GetAsync<T>()
        {
            return SendAsync<T>(HttpMethod.Get, "");
        }

        protected Task<T> PostAsync<T>(string requestUri)
        {
            return SendAsync<T>(HttpMethod.Post, requestUri);
        }

        protected Task<T> PostAsync<T, K>(string requestUri, K obj)
        {
            string jsonRequest = obj != null ? JsonConvert.SerializeObject(obj) : null;
            return SendAsync<T>(HttpMethod.Post, requestUri, jsonRequest);
        }

        protected Task<T> PutAsync<T>(string requestUri)
        {
            return SendAsync<T>(HttpMethod.Put, requestUri);
        }

        protected Task<T> PutAsync<T, K>(string requestUri, K obj)
        {
            string jsonRequest = obj != null ? JsonConvert.SerializeObject(obj) : null;
            return SendAsync<T>(HttpMethod.Put, requestUri, jsonRequest);
        }

        protected Task<T> DeleteAsync<T>(string requestUri)
        {
            return SendAsync<T>(HttpMethod.Delete, requestUri);
        }


        protected async Task<T> SendAsync<T>(HttpMethod requestType, string requestUri, string jsonRequest = null)
        {
            T result = default;
            var uri = "https://10.0.2.2:44387/home";
            HttpRequestMessage request = new HttpRequestMessage(requestType, new Uri(uri));

            if (jsonRequest != null)
            {
                request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }

            string json = string.Empty;

            if (response != null)
            {
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            if (result is string)
            {
                result = (T)Convert.ChangeType(json, typeof(T));
            }
            else if (!string.IsNullOrEmpty(json))
            {
                result = JsonConvert.DeserializeObject<T>(json);
            }

            return result;
        }
        
    }
}
