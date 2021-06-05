using ApiApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ApiApp.Providers.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        #region Fields
        private readonly Dictionary<string, string> _baseParameters = new Dictionary<string, string>();
        private readonly ILogger<RequestProvider> _logger;
        private readonly HttpClient _client;
        #endregion

        #region Constructor
        public RequestProvider(ILogger<RequestProvider> logger,
                               HttpClient client)
        {
            _logger = logger;
            _client = client;
        }
        #endregion

        #region IRequestProvider
        public async Task Delete(string requestUrl)
        {
            _logger.LogTrace($"GET request {requestUrl}");
            var response = await _client.DeleteAsync(ConstructUrl(requestUrl));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            _logger.LogTrace($"Delete complited{result}");
        }

        public async Task<string> GetJsonString(string requestUrl, Dictionary<string, string> parameters = null)
        {
            _logger.LogTrace($"GET request {requestUrl}");
            var response = await _client.GetAsync(ConstructUrl(requestUrl, parameters));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            _logger.LogTrace($"GET response {result}");
            return result;
        }

        public async Task<T> GetJson<T>(string requestUrl, Dictionary<string, string> parameters = null) where T : JsonBase
        {
            _logger.LogTrace($"GET request {requestUrl}");
            var response = await _client.GetAsync(ConstructUrl(requestUrl, parameters));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            _logger.LogTrace($"GET response {result}");
            return JsonConvert.DeserializeObject<T>(result);
        }


        public async Task<T> PostJson<T>(string requestUrl, string body = "") where T : JsonBase
        {
            _logger.LogTrace($"POST request {requestUrl}. Body: {body}");

            var parametersString = new StringContent(body, Encoding.UTF8, "application/json");
            var url = ConstructUrl(requestUrl, null);
            var response = await _client.PostAsync(url, parametersString);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            _logger.LogTrace($"POST response {requestUrl}. Body: {result}");
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task PostJson(string requestUrl, string body = "")
        {
            _logger.LogTrace($"POST request {requestUrl}. Body: {body}");
            var parametersString = new StringContent(body, Encoding.UTF8, "application/json");
            parametersString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync(ConstructUrl(requestUrl, null), parametersString);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PutJson<T>(string requestUrl, string body = "") where T : JsonBase
        {
            _logger.LogTrace($"PUT request {requestUrl}. Body: {body}");
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(ConstructUrl(requestUrl), content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public void UpdateUser(ClaimsIdentity user)
        {
            if (_baseParameters.ContainsKey("token"))
            {
                _baseParameters.Remove("token");
            }
            _client.DefaultRequestHeaders.Authorization = user != null ? new AuthenticationHeaderValue("Bearer", user.FindFirst("Token").Value) : null;
        }
        #endregion

        #region Private methods
        private string ConstructUrl(string requestUrl, Dictionary<string, string> parameters = null)
        {
            parameters = parameters ?? new Dictionary<string, string>();

            var url = "api/" + requestUrl + @"?" + GetString(parameters.Union(_baseParameters));
            return requestUrl;
        }

        private string GetString(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return String.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
        }
        #endregion
    }
}
