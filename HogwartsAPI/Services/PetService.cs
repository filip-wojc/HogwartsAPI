using AutoMapper;
using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Authorization;
using HogwartsAPI.Enums;

namespace HogwartsAPI.Services
{
    public class PetService : IGetEntitiesService<PetDto>, IAddEntitiesService<CreatePetDto>, IDeleteEntitiesService<Pet>, IManyToManyRelationGetService<Student, PetDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContext;
        public PetService(HogwartDbContext context, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContext)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContext = userContext;
        }

        public async Task<IEnumerable<PetDto>> GetAll()
        {
            var pets = await _context.Pets.Include(p => p.Student).ToListAsync();
            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        public async Task<PetDto> GetById(int id)
        {
            var pet = await GetPetById(id);
            return _mapper.Map<PetDto>(pet);
        }

        public async Task<IEnumerable<PetDto>> GetAllChildren(int parrentId)
        {
            var student = await _context.Students.Include(s => s.Pets).FirstOrDefaultAsync(s => s.Id == parrentId);
            if (student is null)
            {
                throw new NotFoundException("Student not found");
            }

            return _mapper.Map<IEnumerable<PetDto>>(student.Pets);
        }

        public async Task<int> Create(CreatePetDto dto)
        {
            var pet = _mapper.Map<Pet>(dto);
            pet.CreatedById = _userContext.UserId;
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();

            return pet.Id;
        }

        public async Task Delete(int id)
        {
            var pet = await GetPetById(id);
           
            var authorizeResult = await _authorizationService.AuthorizeAsync(_userContext.User, pet,
                new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizeResult.Succeeded)
            {
                throw new ForbidException("You can't delete pet you didn't add");
            }
            await _context.Pets.Where(p => p.Id == id).ExecuteDeleteAsync();
        }
        
        private async Task<Pet> GetPetById(int petId)
        {
            var pet = await _context.Pets.Include(p => p.Student).FirstOrDefaultAsync(p => p.Id == petId);
            if (pet is null)
            {
                throw new NotFoundException("Pet not found");
            }
            return pet;
        }

        public Task<PetDto> GetChildById(int parrentId, int childId)
        {
            throw new NotImplementedException();
        }
    }
}
