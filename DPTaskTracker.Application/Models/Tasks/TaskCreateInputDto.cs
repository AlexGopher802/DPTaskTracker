namespace DPTaskTracker.Application.Models.Tasks;

public class TaskCreateInputDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int UserId { get; set; }
    
    public string StatusCode { get; set; }

    public TaskCreateInputDto()
    {
    }

    public TaskCreateInputDto(string name, string description, int userId, string statusCode = "Open")
        : this()
    {
        Name = name;
        Description = description;
        UserId = userId;
        StatusCode = statusCode;
    }
}