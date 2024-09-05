using AutoMapper;
using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HogwartsAPI.Authorization;
using HogwartsAPI.Enums;

namespace HogwartsAPI.Services
{
    public class HomeworkService : IManyToManyRelationGetService<Course, HomeworkDto>, IAddEntitiesService<CreateHomeworkDto>, IManyToManyRelationDeleteService<Course, Homework>, IModifyEntitiesService<ModifyHomeworkDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        private readonly IAuthorizationService _authorizationService;
        public HomeworkService(HogwartDbContext context, IMapper mapper,
            IUserContextService userContext, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContext;
            _authorizationService = authorizationService;
        }

        public async Task<IEnumerable<HomeworkDto>> GetAllChildren(int parrentId)
        {
            var course = await GetCourseById(parrentId);
            return _mapper.Map<IEnumerable<HomeworkDto>>(course.Homeworks);
        }

        public async Task<HomeworkDto> GetChildById(int parrentId, int childId)
        {
            var course = await GetCourseById(parrentId);
            var homework = course.Homeworks.FirstOrDefault(h => h.Id == childId);
            if (homework is null)
            {
                throw new NotFoundException("Homework in this course not found");
            }
            return _mapper.Map<HomeworkDto>(homework);
        }

        public async Task<int> Create(CreateHomeworkDto dto)
        {
            var homework = _mapper.Map<Homework>(dto);
            homework.CreatedById = _userContext.UserId;

            await _context.Homeworks.AddAsync(homework);
            await _context.SaveChangesAsync();
            return homework.Id;
        }

        public async Task Modify(int id, ModifyHomeworkDto dto)
        {
            if (!dto.DueDate.HasValue && string.IsNullOrEmpty(dto.Description))
            {
                throw new BadHttpRequestException("Yous passed no data");
            }

            var homework = await GetHomeworkById(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, homework,
                new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't modify a homework you didn't add");
            }

            if (dto.DueDate.HasValue)
            {
                homework.DueDate = dto.DueDate.Value;
            }
            if (!string.IsNullOrEmpty(dto.Description))
            {
                homework.Description = dto.Description;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteChild(int parrentId, int childId)
        {
            var course = await GetCourseById(parrentId);
            var homework = await GetHomeworkById(childId);

            if (!course.Homeworks.Contains(homework))
            {
                throw new BadHttpRequestException("This homework does not exist in this course");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, homework,
                          new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't delete a homework you didn't add");
            }


            await _context.Homeworks.Where(h => h.Id == childId).ExecuteDeleteAsync();

        }

        public async Task DeleteAllChildren(int parrentId)
        {
            var course = await GetCourseById(parrentId);
            var homeworksToDelete = _context.Homeworks.Where(h => h.CreatedById == _userContext.UserId && h.CourseId == parrentId);
            if (!homeworksToDelete.Any() && _userContext.UserRole != "Admin")
            {
                throw new BadHttpRequestException("There are no homeworks that you can delete. You can only delete homeworks that you created");
            }
            await homeworksToDelete.ExecuteDeleteAsync();
        }

        private async Task<Course> GetCourseById(int courseId)
        {
            var course = await _context.Courses.
                Include(c => c.Homeworks).Include(c => c.Teacher).FirstOrDefaultAsync(c => c.Id == courseId);
            if(course is null)
            {
                throw new NotFoundException("Course not found");
            }
            return course;
        }

        private async Task<Homework> GetHomeworkById(int homeworkId)
        {
            var homework = await _context.Homeworks.Include(h => h.CreatedBy).FirstOrDefaultAsync(h => h.Id == homeworkId);
            if (homework is null)
            {
                throw new NotFoundException("Homework not found");
            }
            return homework;
        }

    }
}
