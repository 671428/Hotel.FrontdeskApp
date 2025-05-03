using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace HotellAPI.Data
{
    public class HotellContext : DbContext
    {
        public HotellContext(DbContextOptions<HotellContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<SharedModels.Task> Tasks { get; set; }
    }
}
