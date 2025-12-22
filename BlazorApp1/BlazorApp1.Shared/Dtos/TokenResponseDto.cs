namespace BlazorApp1.Shared.Dtos;

public sealed class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }

    public UserDto? User { get; set; }

}