using Microsoft.EntityFrameworkCore;
using remitee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.Repositories
{
    public class AppRemiteeContext : DbContext
    {
        public DbSet<Quote> Quotes { get; set; }

        public AppRemiteeContext(DbContextOptions<AppRemiteeContext> options) : base(options)
        {
        }
    }
}
