using AutoMapper;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;
using HogwartsAPI.Tools;
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
        private readonly IPaginationService<StudentDto> _paginateService;
        public StudentsController(IGetEntitiesService<StudentDto> getService, IAddEntitiesService<CreateStudentDto> addService,
            IDeleteEntitiesService<Student> deleteService, IModifyEntitiesService<ModifyStudentDto> modifyService, IPaginationService<StudentDto> paginateService)
        {
            _getService = getService;
            _addService = addService;
            _deleteService = deleteService;
            _modifyService = modifyService;
            _paginateService = paginateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll([FromQuery] PaginateQuery query)
        {
            var students = await _getService.GetAll();
            var paginateResult = _paginateService.GetPaginatedResult(query, students);
            return Ok(paginateResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var student = await _getService.GetById(id);
            return Ok(student);
        }

        [Authorize(Roles = "StudentManager,Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStudentDto dto)
        {
            int studentId = await _addService.Create(dto);
            return Created($"/api/student/{studentId}", null);
        }

        [Authorize(Roles = "StudentManager,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _deleteService.Delete(id);
            return NoContent();
        }

        [Authorize(Roles = "StudentManager,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Modify([FromRoute] int id, [FromBody] ModifyStudentDto dto)
        {
            await _modifyService.Modify(id, dto);
            return Ok();
        }
    }
}
