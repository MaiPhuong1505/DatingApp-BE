using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Entities;

namespace DatingApp.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository (DataContext context)
        {
            _context = context;
        }
        public void DeleteUser(User user)
        {
            _context.AppUsers.Remove(user);
        }

        public User GetUserById(int id)
        {
            return _context.AppUsers.FirstOrDefault(user => user.Id == id);
        }

        public User GetUserByUserName(string username)
        {
            return _context.AppUsers.FirstOrDefault(user => user.Usename == username);

        }

        public List<User> GetUsers()
        {
            return _context.AppUsers.ToList();
        }

        public void InsertNewUser(User user)
        {
            _context.AppUsers.Add(user);
        }

        public void UpdateUser(User user)
        {
            _context.AppUsers.Update(user);
        }
        public bool IsSaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}