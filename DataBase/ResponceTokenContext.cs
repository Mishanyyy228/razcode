using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace lab.DataBase
{
    public class ResponceTokenContext : DbContext
    {
        public DbSet<Token> Token { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=MISHANYYY228\SQLEXPRESS; Initial Catalog=ResponceToken; User ID=sa; Password=Qwerty0690;");
        }
    }
}
