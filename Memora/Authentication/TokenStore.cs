using Memora.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memora.Authentication;

public class TokenStore : ITokenStore
{
    public string? Token { get; private set; }
    public void SetToken(string token)
    {
        Token = token;
    }
}
