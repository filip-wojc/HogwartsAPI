using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
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
        public CourseStudentsController(IManyToManyRelationGetService<Course, StudentDto> getService, IManyToManyRelationAddService<Course, Student> addService, IManyToManyRelationDeleteService<Course, Student> deleteService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll([FromRoute] int courseId)
        {
            var students = await _getService.GetAllChildren(courseId);
            return Ok(students);
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll([FromRoute] int courseId, [FromRoute] int studentId)
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
