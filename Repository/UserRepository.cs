using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoEntities;


namespace lab.Repository
{
    public class UserRepository
    {
        private static List<Usermodel> _users = new List<Usermodel>()
       {
           new Usermodel { Name = "Салфетка", Email = "mananev13@gmail.com", Password = "123456" },
           new Usermodel { Name = "Anna", Email = "anna@example.com", Password = "password" }
       };
        public static Usermodel CurrentUser { get; private set; }
        public Usermodel? GetUser(string email, string password)
        {
            return _users.FirstOrDefault(user => user.Email == email && user.Password == password);
        }

        public bool Register(string username, string email, string password)
        {
            if (_users.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var newUser = new Usermodel
            {
                Name = username,
                Email = email,
                Password = password
            };
            _users.Add(newUser);

            CurrentUser = newUser;
            return true;
        }
    }
}