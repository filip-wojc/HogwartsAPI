using AutoMapper;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HogwartsAPI.Controllers
{
    [Route("api/student")]
    [Authorize]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IGetEntitiesService<StudentDto> _getService;
        private readonly IAddEntitiesService<CreateStudentDto> _addService;
        private readonly IDeleteEntitiesService<Student> _deleteService;
        private readonly IModifyEntitiesService<ModifyStudentDto> _modifyService;
        public StudentsController(IGetEntitiesService<StudentDto> getService, IAddEntitiesService<CreateStudentDto> addService, IDeleteEntitiesService<Student> deleteService, IModifyEntitiesService<ModifyStudentDto> modifyService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
            _modifyService = modifyService;
        }

        [HttpGet]
        [Authorize(Policy = "AtLeast15")]
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Modify([FromRoute] int id, [FromBody] ModifyStudentDto dto)
        {
            await _modifyService.Modify(id, dto);
            return Ok();
        }
    }
}
