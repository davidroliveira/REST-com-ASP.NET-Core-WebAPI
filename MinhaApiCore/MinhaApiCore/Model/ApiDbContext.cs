using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MinhaApiCore.Model
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<Fornecedor> Fornecedor { get; set; }
    }
}
