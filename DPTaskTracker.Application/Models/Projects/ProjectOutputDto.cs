using System.Text.Json.Serialization;
using DPTaskTracker.Domain.Models;

namespace DPTaskTracker.Application.Models.Projects;

public class ProjectOutputDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("deadline")]
    public DateTime Deadline { get; set; }

    public string DeadlineString => Deadline.ToString("dd.MM.yyyy");

    public ProjectOutputDto()
    {
    }

    public ProjectOutputDto(Project project)
        : this()
    {
        Id = project.Id;
        Name = project.Name;
        Description = project.Description;
        Deadline = project.Deadline;
    }
}