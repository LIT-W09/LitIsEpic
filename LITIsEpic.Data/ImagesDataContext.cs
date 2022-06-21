using Microsoft.EntityFrameworkCore;

namespace LITIsEpic.Data
{
    public class ImagesDataContext : DbContext
    {
        private readonly string _connectionString;

        public ImagesDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Image> Images { get; set; }
    }
}
