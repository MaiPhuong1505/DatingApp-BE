using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data.Entities;
using DatingApp.API.Data.Repositories;
using DatingApp.API.DTOs;

namespace DatingApp.API.Services
{
  public class AuthService : IAuthService
  {
    private readonly IUserRepository _userRepository;
    private readonly iTokenService _tokenService;
    private readonly IMapper _mapper;
    public AuthService(IUserRepository userRepository, iTokenService tokenService, IMapper mapper)
    {
      _userRepository = userRepository;
      _tokenService = tokenService;
      _mapper = mapper;
    }
    public string Login(AuthUserDTO authUserDTO)
    {
      authUserDTO.Username = authUserDTO.Username.ToLower();
      var currentUser = _userRepository.GetUserByUserName(authUserDTO.Username);
      if (currentUser == null)
      {
        throw new UnauthorizedAccessException("Username is invalid");
      }
      using var hmac = new HMACSHA512(currentUser.PasswordSalt);
      var passwordBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(authUserDTO.Password));
      for (int i = 0; i < currentUser.PasswordHash.Length; i++)
      {
        if (currentUser.PasswordHash[i] != passwordBytes[i])
        {
          throw new UnauthorizedAccessException("Password is invalid");
        }
      }
      return _tokenService.CreateToken(currentUser.Usename);
    }

    public string Register(RegisterUserDTO registerUserDto)
    {
      registerUserDto.Username = registerUserDto.Username.ToLower();
      var currentUser = _userRepository.GetUserByUserName(registerUserDto.Username);
      if (currentUser != null)
      {
        throw new BadHttpRequestException("Username is already register");
      }
      using var hmac = new HMACSHA512();
      var passwordBytes = Encoding.UTF8.GetBytes(registerUserDto.Password);
      var newUser = _mapper.Map<RegisterUserDTO, User>(registerUserDto);
      newUser.PasswordSalt = hmac.Key;
      newUser.PasswordHash = hmac.ComputeHash(passwordBytes);
      //   var newUser = new User
      //   {
      //     Usename = registerUserDto.Username,
      //     PasswordSalt = hmac.Key,
      //     PasswordHash = hmac.ComputeHash(passwordBytes)
      //   };
      _userRepository.InsertNewUser(newUser);
      _userRepository.IsSaveChanges();
      return _tokenService.CreateToken(newUser.Usename);
    }
  }
}