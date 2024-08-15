using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using HogwartsAPI.Dtos.StudentDtos;
using AutoMapper;

namespace HogwartsAPI.Services
{
    public class CourseStudentsService : IManyToManyRelationGetService<Course, StudentDto>, IManyToManyRelationAddService<Course, Student>, IManyToManyRelationDeleteService<Course, Student>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        public CourseStudentsService(HogwartDbContext context, IMapper mapper, IUserContextService userContext)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<IEnumerable<StudentDto>> GetAllChildren(int parrentId)
        {
            var course = await GetCourseById(parrentId);
            return _mapper.Map<List<StudentDto>>(course.Students);
        }

        public async Task<StudentDto> GetChildById(int parrentId, int childId)
        {
            var course = await GetCourseById(parrentId);
            var student = course.Students.FirstOrDefault(s => s.Id == childId);

            if (student is null)
            {
                throw new NotFoundException("Student in this course not found");
            }

            return _mapper.Map<StudentDto>(student);
        }

        public async Task Create(int parrentId, int childId)
        {
            var course = await GetCourseById(parrentId);

            if (course.CreatedById != _userContext.UserId && _userContext.UserRole != "Admin")
            {
                throw new ForbidException("You can't add students to the course you didn't create");
            }

            var student = await GetStudentById(childId);

            if (course.Students.Contains(student))
            {
                throw new BadHttpRequestException("This student belongs to this course");
            }

            course.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllChildren(int parrentId)
        {
            var course = await GetCourseById(parrentId);

            if (course.CreatedById != _userContext.UserId && _userContext.UserRole != "Admin")
            {
                throw new ForbidException("You can't delete students from the course you didn't create");
            }

            if (!course.Students.Any())
            {
                throw new BadHttpRequestException("This course does not have any students");
            }
            course.Students.Clear();
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChild(int parrentId, int childId)
        {
            var course = await GetCourseById(parrentId);

            if (course.CreatedById != _userContext.UserId && _userContext.UserRole != "Admin")
            {
                throw new ForbidException("You can't delete student from the course you didn't create");
            }

            var student = await GetStudentById(childId);

            if(!course.Students.Contains(student))
            {
                throw new BadHttpRequestException("This student is not signed up to this course");
            }

            course.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        private async Task<Course> GetCourseById(int courseId)
        {
            var course = await _context.Courses.Include(c => c.Students)
                .ThenInclude(s => s.House)
                .FirstOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
            {
                throw new NotFoundException("Course not found");
            }
            return course;
        }
        private async Task<Student> GetStudentById(int studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student is null)
            {
                throw new NotFoundException("Student not found");
            }
            return student;
        }
    }
}
