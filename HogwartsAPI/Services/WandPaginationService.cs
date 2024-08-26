using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Enums;
using AutoMapper;
using HogwartsAPI.Tools;
using System.Linq.Expressions;

namespace HogwartsAPI.Services
{
    public class WandPaginationService : IPaginationService<WandDto>
    {
        private readonly IMapper _mapper;
        public WandPaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public PageResult<WandDto> GetPaginatedResult(IPaginateQuery query, IEnumerable<WandDto> allWands)
        {
            var baseQuery = allWands.Where(w => query.SearchPhrase == null || w.CoreName.ToLower().Contains(query.SearchPhrase));
            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var sortSelector = new Dictionary<string, Func<WandDto, object>>
                {
                    { nameof(WandDto.CoreName).ToLower(), w => w.CoreName},
                    { nameof(WandDto.Price).ToLower(), w => w.Price},
                    { nameof(WandDto.Length).ToLower() , w => w.Length}
                };

                if(!sortSelector.ContainsKey(query.SortBy.ToLower()))
                {
                    throw new BadHttpRequestException($"Invalid sortBy value: {query.SortBy}");
                }

                var selectedSort = sortSelector[query.SortBy.ToLower()];

                baseQuery = query.SortDirection == SortDirection.Asc
                    ?
                    baseQuery.OrderBy(selectedSort)
                    : baseQuery.OrderByDescending(selectedSort);
            }
            var wands = baseQuery.Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize).ToList();
            var totalWands = baseQuery.Count();

            return new PageResult<WandDto>(wands, totalWands, query.PageSize, query.PageNumber);
        }
    }
}
