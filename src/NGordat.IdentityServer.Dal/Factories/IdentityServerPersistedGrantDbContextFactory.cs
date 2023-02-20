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
    public class IdentityServerPersistedGrantDbContextFactory : IDesignTimeDbContextFactory<IdentityServerPersistedGrantDbContext>
    {
        public IdentityServerPersistedGrantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityServerPersistedGrantDbContext>();

            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            

            //optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5433;Database=midone_gpt;User Id=postgres;Password=postgres;");
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(nameof(IdentityServerPersistedGrantDbContext)));

            return new IdentityServerPersistedGrantDbContext(optionsBuilder.Options);
        }
    }
}
