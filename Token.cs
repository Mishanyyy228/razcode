using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab
{
    public class Token
    {
        [Key]
        public string access_token { get; set; }

        public Token() { }

        public Token(string accessToken)
        {
            access_token = accessToken;
        }
    }
}
