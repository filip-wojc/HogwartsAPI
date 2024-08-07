using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [Route("/api/wand")]
    [ApiController]
    public class WandController : ControllerBase
    {
        private readonly IGetEntitiesService<WandDto> _getService;
        private readonly IAddEntitiesService<CreateWandDto> _addService;
        private readonly IDeleteEntitiesService _deleteService;
        public WandController(IGetEntitiesService<WandDto> getService, IAddEntitiesService<CreateWandDto> addService, IDeleteEntitiesService deleteService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WandDto>>> GetAll()
        {
            var wands = await _getService.GetAll();
            return Ok(wands);
        }
    }
}
