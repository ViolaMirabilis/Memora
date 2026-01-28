using Memora.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Memora.Services;

class JwtTokenHandler : DelegatingHandler
{

    private readonly ITokenStore _tokenStore;

    public JwtTokenHandler(ITokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Adds the JWT token to the request headers
            if (!string.IsNullOrEmpty(_tokenStore.Token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _tokenStore.Token);
            }

            return base.SendAsync(request, cancellationToken).Result;
            
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        
    }
}
