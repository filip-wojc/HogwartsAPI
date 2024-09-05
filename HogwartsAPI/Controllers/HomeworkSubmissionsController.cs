using HogwartsAPI.Dtos.HomeworkSubmissionsDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/homework-submissions")]
    public class HomeworkSubmissionsController : ControllerBase
    {
        private readonly IManyToManyRelationGetService<Student, HomeworkSubmissionDto> _getService;
        private readonly IAddEntitiesService<CreateHomeworkSubmissionDto> _addService;
        private readonly IManyToManyRelationDeleteService<Student, HomeworkSubmission> _deleteService;
        private readonly IModifyEntitiesService<ModifyHomeworkSubmissionDto> _modifyService;
        public HomeworkSubmissionsController(IManyToManyRelationGetService<Student, HomeworkSubmissionDto> getService, IAddEntitiesService<CreateHomeworkSubmissionDto> addService,
            IManyToManyRelationDeleteService<Student, HomeworkSubmission> deleteService, IModifyEntitiesService<ModifyHomeworkSubmissionDto> modifyService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
            _modifyService = modifyService;
        }
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<HomeworkSubmissionDto>>> GetAll([FromRoute] int studentId)
        {
            var homeworkSubmissions = await _getService.GetAllChildren(studentId);
            return Ok(homeworkSubmissions);
        }

        [HttpGet("student/{studentId}/submission/{homeworkSubmissionId}")]
        public async Task<ActionResult<HomeworkSubmissionDto>> Get([FromRoute] int studentId, [FromRoute] int homeworkSubmissionId)
        {
            var homeworkSubmission = await _getService.GetChildById(studentId, homeworkSubmissionId);
            return Ok(homeworkSubmission);
        }
        [Authorize(Roles = "CourseManager,Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateHomeworkSubmissionDto dto)
        {
            int createdSubmission = await _addService.Create(dto);
            return Created($"/api/homework-submissions/student/{dto.StudentId}/submission/{createdSubmission}", null);
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpPut("{homeworkSubmissionId}")]
        public async Task<ActionResult> Modify([FromRoute] int homeworkSubmissionId, [FromBody] ModifyHomeworkSubmissionDto dto)
        {
            await _modifyService.Modify(homeworkSubmissionId, dto);
            return Ok();
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpDelete("student/{studentId}")]
        public async Task<ActionResult> DeleteAll([FromRoute] int studentId)
        {
            await _deleteService.DeleteAllChildren(studentId);
            return NoContent();
        }
        [HttpDelete("student/{studentId}/submission/{homeworkSubmissionId}")]
        public async Task<ActionResult> Delete([FromRoute] int studentId, [FromRoute] int homeworkSubmissionId)
        {
            await _deleteService.DeleteChild(studentId, homeworkSubmissionId);
            return NoContent();
        }
    }
}
