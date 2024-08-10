﻿using AutoMapper;
using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HogwartsAPI.Services
{
    public class AccountService : IAddEntitiesService<RegisterUserDto>, ILoginService
    {
        private readonly HogwartDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtParameters _jwtParameters;
        public AccountService(HogwartDbContext context, IMapper mapper, IPasswordHasher<User> hasher, JwtParameters jwtParameters)
        {
            _context = context;
            _hasher = hasher;
            _jwtParameters = jwtParameters;
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

        public async Task<string> GenerateJwt(LoginUserDto dto)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                throw new BadHttpRequestException("Invalid email or password");
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadHttpRequestException($"Invalid email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToShortDateString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtParameters.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_jwtParameters.JwtExpireDays);

            var token = new JwtSecurityToken(_jwtParameters.JwtIssuer, _jwtParameters.JwtIssuer, claims,
                expires: expires, signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        } 
    }
}
