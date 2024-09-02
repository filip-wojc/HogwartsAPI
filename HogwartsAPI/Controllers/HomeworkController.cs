using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/course/")]
    public class HomeworkController : ControllerBase
    {
        private readonly IManyToManyRelationGetService<Course, HomeworkDto> _getService;
        private readonly IAddEntitiesService<CreateHomeworkDto> _addService;
        private readonly IManyToManyRelationDeleteService<Course, Homework> _deleteService;
        private readonly IModifyEntitiesService<ModifyHomeworkDto> _modifyService;
        public HomeworkController(IManyToManyRelationGetService<Course, HomeworkDto> getService, IAddEntitiesService<CreateHomeworkDto> addService,
                            IManyToManyRelationDeleteService<Course, Homework> deleteService, IModifyEntitiesService<ModifyHomeworkDto> modifyService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
            _modifyService = modifyService;
        }
        [HttpGet("{courseId}/homeworks")]
        public async Task<ActionResult<IEnumerable<HomeworkDto>>> GetAll([FromRoute] int courseId)
        {
            var homeworks = await _getService.GetAllChildren(courseId);
            return Ok(homeworks);
        }

        [HttpGet("{courseId}/homeworks/{homeworkId}")]
        public async Task<ActionResult<IEnumerable<HomeworkDto>>> Get([FromRoute] int courseId, [FromRoute] int homeworkId)
        {
            var homework = await _getService.GetChildById(courseId, homeworkId);
            return Ok(homework);
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpPost("homeworks")]
        public async Task<ActionResult> Create([FromBody] CreateHomeworkDto dto)
        {
            int homeworkId = await _addService.Create(dto);
            return Created($"/api/course/{dto.CourseId}/homeworks/{homeworkId}", null);
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpPut("homeworks/{homeworkId}")]
        public async Task<ActionResult> Modify([FromRoute] int homeworkId, [FromBody] ModifyHomeworkDto dto)
        {
            await _modifyService.Modify(homeworkId, dto);
            return Ok();
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpDelete("{courseId}/homeworks/{homeworkId}")]
        public async Task<ActionResult> DeleteChild([FromRoute] int courseId, [FromRoute] int homeworkId)
        {
            await _deleteService.DeleteChild(courseId, homeworkId);
            return NoContent();
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpDelete("{courseId}/homeworks")]
        public async Task<ActionResult> DeleteAll([FromRoute] int courseId)
        {
            await _deleteService.DeleteAllChildren(courseId);
            return NoContent();
        }
    }
}
