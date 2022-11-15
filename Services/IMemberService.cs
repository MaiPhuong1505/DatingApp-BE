using DatingApp.API.DTOs;

namespace DatingApp.API.Services
{
  public interface IMemberService
  {
    List<MemberDTO> GetMembers(MemberFilterDTO memberFilterDTO);
    MemberDTO GetMemberByUsername(string username);
  }
}