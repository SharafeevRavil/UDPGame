using Microsoft.EntityFrameworkCore;

namespace UDPGameBackend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<CharacteristicUser> CharacteristicUsers { get; set; }
        
        
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:zxcserver.database.windows.net,1433;Initial Catalog=zxcTicTacToeDatabase;Persist Security Info=False;User ID=royaljackal;Password=Dimondim123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}