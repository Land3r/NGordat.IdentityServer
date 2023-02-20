using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.Dal.Constants
{
    public static class TableConsts
    {
        public const string IdentityRoles = "Roles";
        public const string IdentityRoleClaims = "RoleClaims";
        public const string IdentityUserRoles = "UserRoles";
        public const string IdentityUsers = "Users";
        public const string IdentityUserLogins = "UserLogins";
        public const string IdentityUserClaims = "UserClaims";
        public const string IdentityUserTokens = "UserTokens";
    }

    public static class SchemaConsts
    {
        public const string IdentitySchema = "identity";
    }
}
