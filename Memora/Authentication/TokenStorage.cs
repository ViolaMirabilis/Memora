using Memora.Interfaces;

namespace Memora.Authentication;

/// <summary>
/// Token Storage which is a singleton, register in app.xaml.cs.
/// It's responsible for setting (once) and retrieving the JWT token for authenticated API requests.
/// Once set, the MessagingHandler adds the token to each API request.
/// </summary>
public class TokenStorage : ITokenStorage
{
    public string? Token { get; private set; }
    public void SetToken(string token)
    {
        Token = token;
    }
}
