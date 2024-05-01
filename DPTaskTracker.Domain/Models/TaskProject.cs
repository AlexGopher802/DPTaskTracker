using System.ComponentModel.DataAnnotations.Schema;

namespace DPTaskTracker.Domain.Models;

public class TaskProject
{
    public int Id { get; protected set; }

    public string Name { get; protected set; }

    public string? Description { get; protected set; }

    public string StatusCode { get; protected set; }

    public int ProjectId { get; protected set; }

    public int UserId { get; protected set; }

    public virtual Project Project { get; protected set; }

    public virtual User User { get; protected set; }

    public virtual ICollection<Message> Messages { get; protected set; }

    [NotMapped]
    public TaskStatus Status
    {
        get => Enum.Parse<TaskStatus>(StatusCode);
        protected set => StatusCode = value.ToString();
    }

    public TaskProject()
    {
    }

    public TaskProject(string name, string description, TaskStatus status, Project project, User user)
        : this()
    {
        Name = name;
        Description = description;
        Status = status;
        ProjectId = project.Id;
        Project = project;
        UserId = user.Id;
        User = user;
    }
    
    public void Update(string name, string description, TaskStatus status, User user)
    {
        Name = name;
        Description = description;
        Status = status;
        UserId = user.Id;
        User = user;
    }
}