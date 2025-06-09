using System.Text;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;

public static class HttpContentHelper
{
    public static HttpContent ToJsonHttpContent(this object model)
    {
        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        return httpContent;
    }
    
    public static async Task<T?> DeserializeFromHttpContentAsync<T>(this HttpContent content) where T : class
    {
        string json = await content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(json))
            return null;

        // Parse to JObject
        JObject jObj;
        try
        {
            jObj = JObject.Parse(json);
        }
        catch (JsonReaderException)
        {
            return null; // Invalid JSON
        }
        
        // All good, deserialize
        return jObj.ToObject<T>();
    }
}