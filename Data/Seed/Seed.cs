using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingApp.API.Data.Entities;

namespace DatingApp.API.Data.Seed
{
    public class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if (context.AppUsers.Any()) return;
            var userFile = System.IO.File.ReadAllText("Data/Seed/users.json");
            var users = JsonSerializer.Deserialize<List<User>>(userFile);
            if (users == null) return;
            foreach (var user in users)
            {
                user.CreateAt = DateTime.Now;
                using var hmac = new HMACSHA512();

                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));
                user.PasswordSalt = hmac.Key;
                context.AppUsers.Add(user);
            }
            context.SaveChanges();
        }
    }
}