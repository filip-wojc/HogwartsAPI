using AutoMapper;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentsDto>>> GetAll()
        {
            var students = await _service.GetAll();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var student = await _service.GetById(id);
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStudentDto dto)
        {
            int studentId = await _service.Create(dto);
            return Created($"/api/student/{studentId}", null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
