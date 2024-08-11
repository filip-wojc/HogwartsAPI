using AutoMapper;
using HogwartsAPI.Authorization;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HogwartsAPI.Enums;

namespace HogwartsAPI.Services
{
    public class WandService : IGetEntitiesService<WandDto>, IAddEntitiesService<CreateWandDto>, IDeleteEntitiesService<Wand>, IModifyEntitiesService<ModifyWandDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        private readonly IAuthorizationService _authorizationService;
        public WandService(HogwartDbContext context, IMapper mapper, IUserContextService userContext, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContext;
            _authorizationService = authorizationService;
        }
        public async Task<IEnumerable<WandDto>> GetAll()
        {
            var wands = await _context.Wands.Include(w => w.Core).Include(w => w.Core).Include(w => w.StudentOwners).Include(w => w.TeacherOwners).ToListAsync();
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
            wand.CreatedById = _userContext.UserId;

            await _context.Wands.AddAsync(wand);
            await _context.SaveChangesAsync();
            return wand.Id;
        }

        public async Task Delete(int id)
        {
            var wand = await GetWandById(id);
            if (wand.TeacherOwners.Any() || wand.StudentOwners.Any())
            {
                throw new ForbidException("This wand has an owner");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, wand,
                new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't delete a wand you didn't add");
            }

            await _context.Wands.Where(w => w.Id == id).ExecuteDeleteAsync();
        }

        public async Task Modify(int id, ModifyWandDto dto)
        {
            var wand = await GetWandById(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, wand,
                new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You can't modify a wand you didn't add");
            }

            wand.Price = dto.Price;
            await _context.SaveChangesAsync();
        }

        private async Task<Wand> GetWandById(int id)
        {
            var wand = await _context.Wands.Include(w => w.Core).Include(w => w.StudentOwners).Include(w => w.TeacherOwners).FirstOrDefaultAsync(w => w.Id == id);
            if (wand is null)
            {
                throw new NotFoundException("Wand not found");
            }

            return wand;
        }

    }
}
