using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Entities;

namespace DatingApp.API.Services
{
    public interface iTokenService
    {
        string CreateToken(string username);
    }
}