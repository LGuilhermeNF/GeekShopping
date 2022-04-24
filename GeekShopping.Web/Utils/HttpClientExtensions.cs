﻿using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.Web.Utils
{
    public static class HttpClientExtensions
    {
        public static MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("application/json");
        
        public static async Task<T> ReadContetAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($@"Something went wrong calling API: {response.ReasonPhrase}");
            }
            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            return JsonSerializer.Deserialize<T>(dataAsString, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        }

        public static Task<HttpResponseMessage> PostAsJason<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpClient.PostAsync(url, content);
        }
        
        public static Task<HttpResponseMessage> PutAsJason<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
         
            
            content.Headers.ContentType = contentType;
            return httpClient.PutAsync(url, content);
        }
    }
}