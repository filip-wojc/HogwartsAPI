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
        public AccountController(IAddEntitiesService<RegisterUserDto> registerService)
        {
            _registerService = registerService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
        {
            int userId = await _registerService.Create(dto);
            return Created($"/api/account/{userId}", null);
        }
    }
}
