using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Enums;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;

namespace HogwartsAPI.Services
{
    public class TeacherPaginationService : IPaginationService<TeacherDto>
    {
        public PageResult<TeacherDto> GetPaginatedResult(PaginateQuery query, IEnumerable<TeacherDto> allTeachers)
        {
            var baseQuery = allTeachers.Where(t => query.SearchPhrase == null || t.Name.ToLower().Contains(query.SearchPhrase) || t.Surname.ToLower().Contains(query.SearchPhrase));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortSelector = new Dictionary<string, Func<TeacherDto, object>>
                {
                    { nameof(TeacherDto.DateOfBirth).ToLower(), w => w.DateOfBirth},
                    { nameof(TeacherDto.Name).ToLower(), w => w.Name},
                    { nameof(TeacherDto.Surname).ToLower(), w => w.Surname}
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
            var teachers = baseQuery.Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize).ToList();
            var totalTeachers = baseQuery.Count();

            return new PageResult<TeacherDto>(teachers, totalTeachers, query.PageSize, query.PageNumber);
        }
    }
}
