using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/pet")]
    public class PetController : ControllerBase
    {
        private readonly IGetEntitiesService<PetDto> _getService;
        private readonly IAddEntitiesService<CreatePetDto> _addService;
        private readonly IDeleteEntitiesService<Pet> _deleteService;
        public PetController(IGetEntitiesService<PetDto> getService, IAddEntitiesService<CreatePetDto> addService, IDeleteEntitiesService<Pet> deleteService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAll()
        {
            var pets = await _getService.GetAll();
            return Ok(pets);
        }

        [HttpGet("{petId}")]
        public async Task<ActionResult<PetDto>> Get([FromRoute] int petId)
        {
            var pet = await _getService.GetById(petId);
            return Ok(pet);
        }

        [Authorize(Roles = "PetManager,Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePetDto dto)
        {
            int petId = await _addService.Create(dto);
            return Created($"/api/pet/{petId}", null);
        }

        [Authorize(Roles = "PetManager,Admin")]
        [HttpDelete("{petId}")]
        public async Task<ActionResult> Delete([FromRoute] int petId)
        {
            await _deleteService.Delete(petId);
            return NoContent();
        }
    }
}
