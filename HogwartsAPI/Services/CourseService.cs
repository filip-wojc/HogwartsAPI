using AutoMapper;
using HogwartsAPI.Dtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Services
{
    public class CourseService : IGetEntitiesService<CourseDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        public CourseService(HogwartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CourseDto>> GetAll()
        {
            var courses = await _context.Courses.Include(c => c.Teacher).Include(c => c.Students).ToListAsync();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<CourseDto> GetById(int id)
        {
            var course = await GetCourseById(id);
            return _mapper.Map<CourseDto>(course);
        }

        private async Task<Course> GetCourseById(int id)
        {
            var course = await _context.Courses.Include(c => c.Teacher).Include(c => c.Students).FirstOrDefaultAsync(s => s.Id == id);
            if (course is null)
            {
                throw new NotFoundException("Course not found");
            }

            return course;
        }
    }
}
