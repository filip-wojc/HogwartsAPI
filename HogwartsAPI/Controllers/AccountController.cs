using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAddEntitiesService<RegisterUserDto> _registerService;
        private readonly ILoginService _loginService;
        private readonly IModifyEntitiesService<ChangeUserRoleDto> _modifyRoleService;
        private readonly IModifyEntitiesService<ModifyUserDto> _modifyUserService;
        public AccountController(IAddEntitiesService<RegisterUserDto> registerService, ILoginService loginService, IModifyEntitiesService<ChangeUserRoleDto> modifyRoleService, IModifyEntitiesService<ModifyUserDto> modifyUserService)
        {
            _registerService = registerService;
            _loginService = loginService;
            _modifyRoleService = modifyRoleService;
            _modifyUserService = modifyUserService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
        {
            int userId = await _registerService.Create(dto);
            return Created($"/api/account/{userId}", null);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto dto)
        {
            string token = await _loginService.GenerateJwt(dto);
            return Ok(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("changeRole/{userId}")]
        public async Task<ActionResult> ChangeRole([FromRoute] int userId,[FromBody] ChangeUserRoleDto dto)
        {
            await _modifyRoleService.Modify(userId, dto);
            return Ok();
        }

        [Authorize]
        [HttpPut("modifyProfile/{userId}")]
        public async Task<ActionResult> Modify([FromRoute] int userId, [FromBody] ModifyUserDto dto)
        {
            await _modifyUserService.Modify(userId, dto);
            return Ok();
        }

    }
}
