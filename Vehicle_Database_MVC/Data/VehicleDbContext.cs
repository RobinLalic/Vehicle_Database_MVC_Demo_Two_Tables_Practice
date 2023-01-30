using Microsoft.EntityFrameworkCore;
using Vehicle_Database_MVC.Models.Domain;

namespace Vehicle_Database_MVC.Data
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<VehicleMake> Makes { get; set; }
        public DbSet<VehicleModel> Models { get; set; }
    }
}
