using System.Text.Json.Serialization;
using DPTaskTracker.Domain.Models;

namespace DPTaskTracker.Application.Models.Messages;

public class MessageOutputDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("dateAdded")]
    public DateTime DateAdded { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("projectId")]
    public int ProjectId { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    public string DateAddedString => DateAdded.ToString("dd.MM.yyyy HH:mm:ss");

    public MessageOutputDto()
    {
    }
    
    public MessageOutputDto(Message message)
        : this()
    {
        Id = message.Id;
        DateAdded = message.DateAdded;
        Text = message.Text;
        ProjectId = message.ProjectId;
        UserId = message.UserId;
        Username = message.User.Username;
    }
}