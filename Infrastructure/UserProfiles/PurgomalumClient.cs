using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UserProfiles
{
    public class PurgomalumClient
    {
        private  readonly HttpClient _httpClient;
        public PurgomalumClient() : this(new HttpClient()) { }

        public PurgomalumClient(HttpClient httpClient) => _httpClient = httpClient;
        public async Task<bool> CheckTextForProfanity(string text)
        {
            var result = await _httpClient.GetAsync(
                QueryHelpers.AddQueryString(
                    "https//:www.purgomalum.com/service/containprofanity", "text", text));
            var value = await result.Content.ReadAsStringAsync();
            return bool.Parse(value);
        }
    }
}
