using AutoMapper;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Services
{
    public class StudentService : IGetEntitiesService<StudentDto>, IAddEntitiesService<CreateStudentDto>, IDeleteEntitiesService<Student>, IModifyEntitiesService<ModifyStudentDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        public StudentService(HogwartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> GetAll()
        {
            var students = await _context.Students.Include(s => s.House)
               .Include(s => s.Pets).Include(s => s.Courses).
               Include(s => s.Wand).ThenInclude(w => w.Core).ToListAsync();

            return _mapper.Map<List<StudentDto>>(students);
        }

        public async Task<StudentDto> GetById(int id)
        {
            var student = await GetStudentById(id);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<int> Create(CreateStudentDto dto)
        {
            var student = _mapper.Map<Student>(dto);
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return student.Id;
        }

        public async Task Delete(int id)
        {
            var student = await GetStudentById(id);
            await _context.Students.Where(s => s.Id == id).ExecuteDeleteAsync();
        }

        public async Task Modify(int id, ModifyStudentDto dto)
        {
            if (!(dto.WandId.HasValue || dto.SchoolYear.HasValue))
            {
                throw new BadHttpRequestException("You passed no data");
            }
            var student = await GetStudentById(id);
          
            if(dto.SchoolYear.HasValue)
            {
                student.SchoolYear = dto.SchoolYear.Value;
            }
            if(dto.WandId.HasValue)
            {
                student.WandId = dto.WandId.Value;
            }
          
            await _context.SaveChangesAsync();
        }

        private async Task<Student> GetStudentById(int id)
        {
            var student = await _context.Students.Include(s => s.House)
               .Include(s => s.Pets).Include(s => s.Courses).
                Include(s => s.Wand).ThenInclude(w => w.Core)
               .FirstOrDefaultAsync(s => s.Id == id);
            if (student is null)
            {
                throw new NotFoundException("Student not found");
            }

            return student;
        }

       
    }
}
