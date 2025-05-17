using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entites;
using AuthenticationApi.Infrastructure.Data;
using ECommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Infrastructure.Repositories
{
    internal class UserRepository(AuthenticationDBContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<GEtUserDTO> GetUser(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }
            var userDTO = new GEtUserDTO
            (
                user.Id,
                user.Name!,
                 user.TelephoneNumber!,
               user.Email!,
                user.Address!,
                user.Role!
            );
            return userDTO;
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var getUser = await GetUserByEmail(loginDTO.Email);
            if (getUser == null)
            {
                return new Response(false, "invalid Credentials");
            }
            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (!verifyPassword)
            {
                return new Response(false, "Invalid Credentials");
            }
            string token = GenerateToken(getUser);
            return new Response(true, token);
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name , user.Name!),
                new (ClaimTypes.Email , user.Email!),
                new (ClaimTypes.Role , user.Role!),
            }; 
            if (!string.IsNullOrEmpty(user.Role) ||!Equals("string",user.Role)) 
            claims.Add(new(ClaimTypes.Role, user.Role!));
            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: null,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var getUser = GetUserByEmail(appUserDTO.Email);
            if (getUser is null)
            {
                return new Response (false, "User already exists");
            }
            var result = context.Users.Add(new AppUser()
            {
                Name = appUserDTO?.Name,
                Email = appUserDTO?.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
                TelephoneNumber = appUserDTO?.TelephoneNumber,
                Address = appUserDTO.Address,
                Role = appUserDTO.Role
            });
            await context.SaveChangesAsync();
            return result.Entity.Id > 0 ? new Response(true, "User registered successfully"): new Response(false, "User registration failed"); ;
        }
    }
}
