﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RAP.Services
{
    public class Http
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private async Task<T> MakeRequestAsync<T>(HttpRequestMessage request)
        {
            var result = await _httpClient.SendAsync(request);
            return await ParseResponse<T>(result);
        }


        public async Task<T> Get<T>(string requestUri, Dictionary<string, string> headers)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(HttpMethod.Get, requestUri);

            foreach (var header in headers)
            {
                httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await MakeRequestAsync<T>(httpRequestMessage);
        }

        public async Task<T> Post<T>(string requestUri, Dictionary<string, string> headers)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(HttpMethod.Post, requestUri);

            foreach (var header in headers)
            {
                httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await MakeRequestAsync<T>(httpRequestMessage);
        }

        private HttpRequestMessage BuildRequestMessage(HttpMethod method, string requestUri)
        {

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUri)
            };

            return httpRequestMessage;
        }

        private async Task<T> ParseResponse<T>(HttpResponseMessage result)
        {
            var resquestUri = result.RequestMessage.RequestUri.GetLeftPart(UriPartial.Path);
            var responseJson = string.Empty;
            try
            {
                responseJson = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseJson, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception parseException)
            {
                var ex = new Exception($"{resquestUri}\n{responseJson}", parseException);
                throw ex;
            }
        }
    }
}
