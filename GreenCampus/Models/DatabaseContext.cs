using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace GreenCampus.Models
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<GreenActivity> GreenActivities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source=GreenCampus.db");
    }

}
