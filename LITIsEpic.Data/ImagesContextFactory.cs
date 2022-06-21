using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LITIsEpic.Data
{
    public class ImagesContextFactory : IDesignTimeDbContextFactory<ImagesDataContext>
    {

        public ImagesDataContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}LITIsEpic.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new ImagesDataContext(config.GetConnectionString("ConStr"));
        }

    }
}
