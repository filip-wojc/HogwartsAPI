using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Enums;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;

namespace HogwartsAPI.Services
{
    public class CoursePaginationService : IPaginationService<CourseDto>
    {
        public PageResult<CourseDto> GetPaginatedResult(PaginateQuery query, IEnumerable<CourseDto> allCourses)
        {
            var baseQuery = allCourses.Where(c => query.SearchPhrase == null || c.Name.ToLower().Contains(query.SearchPhrase) || c.Description.ToLower().Contains(query.SearchPhrase));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortSelector = new Dictionary<string, Func<CourseDto, object>>
                {
                    { nameof(CourseDto.DifficultyLevel).ToLower(), c => c.DifficultyLevel},
                    { nameof(CourseDto.Name).ToLower(), c => c.Name},
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
            var courses = baseQuery.Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize).ToList();
            var totalCourses = baseQuery.Count();

            return new PageResult<CourseDto>(courses, totalCourses, query.PageSize, query.PageNumber);
        }
    }
}
