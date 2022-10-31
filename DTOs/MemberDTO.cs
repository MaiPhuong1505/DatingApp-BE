using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTOs
{
    public class MemberDTO
    {
         public int Id {get; set;}
        public string Usename {get; set;}
        public string Email {get; set;}
        public int Age { get; set; }
        public string KnownAs {get; set; }
        public string Gender {get; set; }
        public string Introduction {get; set; }
        public string City {get; set; }
        public string Avatar {get; set; }
    }
}