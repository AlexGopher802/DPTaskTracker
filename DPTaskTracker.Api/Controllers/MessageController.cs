using DPTaskTracker.Application.Models.Messages;
using DPTaskTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DPTaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly DatabaseContext _context;

    public MessageController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("{projectId}/get-all")]
    public async Task<IActionResult> GetAll(int projectId)
    {
        var messages = await _context.Messages
            .Where(x => x.ProjectId == projectId)
            .Include(x => x.User)
            .Select(x => new MessageOutputDto(x))
            .ToListAsync();

        return Ok(messages);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(MessageCreateInputDto inputDto)
    {
        var project = await _context.Projects.FindAsync(inputDto.ProjectId);
        var user = await _context.Users.FindAsync(inputDto.UserId);

        if (project == null || user == null)
        {
            return NotFound();
        }

        var message = new Message(inputDto.Text, project, user);

        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        return Ok(new MessageOutputDto(message));
    }
}