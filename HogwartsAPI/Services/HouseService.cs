using AutoMapper;
using HogwartsAPI.Dtos.HouseDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using HogwartsAPI.Dtos.StudentDtos;

namespace HogwartsAPI.Services
{
    public class HouseService : IGetEntitiesService<HouseDto>, IModifyEntitiesService<ModifyHouseDto>, IManyToManyRelationGetService<House, StudentDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        public HouseService(HogwartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<HouseDto>> GetAll()
        {
            var houses = await _context.Houses.Include(h => h.Students).Include(h => h.Teacher).ToListAsync();
            return _mapper.Map<IEnumerable<HouseDto>>(houses);
        }

        public async Task<HouseDto> GetById(int id)
        {
            var house = await GetHouseById(id);
            return _mapper.Map<HouseDto>(house);
        }

        public async Task<StudentDto> GetChildById(int parrentId, int childId)
        {
            var house = await GetHouseById(parrentId);
            var student = house.Students.FirstOrDefault(s => s.Id == childId);
            if (student is null)
            {
                throw new NotFoundException("Student in this house not found");
            }
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<IEnumerable<StudentDto>> GetAllChildren(int parrentId)
        {
            var house = await GetHouseById(parrentId);
            return _mapper.Map<IEnumerable<StudentDto>>(house.Students);
        }

        public async Task Modify(int id, ModifyHouseDto dto)
        {
            if (!(dto.TeacherId.HasValue || dto.TrophyCount.HasValue))
            {
                throw new BadHttpRequestException("Yous passed no data");
            }
            var house = await GetHouseById(id);

            if (dto.TeacherId.HasValue)
            {
                house.TeacherId = dto.TeacherId.Value;
            }
            if (dto.TrophyCount.HasValue)
            {
                house.TrophyCount = dto.TrophyCount.Value;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<House> GetHouseById(int houseId)
        {
            var house = await _context.Houses.Include(h => h.Students).Include(h => h.Teacher).FirstOrDefaultAsync(h => h.Id == houseId);
            if (house is null)
            {
                throw new NotFoundException("House not found");
            }
            return house;
        }
    }
}
