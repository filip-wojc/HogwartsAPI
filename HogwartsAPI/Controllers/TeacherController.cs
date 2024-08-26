using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IGetEntitiesService<TeacherDto> _getService;
        private readonly IAddEntitiesService<CreateTeacherDto> _addService;
        private readonly IModifyEntitiesService<ModifyTeacherDto> _modifyService;
        private readonly IDeleteEntitiesService<Teacher> _deleteService;
        private readonly IPaginationService<TeacherDto> _paginationService;
        public TeacherController(IGetEntitiesService<TeacherDto> getService, IAddEntitiesService<CreateTeacherDto> addService,
                               IModifyEntitiesService<ModifyTeacherDto> modifyService, IDeleteEntitiesService<Teacher> deleteService, IPaginationService<TeacherDto> paginationService)
        {
            _getService = getService;
            _addService = addService;
            _modifyService = modifyService;
            _deleteService = deleteService;
            _paginationService = paginationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAll([FromQuery] PaginateQuery query)
        {
            var teachers = await _getService.GetAll();
            var paginationResult = _paginationService.GetPaginatedResult(query, teachers);
            return Ok(paginationResult);
        }
        [HttpGet("{teacherId}")]
        public async Task<ActionResult<TeacherDto>> GetById([FromRoute] int teacherId)
        {
            var teacher = await _getService.GetById(teacherId);
            return Ok(teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateTeacherDto dto)
        {
            int teacherId = await _addService.Create(dto);
            return Created($"/api/teacher/{teacherId}", null);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{teacherId}")]
        public async Task<ActionResult> Modify([FromRoute] int teacherId, [FromBody] ModifyTeacherDto dto)
        {
            await _modifyService.Modify(teacherId, dto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{teacherId}")]
        public async Task<ActionResult> Delete([FromRoute] int teacherId)
        {
            await _deleteService.Delete(teacherId);
            return NoContent();
        }
    }
}
