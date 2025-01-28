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
           new Usermodel { Username = "Салфетка", Email = "mananev13@gmail.com", Password = "123456" },
           new Usermodel { Username = "Anna", Email = "anna@example.com", Password = "password" }
       };

        public Usermodel? GetUser(string email, string password)
        {
            foreach (var user in _users)
            {
                if (user.Email == email && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }
        public bool Register(string username, string email, string password)
        {
            if (_users.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            _users.Add(new Usermodel { Username = username, Email = email, Password = password });
            return true;
        }
    }

}
