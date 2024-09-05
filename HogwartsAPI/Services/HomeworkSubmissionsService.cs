using AutoMapper;
using HogwartsAPI.Dtos.HomeworkSubmissionsDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using HogwartsAPI.Authorization;
using HogwartsAPI.Enums;
using HogwartsAPI.Migrations;
using Microsoft.AspNetCore.Authorization;

namespace HogwartsAPI.Services
{
    public class HomeworkSubmissionsService : IManyToManyRelationGetService<Student, HomeworkSubmissionDto>, IAddEntitiesService<CreateHomeworkSubmissionDto>,
                    IManyToManyRelationDeleteService<Student, HomeworkSubmission>, IModifyEntitiesService<ModifyHomeworkSubmissionDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;
        public HomeworkSubmissionsService(HogwartDbContext context, IMapper mapper, IUserContextService userContext, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
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
            if (homeworkSubmission is null)
            {
                throw new BadHttpRequestException("This student doesn't have this submission");
            }
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

        public async Task Modify(int id, ModifyHomeworkSubmissionDto dto)
        {
            var homeworkSubmission = await _context.HomeworkSubmissions.FirstOrDefaultAsync(h => h.Id == id);
            if (homeworkSubmission is null)
            {
                throw new NotFoundException("Homework submission not found");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, homeworkSubmission,
                         new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't modify a homework submission you didn't add");
            }

            if (!(dto.Grade.HasValue || dto.Feedback != null))
            {
                throw new BadHttpRequestException("You passed no data");
            }

            if (dto.Grade.HasValue)
            {
                homeworkSubmission.Grade = dto.Grade.Value;
                homeworkSubmission.SubmissionDate = DateTime.Now;
            }
            if (dto.Feedback != null)
            {
                homeworkSubmission.Feedback = dto.Feedback;
                homeworkSubmission.SubmissionDate = DateTime.Now;
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChild(int parrentId, int childId)
        {
            if (!await IsStudent(parrentId))
            {
                throw new NotFoundException("Student not found");
            }
            else if (!await IsHomeworkSubmission(childId))
            {
                throw new NotFoundException("Homework submission not found");
            }
            var homeworkSubmission = await _context.HomeworkSubmissions.FirstOrDefaultAsync(h => h.StudentId == parrentId && h.Id == childId);
            if (homeworkSubmission is null)
            {
                throw new BadHttpRequestException("This student doesn't have this submission");
            }
           
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, homeworkSubmission,
                          new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't delete a homework submission you didn't add");
            }

            await _context.HomeworkSubmissions.Where(h => h.Id == childId).ExecuteDeleteAsync();

        }

        public async Task DeleteAllChildren(int parrentId)
        {
            if (!await IsStudent(parrentId))
            {
                throw new NotFoundException("Student not found");
            }
            var homeworkSubmissionsToDelete = _context.HomeworkSubmissions.Where(h => h.CreatedById == _userContextService.UserId && h.StudentId == parrentId);
            if (!homeworkSubmissionsToDelete.Any())
            {
                throw new BadHttpRequestException("There are no homework submissions that you can delete");
            }
            else if (_userContextService.UserRole != "Admin")
            {
                throw new ForbidException("You can only delete homework submissions that you created");
            }
            await homeworkSubmissionsToDelete.ExecuteDeleteAsync();
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
