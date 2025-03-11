using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.View
{
    public class Token
    {
        public string access_token { get; set; }
        public Token(string accessToken)
        {
            access_token = accessToken;
        }
    }
}
