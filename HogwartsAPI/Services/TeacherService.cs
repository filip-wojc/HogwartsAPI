using AutoMapper;
using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Services
{
    public class TeacherService : IGetEntitiesService<TeacherDto>, IAddEntitiesService<CreateTeacherDto>, IModifyEntitiesService<ModifyTeacherDto>, IDeleteEntitiesService<Teacher>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        public TeacherService(HogwartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TeacherDto>> GetAll()
        {
            var teachers = await _context.Teachers.Include(t => t.Course).Include(t => t.House).Include(t => t.Wand).ThenInclude(w => w.Core).ToListAsync();
            return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto> GetById(int id)
        {
            var teacher = await GetTeacherById(id);
            return _mapper.Map<TeacherDto>(teacher);
        }
        public async Task<int> Create(CreateTeacherDto dto)
        {
            var teacher = _mapper.Map<Teacher>(dto);
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();

            return teacher.Id;
        }

        public async Task Delete(int id)
        {
            var teacher = await GetTeacherById(id);
            await _context.Teachers.Where(t => t.Id == id).ExecuteDeleteAsync();
        }  

        public async Task Modify(int id, ModifyTeacherDto dto)
        {
            var teacher = await GetTeacherById(id);
            teacher.WandId = dto.WandId;
            await _context.SaveChangesAsync();
        }

        private async Task<Teacher> GetTeacherById(int teacherId)
        {
            var teacher = await _context.Teachers.Include(t => t.Course).Include(t => t.House).Include(t => t.Wand).ThenInclude(w => w.Core).FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }
            return teacher;
        }
    }
}
