using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Data.Entities;
using DatingApp.API.DTOs;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly iTokenService _tokenService;

        public AuthController(DataContext context, iTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUserDTO authUserDTO)
        {
            authUserDTO.Username = authUserDTO.Username.ToLower();
            if (_context.AppUsers.Any(u => u.Usename == authUserDTO.Username))
            {
                return BadRequest("Username is already register");
            }
            using var hmac = new HMACSHA512();
            var passwordBytes = Encoding.UTF8.GetBytes(authUserDTO.Password);
            var newUser = new User
            {
                Usename = authUserDTO.Username,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(passwordBytes)
            };
            _context.AppUsers.Add(newUser);
            _context.SaveChanges();
            var token = _tokenService.CreateToken(newUser.Usename);
            return Ok(token);
        } 
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthUserDTO authUserDTO)
        {
            authUserDTO.Username = authUserDTO.Username.ToLower();
            var currentUser = _context.AppUsers.FirstOrDefault(u => u.Usename == authUserDTO.Username);
            if(currentUser == null)
            {
                return Unauthorized("Username is invalid");
            }
            using var hmac = new HMACSHA512(currentUser.PasswordSalt);
            var passwordBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(authUserDTO.Password));
            for (int i = 0; i < currentUser.PasswordHash.Length; i++)
            {
                if (currentUser.PasswordHash[i] != passwordBytes[i])
                {
                    return Unauthorized("Password is invalid");
                }
            }
            var token = _tokenService.CreateToken(currentUser.Usename);
            return Ok(token);
        } 

    }
}