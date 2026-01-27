namespace Memora.Interfaces;

public interface ITokenStore
{
    string? Token { get; }
    public void SetToken(string token);
}
