using HogwartsAPI.Dtos.HouseDtos;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/house")]
    public class HouseController : ControllerBase
    {
        private readonly IGetEntitiesService<HouseDto> _getService;
        private readonly IModifyEntitiesService<ModifyHouseDto> _modifyService;
        private readonly IManyToManyRelationGetService<House, StudentDto> _getStudentsInHouseService;

        public HouseController(IGetEntitiesService<HouseDto> getService, IModifyEntitiesService<ModifyHouseDto> modifyService, IManyToManyRelationGetService<House, StudentDto> getStudentsInHouseService)
        {
            _getService = getService;
            _modifyService = modifyService;
            _getStudentsInHouseService = getStudentsInHouseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HouseDto>>> GetAll()
        {
            var houses = await _getService.GetAll();
            return Ok(houses);
        }
        [HttpGet("{houseId}")]
        public async Task<ActionResult<HouseDto>> Get([FromRoute] int houseId)
        {
            var house = await _getService.GetById(houseId);
            return Ok(house);
        }

        [Authorize(Roles = "HouseManager,Admin")]
        [HttpPut("{houseId}")]
        public async Task<ActionResult> Modify([FromRoute] int houseId, [FromBody] ModifyHouseDto dto)
        {
            await _modifyService.Modify(houseId, dto);
            return Ok();
        }

        [HttpGet("{houseId}/students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents([FromRoute] int houseId)
        {
            var students = await _getStudentsInHouseService.GetAllChildren(houseId);
            return Ok(students);
        }

        [HttpGet("{houseId}/students/{studentId}")]
        public async Task<ActionResult<StudentDto>> GetStudent([FromRoute] int houseId, [FromRoute] int studentId)
        {
            var student = await _getStudentsInHouseService.GetChildById(houseId, studentId);
            return Ok(student);
        }
    }
}
