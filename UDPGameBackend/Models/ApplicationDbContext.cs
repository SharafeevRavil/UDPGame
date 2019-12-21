using Microsoft.EntityFrameworkCore;

namespace UDPGameBackend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Characteristic> CharacteristicsUsers { get; set; }
        public DbSet<CharacteristicUser> CharacteristicUsers { get; set; }
    }
}