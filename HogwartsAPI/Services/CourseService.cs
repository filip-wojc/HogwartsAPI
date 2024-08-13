using AutoMapper;
using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Dtos.CourseDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HogwartsAPI.Authorization;
using HogwartsAPI.Enums;

namespace HogwartsAPI.Services
{
    public class CourseService : IGetEntitiesService<CourseDto>, IAddEntitiesService<CreateCourseDto>, IModifyEntitiesService<ModifyCourseDto>, IDeleteEntitiesService<Course>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        private readonly IAuthorizationService _authorizationService;
        public CourseService(HogwartDbContext context, IMapper mapper, IUserContextService userContext, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContext;
            _authorizationService = authorizationService;
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


        public async Task<int> Create(CreateCourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            course.CreatedById = _userContext.UserId;

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course.Id;
        }

        public async Task Modify(int id, ModifyCourseDto dto)
        {
            if (!(dto.DifficultyLevel.HasValue || dto.TeacherId.HasValue))
            {
                throw new BadHttpRequestException("You passed no data");
            }

            var course = await GetCourseById(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, course,
                new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't modify a course you didn't add");
            }

            if (dto.DifficultyLevel.HasValue)
            {
                course.DifficultyLevel = dto.DifficultyLevel.Value;
            }
            if (dto.TeacherId.HasValue)
            {
                course.TeacherId = dto.TeacherId.Value;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var course = await GetCourseById(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, course,
                new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't delete a course you didn't add");
            }

            await _context.Courses.Where(c => c.Id == id).ExecuteDeleteAsync();
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
