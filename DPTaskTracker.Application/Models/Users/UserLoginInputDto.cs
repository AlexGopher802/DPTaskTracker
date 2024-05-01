namespace DPTaskTracker.Application.Models.Users;

public class UserLoginInputDto
{
    public string Login { get; set; }

    public string Password { get; set; }

    public UserLoginInputDto()
    {
    }

    public UserLoginInputDto(string login, string password)
    {
        Login = login;
        Password = password;
    }
}