using Newtonsoft.Json;

namespace ApiApp.Models
{
    public abstract class JsonBase
    {
        public int Id { get; set; }
        public string ToJson(bool ignoreNulls = false)
        {
            return JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    NullValueHandling = ignoreNulls
                        ? NullValueHandling.Ignore
                        : NullValueHandling.Include
                });
        }
        [JsonIgnore]
        public virtual bool HasValue { get; }
    }
}
