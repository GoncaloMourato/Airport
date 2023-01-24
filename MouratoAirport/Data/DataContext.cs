using MouratoAirport.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MouratoAirport.Data
{
    public class DataContext : IdentityDbContext<User>
    {

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Airplane> Airplane { get; set; }
        public DbSet<Flights> Flights { get; set; }

        public DbSet<Airpo> Airpo { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

    }
}
