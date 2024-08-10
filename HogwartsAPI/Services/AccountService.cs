using AutoMapper;
using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HogwartsAPI.Services
{
    public class AccountService : IAddEntitiesService<RegisterUserDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _hasher;
        public AccountService(HogwartDbContext context, IMapper mapper, IPasswordHasher<User> hasher)
        {
            _context = context;
            _mapper = mapper;
            _hasher = hasher;
        }
        public async Task<int> Create(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Username = dto.Username,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                RoleId = dto.RoleId,
            };

            var hash = _hasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hash;
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser.Id;
        }
    }
}
