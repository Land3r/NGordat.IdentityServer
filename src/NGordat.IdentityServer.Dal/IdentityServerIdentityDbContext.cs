using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NGordat.Identity.Domain.Entities;
using NGordat.IdentityServer.Dal.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.Dal
{
    public class IdentityServerIdentityDbContext<TKey> : IdentityDbContext<UserIdentity<TKey>, UserIdentityRole<TKey>, TKey, UserIdentityUserClaim<TKey>, UserIdentityUserRole<TKey>, UserIdentityUserLogin<TKey>, UserIdentityRoleClaim<TKey>, UserIdentityUserToken<TKey>>
        where TKey : IEquatable<TKey>
    {
        public IdentityServerIdentityDbContext(DbContextOptions<IdentityServerIdentityDbContext<TKey>> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentityContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.HasDefaultSchema(SchemaConsts.IdentitySchema);

            builder.Entity<UserIdentityRole<TKey>>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<UserIdentityRoleClaim<TKey>>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<UserIdentityUserRole<TKey>>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<UserIdentity<TKey>>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<UserIdentityUserLogin<TKey>>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<UserIdentityUserClaim<TKey>>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<UserIdentityUserToken<TKey>>().ToTable(TableConsts.IdentityUserTokens);
        }
    }
}
