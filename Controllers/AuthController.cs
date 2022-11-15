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
        public IAuthService _authService;

        public AuthController( IAuthService authService)
        {
            _authService = authService;
        }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDTO registerUserDto)
        {
            try
            {
                return Ok(_authService.Register(registerUserDto));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

        } 
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthUserDTO authUserDTO)
        {
            try
            {
                return Ok (_authService.Login(authUserDTO));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

        } 

    }
}