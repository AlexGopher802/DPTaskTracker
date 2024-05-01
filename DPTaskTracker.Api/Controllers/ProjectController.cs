using DPTaskTracker.Application.Models.Projects;
using DPTaskTracker.Application.Models.Tasks;
using DPTaskTracker.Application.Models.Users;
using DPTaskTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskStatus = DPTaskTracker.Domain.Models.TaskStatus;

namespace DPTaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly DatabaseContext _context;

    public ProjectController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("get/{projectId}")]
    public async Task<IActionResult> Get(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(new ProjectOutputDto(project));
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _context.Projects.Select(x => new ProjectOutputDto(x)).ToListAsync();
        return Ok(projects);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ProjectCreateInputDto inputDto)
    {
        var project = new Project(inputDto.Name, inputDto.Description, inputDto.Deadline);

        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return Ok(new ProjectOutputDto(project));
    }

    [HttpGet("{projectId}/get-tasks")]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var tasks = await _context.TaskProjects
            .Where(x => x.ProjectId == projectId)
            .Include(x => x.User)
            .Select(x => new TaskOutputDto(x))
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{projectId}/get-users")]
    public async Task<IActionResult> GetUsers(int projectId)
    {
        var users = await _context.Users
            .Where(x => x.Projects.Any(x => x.Id == projectId))
            .Include(x => x.Projects)
            .Select(x => new UserLoginOutputDto(x))
            .ToListAsync();

        return Ok(users);
    }

    [HttpPost("{projectId}/add-user/{userId}")]
    public async Task<IActionResult> AddUser(int projectId, int userId)
    {
        var project = await _context.Projects
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == projectId);
        var user = await _context.Users.FindAsync(userId);

        if (project == null || user == null)
        {
            return NotFound();
        }

        project.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("{projectId}/remove-user/{userId}")]
    public async Task<IActionResult> RemoveUser(int projectId, int userId)
    {
        var project = await _context.Projects
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == projectId);
        var user = await _context.Users.FindAsync(userId);

        if (project == null || user == null)
        {
            return NotFound();
        }

        project.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("{projectId}/add-task")]
    public async Task<IActionResult> AddTask(int projectId, TaskCreateInputDto inputDto)
    {
        var project = await _context.Projects.FindAsync(projectId);
        var user = await _context.Users.FindAsync(inputDto.UserId);

        if (project == null || user == null)
        {
            return NotFound();
        }

        var task = new TaskProject(inputDto.Name, inputDto.Description, TaskStatus.Open, project, user);

        await _context.TaskProjects.AddAsync(task);
        await _context.SaveChangesAsync();

        return Ok(new TaskOutputDto(task));
    }

    [HttpPut("{projectId}/update")]
    public async Task<IActionResult> Update(int projectId, ProjectCreateInputDto inputDto)
    {
        var project = await _context.Projects.FindAsync(projectId);

        if (project == null)
        {
            return NotFound();
        }

        project.Update(inputDto.Name, inputDto.Description, inputDto.Deadline);
        await _context.SaveChangesAsync();

        return Ok(new ProjectOutputDto(project));
    }
    
    [HttpPut("update-task/{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, TaskCreateInputDto inputDto)
    {
        var task = await _context.TaskProjects.FindAsync(taskId);
        var user = await _context.Users.FindAsync(inputDto.UserId);

        if (task == null || user == null)
        {
            return NotFound();
        }

        task.Update(inputDto.Name, inputDto.Description, Enum.Parse<TaskStatus>(inputDto.StatusCode), user);
        await _context.SaveChangesAsync();

        return Ok(new TaskOutputDto(task));
    }
}