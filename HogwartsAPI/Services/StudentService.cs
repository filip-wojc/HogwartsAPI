using AutoMapper;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        public StudentService(HogwartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentsDto>> GetAll()
        {
            var students = await _context.Students.Include(s => s.House)
               .Include(s => s.Pets).Include(s => s.Courses).
               Include(s => s.Wand).ThenInclude(w => w.Core).ToListAsync();

            return _mapper.Map<List<StudentsDto>>(students);
        }

        public async Task<StudentsDto> GetById(int id)
        {
            var student = await _context.Students.Include(s => s.House)
                .Include(s => s.Pets).Include(s => s.Courses).
                 Include(s => s.Wand).ThenInclude(w => w.Core)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (student is null)
            {
                throw new NotFoundException("Student not found");
            }

            return _mapper.Map<StudentsDto>(student);
        }

        public async Task<int> Create(CreateStudentDto dto)
        {
            var student = _mapper.Map<Student>(dto);
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return student.Id;
        }
    }
}
