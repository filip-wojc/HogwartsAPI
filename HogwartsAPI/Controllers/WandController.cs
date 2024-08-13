using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [Route("/api/wand")]
    [ApiController]
    [Authorize]
    public class WandController : ControllerBase
    {
        private readonly IGetEntitiesService<WandDto> _getService;
        private readonly IAddEntitiesService<CreateWandDto> _addService;
        private readonly IDeleteEntitiesService<Wand> _deleteService;
        private readonly IModifyEntitiesService<ModifyWandDto> _modifyService;
        public WandController(IGetEntitiesService<WandDto> getService, IAddEntitiesService<CreateWandDto> addService, IDeleteEntitiesService<Wand> deleteService, IModifyEntitiesService<ModifyWandDto> modifyService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
            _modifyService = modifyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WandDto>>> GetAll()
        {
            var wands = await _getService.GetAll();
            return Ok(wands);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<WandDto>> Get([FromRoute] int id)
        {
            var wand = await _getService.GetById(id);
            return Ok(wand);
        }
        [HttpPost]
        [Authorize(Roles = "WandsManager,Admin")]
        public async Task<ActionResult> Create([FromBody] CreateWandDto dto)
        {
            int wandId = await _addService.Create(dto);
            return Created($"/api/wand/{wandId}", null);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "WandsManager,Admin")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _deleteService.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "WandsManager,Admin")]
        public async Task<ActionResult> Modify([FromRoute] int id, [FromBody] ModifyWandDto dto)
        { 
            await _modifyService.Modify(id, dto);
            return Ok();
        }
    }
}
