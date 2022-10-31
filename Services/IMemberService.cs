using DatingApp.API.DTOs;

namespace DatingApp.API.Services
{
    public interface IMemberService
    {
        List<MemberDTO> GetMembers();
        MemberDTO GetMemberByUsername(string username);
    }
}