using ApiApp.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiApp.Providers.RequestProvider
{
    public interface IRequestProvider
    {
        Task<string> GetJsonString(string requestUrl, Dictionary<string, string> parameters = null);
        Task<T> GetJson<T>(string requestUrl, Dictionary<string, string> parameters = null) where T : JsonBase;
        Task<T> PostJson<T>(string requestUrl, string body = "") where T : JsonBase;
        Task PostJson(string requestUrl, string body = "");
        Task<T> PutJson<T>(string requestUrl, string body = "") where T : JsonBase;
        Task Delete(string requestUrl);
        void UpdateUser(ClaimsIdentity user);
    }
}
