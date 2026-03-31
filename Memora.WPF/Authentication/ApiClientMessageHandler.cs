using Memora.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Media.Animation;
using System.Net.Http.Headers;

namespace Memora.Authentication
{
    public class ApiClientMessageHandler : DelegatingHandler
    {
        private readonly ITokenStorage _tokenStore;       // stores the JWT token here
        public ApiClientMessageHandler(ITokenStorage tokenStore)
        {
            _tokenStore = tokenStore;       // the token is stored only once, initialised via the constructor
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken ct)
        {
            var token = _tokenStore.Token;      // provides the token during the api send request

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, ct);
        }
    }
}
