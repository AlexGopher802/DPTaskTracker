using System.ComponentModel.DataAnnotations.Schema;

namespace DPTaskTracker.Domain.Models;

public class User
{
    public int Id { get; protected set; }

    public string Username { get; protected set; }

    public string Email { get; protected set; }

    public string Password { get; protected set; }

    public string RoleCode { get; protected set; }

    public virtual ICollection<TaskProject> Tasks { get; protected set; }

    public virtual ICollection<Message> Messages { get; protected set; }

    public virtual ICollection<Project> Projects { get; protected set; }

    [NotMapped]
    public UserRole Role
    {
        get => Enum.Parse<UserRole>(RoleCode);
        protected set => RoleCode = value.ToString();
    }

    public User()
    {
    }

    public User(string username, string email, string password, UserRole role)
        : this()
    {
        Username = username;
        Email = email;
        Password = password;
        Role = role;
    }

    public static User GetTestInstance(string username, string email, string password, UserRole role, int id)
        => new User(username, email, password, role) { Id = id };
}