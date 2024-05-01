using DPTaskTracker.Application.Models.Users;
using DPTaskTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DPTaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _context;

    public UserController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        return Ok("Test");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginInputDto inputDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                (x.Username == inputDto.Login || x.Email == inputDto.Login) && x.Password == inputDto.Password);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserLoginOutputDto(user));
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users.Select(x => new UserLoginOutputDto(x)).ToListAsync();
        return Ok(users);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterInputDto inputDto)
    {
        var user = new User(inputDto.Username, inputDto.Email, inputDto.Password, UserRole.User);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new UserLoginOutputDto(user));
    }
}