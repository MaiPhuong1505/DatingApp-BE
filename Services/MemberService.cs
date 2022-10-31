using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data;
using DatingApp.API.Data.Entities;
using DatingApp.API.DTOs;

namespace DatingApp.API.Services
{
    public class MemberService : IMemberService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MemberService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public MemberDTO GetMemberByUsername(string username)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.Usename == username);
            if (user == null) return null;
            return _mapper.Map<User, MemberDTO>(user);

            // return new MemberDTO
            // {
            //     Avatar = user.Avatar,
            //     City = user.City,
            //     DateOfBirth = user.DateOfBirth,
            //     Email = user.Email,
            //     Gender = user.Gender,
            //     Introduction = user.Introduction,
            //     KnownAs = user.KnownAs,
            //     Usename = user.Usename,
            // };
        }

        public List<MemberDTO> GetMembers()
        {
            return _context.AppUsers.
            ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToList();
            // var users = _context.AppUsers.ToList();
            // return _mapper.Map<List<User>, MemberDTO>(users);

            // return _context.AppUsers
            // .Select(user => new MemberDTO
            // {
            //     Avatar = user.Avatar,
            //     City = user.City,
            //     DateOfBirth = user.DateOfBirth,
            //     Email = user.Email,
            //     Gender = user.Gender,
            //     Introduction = user.Introduction,
            //     KnownAs = user.KnownAs,
            //     Usename = user.Usename,
            // })
            // .ToList();
        }
    }
}