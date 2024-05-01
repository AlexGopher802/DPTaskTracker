namespace DPTaskTracker.Application.Models.Users;

public class UserRegisterInputDto
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public UserRegisterInputDto()
    {
    }

    public UserRegisterInputDto(string username, string email, string password)
        : this()
    {
        Username = username;
        Email = email;
        Password = password;
    }
}