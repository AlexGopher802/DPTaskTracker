namespace DPTaskTracker.Domain.Models;

public class Project
{
    public int Id { get; protected set; }

    public string Name { get; protected set; }

    public string? Description { get; protected set; }

    public DateTime Deadline { get; protected set; }

    public virtual ICollection<TaskProject> Tasks { get; protected set; }

    public virtual ICollection<User> Users { get; protected set; }

    public Project()
    {
    }

    public Project(string name, string description, DateTime deadline)
        : this()
    {
        Name = name;
        Description = description;
        Deadline = deadline;
    }

    public void Update(string name, string description, DateTime deadline)
    {
        Name = name;
        Description = description;
        Deadline = deadline;
    }

    public static Project GetTestInstance(string name, string description, DateTime deadline, int id)
        => new Project(name, description, deadline) { Id = id };
}