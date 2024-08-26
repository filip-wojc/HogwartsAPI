using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Enums;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;

namespace HogwartsAPI.Services
{
    public class StudentPaginationService : IPaginationService<StudentDto>
    {
        public PageResult<StudentDto> GetPaginatedResult(PaginateQuery query, IEnumerable<StudentDto> allStudents)
        {
            var baseQuery = allStudents.Where(s => query.SearchPhrase == null || s.Name.ToLower().Contains(query.SearchPhrase) || s.Surname.ToLower().Contains(query.SearchPhrase));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortSelector = new Dictionary<string, Func<StudentDto, object>>
                {
                    { nameof(StudentDto.DateOfBirth).ToLower(), w => w.DateOfBirth},
                    { nameof(StudentDto.Name).ToLower(), w => w.Name},
                    { nameof(StudentDto.Surname).ToLower(), w => w.Surname}
                };

                if (!sortSelector.ContainsKey(query.SortBy.ToLower()))
                {
                    throw new BadHttpRequestException($"Invalid sortBy value: {query.SortBy}");
                }

                var selectedSort = sortSelector[query.SortBy.ToLower()];

                baseQuery = query.SortDirection == SortDirection.Asc
                    ?
                    baseQuery.OrderBy(selectedSort)
                    : baseQuery.OrderByDescending(selectedSort);
            }
            var students = baseQuery.Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize).ToList();
            var totalStudents = baseQuery.Count();

            return new PageResult<StudentDto>(students, totalStudents, query.PageSize, query.PageNumber);
        }
    }
}
