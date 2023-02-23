using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.Dal.Helpers
{
    public static class DbContextHelpers
    {
        public static string GetEntityTable<TDbContext>(IServiceProvider serviceProvider, string entityTypeName = null) where TDbContext : DbContext
        {
            TDbContext service = serviceProvider.GetService<TDbContext>();
            if (service != null)
            {
                IEntityType entityType = ((entityTypeName != null) ? service.Model.FindEntityType(entityTypeName) : service.Model.GetEntityTypes().FirstOrDefault());
                if (entityType != null)
                {
                    return entityType.GetTableName();
                }
            }

            return null;
        }
    }
}
