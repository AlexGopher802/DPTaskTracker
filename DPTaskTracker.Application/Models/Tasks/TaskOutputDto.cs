using System.Text.Json.Serialization;
using DPTaskTracker.Domain.Models;

namespace DPTaskTracker.Application.Models.Tasks;

public class TaskOutputDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("statusCode")]
    public string StatusCode { get; set; }

    public TaskOutputDto()
    {
    }

    public TaskOutputDto(TaskProject taskProject)
        : this()
    {
        Id = taskProject.Id;
        Name = taskProject.Name;
        Description = taskProject.Description;
        UserId = taskProject.UserId;
        Username = taskProject.User.Username;
        StatusCode = taskProject.StatusCode;
    }
}