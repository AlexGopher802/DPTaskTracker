namespace DPTaskTracker.Application.Models.Projects;

public class ProjectCreateInputDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime Deadline { get; set; }

    public ProjectCreateInputDto()
    {
    }

    public ProjectCreateInputDto(string name, string description, DateTime deadline)
        : this()
    {
        Name = name;
        Description = description;
        Deadline = deadline;
    }
}