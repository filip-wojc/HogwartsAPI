using AutoMapper;
using HogwartsAPI.Dtos.HomeworkSubmissionsDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Services
{
    public class HomeworkSubmissionsService : IManyToManyRelationGetService<Student, HomeworkSubmissionDto>, IAddEntitiesService<CreateHomeworkSubmissionDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        public HomeworkSubmissionsService(HogwartDbContext context, IMapper mapper, IUserContextService userContext, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<HomeworkSubmissionDto>> GetAllChildren(int parrentId)
        {
            if (!await IsStudent(parrentId))
            {
                throw new NotFoundException("Student not found");
            }
            var homeworkSubmissions = await _context.HomeworkSubmissions.Include(h => h.Student).Include(h => h.Homework).Where(h => h.StudentId == parrentId).ToListAsync();
            if(!homeworkSubmissions.Any())
            {
                throw new NotFoundException("This student doesn't have any submissions");
            }
            return _mapper.Map<IEnumerable<HomeworkSubmissionDto>>(homeworkSubmissions);
        }

        public async Task<HomeworkSubmissionDto> GetChildById(int parrentId, int childId)
        {
            if (!await IsStudent(parrentId))
            {
                throw new NotFoundException("Student not found");
            }
            else if (!await IsHomeworkSubmission(childId))
            {
                throw new NotFoundException("Homework submission not found");
            }

            var homeworkSubmission = await _context.HomeworkSubmissions.Include(h => h.Student).Include(h => h.Homework).FirstOrDefaultAsync(h => h.StudentId == parrentId && h.Id == childId);
            return _mapper.Map<HomeworkSubmissionDto>(homeworkSubmission);
        }

        public async Task<int> Create(CreateHomeworkSubmissionDto dto)
        {
            var homeworkSubmission = _mapper.Map<HomeworkSubmission>(dto);
            homeworkSubmission.CreatedById = _userContextService.UserId;

            await _context.HomeworkSubmissions.AddAsync(homeworkSubmission);
            await _context.SaveChangesAsync();

            return homeworkSubmission.Id;
        }

        private async Task<bool> IsStudent(int studentId)
        {
            return await _context.Students.AnyAsync(s => s.Id == studentId);
        }

        private async Task<bool> IsHomeworkSubmission(int homeworkSubmissionId)
        {
            return await _context.HomeworkSubmissions.AnyAsync(h => h.Id == homeworkSubmissionId);
        }
    }
}
