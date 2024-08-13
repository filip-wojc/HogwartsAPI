using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Route("api/course")]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly IGetEntitiesService<CourseDto> _getService;
        private readonly IAddEntitiesService<CreateCourseDto> _addService;
        private readonly IModifyEntitiesService<ModifyCourseDto> _modifyService;
        private readonly IDeleteEntitiesService<Course> _deleteService;
        public CourseController(IGetEntitiesService<CourseDto> getService, IAddEntitiesService<CreateCourseDto> addService, IModifyEntitiesService<ModifyCourseDto> modifyService,IDeleteEntitiesService<Course> deleteService)
        {
            _getService = getService;
            _addService = addService;
            _modifyService = modifyService;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAll()
        {
            var courses = await _getService.GetAll();
            return Ok(courses);
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<CourseDto>> Get([FromRoute] int courseId)
        {
            var course = await _getService.GetById(courseId);
            return Ok(course);
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCourseDto dto)
        {
            int courseId = await _addService.Create(dto);
            return Created($"/api/course/{courseId}", null);
        }
        [Authorize(Roles = "CourseManager,Admin")]
        [HttpPut("{courseId}")]
        public async Task<ActionResult> Modify([FromRoute] int courseId, [FromBody] ModifyCourseDto dto)
        {
            await _modifyService.Modify(courseId, dto);
            return Ok();
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpDelete("{courseId}")]
        public async Task<ActionResult> Delete([FromRoute] int courseId)
        {
            await _deleteService.Delete(courseId);
            return NoContent();
        }

    }
}
