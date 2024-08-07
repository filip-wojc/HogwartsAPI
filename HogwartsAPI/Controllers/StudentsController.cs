using AutoMapper;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IGetEntitiesService<StudentDto> _getService;
        private readonly IAddEntitiesService<CreateStudentDto> _addService;
        private readonly IDeleteEntitiesService _deleteService;
        public StudentsController(IGetEntitiesService<StudentDto> getService, IAddEntitiesService<CreateStudentDto> addService, IDeleteEntitiesService deleteService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll()
        {
            var students = await _getService.GetAll();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var student = await _getService.GetById(id);
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStudentDto dto)
        {
            int studentId = await _addService.Create(dto);
            return Created($"/api/student/{studentId}", null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _deleteService.Delete(id);
            return NoContent();
        }
    }
}
