using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.Dal.Factories
{
    public class IdentityServerIdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityServerIdentityDbContext>
    {
        public IdentityServerIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityServerIdentityDbContext>();

            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(nameof(IdentityServerIdentityDbContext)));

            return new IdentityServerIdentityDbContext(optionsBuilder.Options);
        }
    }
}
