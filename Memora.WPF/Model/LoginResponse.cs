namespace Memora.Model;

/// <summary>
/// Api returns a LoginResponse, which holds the token.
/// Before, it returned a raw string (instead of LoginResponse object), which couldn't be handled.
/// </summary>
public class LoginResponse
{
    public string? Token { get; set; }
}
