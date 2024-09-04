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
        private IManyToManyRelationGetService<Student, HomeworkSubmissionDto> _getService;
        private IAddEntitiesService<CreateHomeworkSubmissionDto> _addService;
        public HomeworkSubmissionsController(IManyToManyRelationGetService<Student, HomeworkSubmissionDto> getService, IAddEntitiesService<CreateHomeworkSubmissionDto> addService)
        {
            _getService = getService;
            _addService = addService;
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
    }
}
