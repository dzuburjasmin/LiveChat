using LiveChat;
using LiveChat.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/Test")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ChatService _chatService;
    private readonly ApplicationDbContext _context;

    public TestController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, ChatService chatService, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _chatService = chatService;
        _context = context;
    }

    [HttpGet("testNoDb")]
    public async Task<IActionResult> TestNoDb()
    {
        try
        {
            return Ok("Test with no database works!");

        }catch(Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("testWithDb")]
    public async Task<IActionResult> TestWithDb()
    {
        try
        {
            var message = _context.Messages.FirstOrDefault();
            return Ok(message);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}

