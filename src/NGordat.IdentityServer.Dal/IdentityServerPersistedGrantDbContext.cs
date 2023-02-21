using Duende.IdentityServer.EntityFramework.DbContexts;

using Microsoft.EntityFrameworkCore;
using NGordat.IdentityServer.Dal.Interfaces;

namespace NGordat.IdentityServer.Dal
{
    public class IdentityServerPersistedGrantDbContext : PersistedGrantDbContext<IdentityServerPersistedGrantDbContext>, IAdminPersistedGrantDbContext
    {
        public IdentityServerPersistedGrantDbContext(DbContextOptions<IdentityServerPersistedGrantDbContext> options)
            : base(options)
        {
        }
    }
}