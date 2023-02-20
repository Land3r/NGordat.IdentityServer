# NGordat.IdentityServer.Dal

Dal project for NGordat.IdentityServer infrastructure.

## Evolve model

In order to evolve model, you have to add/remove/edit properties from the entities.
Add an ef migration to the corresponding dbcontext

```
dotnet ef migrations add NameOfMigration --context IdentityServerConfigurationDbContext --output-dir Migrations/IdentityServerConfiguration
dotnet ef migrations add NameOfMigration --context IdentityServerDataProtectionDbContext --output-dir Migrations/IdentityServerDataProtection
dotnet ef migrations add NameOfMigration --context IdentityServerPersistedGrantDbContext --output-dir Migrations/IdentityServerPersistedGrant
```