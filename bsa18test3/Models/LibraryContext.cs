using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace bsa18test3.Models
{
    public class LibraryContext : DbContext
    {
        //public List<User> { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Car> Cars { get; set; }
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        { }
    }
}
