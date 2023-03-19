# NGordat.IdentityServer.Dal

Migration project for NGordat.IdentityServer infrastructure.

## Why ?

Because this solution relies on Duende.IdentityServer, we have to inject several services into DI before being able to run migrations.

To apply the migrations to the specified database in appsettings.json :
From a terminal located where this file is :
```
dotnet NGordat.IdentityServer.Dal.Migrations --migrate
```

In order to deploy the seed data:
```
dotnet NGordat.IdentityServer.Dal.Migrations --seed
```

You can also combine both options
```
`dotnet NGordat.IdentityServer.Dal.Migrations --migrate --seed
```
Or use short options
```
dotnet NGordat.IdentityServer..Dal.Migrations -m -s
```

In order to create new migrations, please have a look at NGordat.IndentityServer.Dal