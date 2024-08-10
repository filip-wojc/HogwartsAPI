using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAddEntitiesService<RegisterUserDto> _registerService;
        private readonly ILoginService _loginService;
        public AccountController(IAddEntitiesService<RegisterUserDto> registerService, ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
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

        //TO DO
        // 1) HttpPut - Admin moze zmieniac role innych uzytkownikow
    }
}
