using System;

namespace UpBlazor.Application.Features.Users;

public class RegisteredUserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string GivenName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public TokenType Token { get; set; }

    public enum TokenType
    {
        NotSet,
        Demo,
        Real
    }
}