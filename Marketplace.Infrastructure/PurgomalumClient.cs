﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure
{
    /// <summary>
    /// PurgoMalum is a simple, free, RESTful web service for filtering
    // and removing content of profanity, obscenity and other unwanted
    // text.
    /// Check http://www.purgomalum.com
    /// </summary>

    public class PurgomalumClient
    {
        private readonly HttpClient _httpClient;
        public PurgomalumClient() : this(new HttpClient()) { }

        public PurgomalumClient(HttpClient httpClient) => _httpClient = httpClient;
        public async Task<bool> CheckTextForProfanity(string text)
        {
            var result= await _httpClient.GetAsync(
                QueryHelpers.AddQueryString(
                    "https//:www.purgomalum.com/service/containprofanity","text",text));
            var value=await result.Content.ReadAsStringAsync();
            return bool.Parse(value);
        }
    }
        
}
