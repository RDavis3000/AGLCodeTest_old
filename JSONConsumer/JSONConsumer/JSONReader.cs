using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JSONConsumer
{
    public static class JsonReader
    {
        public static async Task<string> GetJson(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
            }

        }

        public static List<PetOwnerRecord> DeserializeJsonToPersonRecords(string jsonInput)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling=NullValueHandling.Ignore;

            var output = JsonConvert.DeserializeObject<List<PetOwnerRecord>>(jsonInput, settings);

            return output;
        }

    }
}
