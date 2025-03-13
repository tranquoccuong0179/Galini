using Galini.Models.Enum;

namespace Galini.Models.Payload.Request.User;

public class RegisterUserRequest
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public GenderEnum Gender { get; set; }

    public string? AvatarUrl { get; set; }

}