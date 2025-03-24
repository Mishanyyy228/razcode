using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoEntities;

namespace lab.Repository
{
    interface IAuthRepository
    {
        public Task<string> LoginAndGetUser(Usermodel user);

        public Task<string> RegistrUser(Usermodel user);
    }
}
