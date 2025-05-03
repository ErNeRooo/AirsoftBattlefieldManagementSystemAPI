using System.Text;
using Newtonsoft.Json;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;

public static class HttpContentHelper
{
    public static HttpContent ToJsonHttpContent(this object model)
    {
        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        return httpContent;
    }
}