using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace InfopasazerApp
{
    class BingApi
    {
        private const string BaseUrl =
            "https://api.cognitive.microsoft.com/bing/v5.0/images/search?q={0} dworzec&count=1&offset=0&mkt=en-us&safeSearch=Moderate";

        public static async Task<string> GetImageUrl(string query)
        {
            try
            {
                var request = WebRequest.Create(string.Format(BaseUrl, query)) as HttpWebRequest;
                if (request == null) return null;
                request.Headers["Ocp-Apim-Subscription-Key"] = "fdf2b09702fb422aac83646f737208ea";
                var responseAsync = request.GetResponseAsync();
                if (responseAsync == null) return null;
                using (var response = (await responseAsync) as HttpWebResponse)
                {
                    if (response != null && response.StatusCode != HttpStatusCode.OK)
                        throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                    var jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    var stream = response?.GetResponseStream();
                    if (stream == null) return null;
                    var objResponse = jsonSerializer.ReadObject(stream);
                    var jsonResponse = objResponse as Response;
                    return jsonResponse?.Value[0].ContentUrl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
