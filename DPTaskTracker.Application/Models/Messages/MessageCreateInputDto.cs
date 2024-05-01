namespace DPTaskTracker.Application.Models.Messages;

public class MessageCreateInputDto
{
    public string Text { get; set; }

    public int UserId { get; set; }

    public int ProjectId { get; set; }
    
    public MessageCreateInputDto()
    {
    }
    
    public MessageCreateInputDto(string text, int userId, int projectId)
        : this()
    {
        Text = text;
        UserId = userId;
        ProjectId = projectId;
    }
}