using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Shared.Dtos.User;

namespace WorkRecord.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetUserDto>>> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetUser(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
        {
            await _userService.AddUserAsync(dto, cancellationToken);
            return Ok();
        }

        //[HttpPut]
        //public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
        //{
        //    await _userService.UpdateUserAsync(dto, cancellationToken);
        //    return Ok();
        //}

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
        {
            await _userService.RegisterUserAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginUserDto dto, CancellationToken cancellationToken)
        {
            var token = await _userService.LoginUserAsync(dto, cancellationToken);
            return Ok(token);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(id, dto, true, cancellationToken);
            return Ok();
        }
    }
}
