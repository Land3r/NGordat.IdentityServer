# NGordat.IdentityServer.Dal

Dal project for NGordat.IdentityServer infrastructure.

## Add Migration

From a terminal located where this file is :
```
dotnet ef migrations add NameOfMigration --context IdentityServerConfigurationDbContext --output-dir Migrations/IdentityServerConfiguration
dotnet ef migrations add NameOfMigration --context IdentityServerDataProtectionDbContext --output-dir Migrations/IdentityServerDataProtection
dotnet ef migrations add NameOfMigration --context IdentityServerPersistedGrantDbContext --output-dir Migrations/IdentityServerPersistedGrant
dotnet ef migrations add NameOfMigration --context IdentityServerIdentityDbContext --output-dir Migrations/IdentityServerIdentity
```

In order to run the migration, please have a look at NGordat.IdentityServer.Dal.Migrations