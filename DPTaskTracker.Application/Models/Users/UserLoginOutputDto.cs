using System.Text.Json.Serialization;
using DPTaskTracker.Domain.Models;
using Microsoft.OpenApi.Extensions;

namespace DPTaskTracker.Application.Models.Users;

public class UserLoginOutputDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("roleCode")]
    public string RoleCode { get; set; }

    public UserRole Role
    {
        get => Enum.Parse<UserRole>(RoleCode);
        protected set => RoleCode = value.ToString();
    }

    public string RoleString => Role.GetDisplayName();

    public UserLoginOutputDto()
    {
    }

    public UserLoginOutputDto(User user)
        : this()
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Password = user.Password;
        RoleCode = user.RoleCode;
    }
}