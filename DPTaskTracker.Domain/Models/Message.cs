namespace DPTaskTracker.Domain.Models;

public class Message
{
    public int Id { get; protected set; }

    public DateTime DateAdded { get; protected set; }

    public string Text { get; protected set; }

    public int ProjectId { get; protected set; }

    public int UserId { get; protected set; }

    public virtual Project Project { get; protected set; }

    public virtual User User { get; protected set; }

    public Message()
    {
    }

    public Message(string text, Project project, User user)
        : this()
    {
        DateAdded = DateTime.Now;
        Text = text;
        ProjectId = project.Id;
        Project = project;
        UserId = user.Id;
        User = user;
    }
}