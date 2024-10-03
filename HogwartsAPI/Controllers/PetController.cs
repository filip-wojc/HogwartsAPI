using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;
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
        private readonly IManyToManyRelationGetService<Student, PetDto> _getStudentPetsService;
        private readonly IPaginationService<PetDto> _paginationService;
        public PetController(IGetEntitiesService<PetDto> getService, IAddEntitiesService<CreatePetDto> addService,
            IManyToManyRelationGetService<Student, PetDto> getStudentPetsService, IDeleteEntitiesService<Pet> deleteService, IPaginationService<PetDto> paginationService)
        {
            _getService = getService;
            _addService = addService;
            _getStudentPetsService = getStudentPetsService;
            _deleteService = deleteService;
            _paginationService = paginationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAll([FromQuery] PaginateQuery query)
        {
            var pets = await _getService.GetAll();
            var paginatedResult = _paginationService.GetPaginatedResult(query, pets);
            return Ok(paginatedResult);
        }

        [AllowAnonymous]
        [HttpGet("{petId}")]
        public async Task<ActionResult<PetDto>> Get([FromRoute] int petId)
        {
            var pet = await _getService.GetById(petId);
            return Ok(pet);
        }

        [HttpGet("students/{studentId}")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetStudentPets([FromRoute] int studentId)
        {
            var pets = await _getStudentPetsService.GetAllChildren(studentId);
            return Ok(pets);
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
