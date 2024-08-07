using AutoMapper;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Services
{
    public class WandService : IGetEntitiesService<WandDto>, IAddEntitiesService<CreateWandDto>, IDeleteEntitiesService
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        public WandService(HogwartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<WandDto>> GetAll()
        {
            var wands = await _context.Wands.Include(w => w.Core).ToListAsync();
            return _mapper.Map<List<WandDto>>(wands);
        }

        public async Task<WandDto> GetById(int id)
        {
            var wand = await GetWandById(id);
            return _mapper.Map<WandDto>(wand);
        }
        public async Task<int> Create(CreateWandDto dto)
        {
            var wand = _mapper.Map<Wand>(dto);
            await _context.Wands.AddAsync(wand);
            await _context.SaveChangesAsync();
            return wand.Id;
        }

        public async Task Delete(int id)
        {
            var wand = GetWandById(id);
            _context.Remove(wand);
            await _context.SaveChangesAsync();
        }
        
        private async Task<Wand> GetWandById(int id)
        {
            var wand = await _context.Wands.Include(w => w.Core).FirstOrDefaultAsync(w => w.Id == id);
            if (wand is null)
            {
                throw new NotFoundException("Wand not found");
            }

            return wand;
        }

    }
}
