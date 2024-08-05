using HogwartsAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Controllers
{
    [Route("hogwarts/student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly HogwartDbContext _context;
        public StudentsController(HogwartDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _context.Students.Include(s => s.Wand).ThenInclude(w => w.Core).ToListAsync();

            return Ok(students);

        }
    }
}
