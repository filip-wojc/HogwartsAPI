using HogwartsAPI.Dtos;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Route("api/course")]
    public class CourseController : ControllerBase
    {
        private readonly IGetEntitiesService<CourseDto> _getService;
        public CourseController(IGetEntitiesService<CourseDto> getService)
        {
            _getService = getService;
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

        // TO DO
        // Pozostale metody kontrolera
    }
}
