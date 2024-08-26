using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Route("api/course/{courseId}/students")]
    [Authorize]
    public class CourseStudentsController : ControllerBase
    {
        private readonly IManyToManyRelationGetService<Course, StudentDto> _getService;
        private readonly IManyToManyRelationAddService<Course, Student> _addService;
        private readonly IManyToManyRelationDeleteService<Course, Student> _deleteService;
        private readonly IPaginationService<StudentDto> _paginationService;
        public CourseStudentsController(IManyToManyRelationGetService<Course, StudentDto> getService, IManyToManyRelationAddService<Course, Student> addService,
            IManyToManyRelationDeleteService<Course, Student> deleteService, IPaginationService<StudentDto> paginationService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
            _paginationService = paginationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll([FromRoute] int courseId, [FromQuery] PaginateQuery query)
        {
            var students = await _getService.GetAllChildren(courseId);
            var paginateResult = _paginationService.GetPaginatedResult(query, students);
            return Ok(paginateResult);
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<StudentDto>> GetStudentFromCourse([FromRoute] int courseId, [FromRoute] int studentId)
        {
            var student = await _getService.GetChildById(courseId, studentId);
            return Ok(student);
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpPost("{studentId}")]
        public async Task<ActionResult> Create([FromRoute] int courseId, [FromRoute] int studentId)
        {
            await _addService.Create(courseId, studentId);
            return Created($"api/course/{courseId}/students/{studentId}", null);
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAllStudents([FromRoute] int courseId)
        {
            await _deleteService.DeleteAllChildren(courseId);
            return NoContent();
        }

        [Authorize(Roles = "CourseManager,Admin")]
        [HttpDelete("{studentId}")]
        public async Task<ActionResult> DeleteStudent([FromRoute] int courseId, [FromRoute] int studentId)
        {
            await _deleteService.DeleteChild(courseId, studentId);
            return NoContent();
        }
    }
}
