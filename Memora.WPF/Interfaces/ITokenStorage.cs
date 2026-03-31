namespace Memora.Interfaces;

public interface ITokenStorage
{
    string? Token { get; }
    public void SetToken(string token);
}
