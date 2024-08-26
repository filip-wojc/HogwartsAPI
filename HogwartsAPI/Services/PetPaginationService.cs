using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Enums;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;

namespace HogwartsAPI.Services
{
    public class PetPaginationService : IPaginationService<PetDto>
    {
        public PageResult<PetDto> GetPaginatedResult(PaginateQuery query, IEnumerable<PetDto> allPets)
        {
            var baseQuery = allPets.Where(p => query.SearchPhrase == null || p.Name.ToLower().Contains(query.SearchPhrase) || p.Type.ToLower().Contains(query.SearchPhrase));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortSelector = new Dictionary<string, Func<PetDto, object>>
                {
                    { nameof(PetDto.Name).ToLower(), p => p.Name},
                    { nameof(PetDto.OwnerName).ToLower(), p => p.OwnerName},
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
            var pets = baseQuery.Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize).ToList();
            var totalPets = baseQuery.Count();

            return new PageResult<PetDto>(pets, totalPets, query.PageSize, query.PageNumber);
        }
    }
}
