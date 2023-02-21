using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using NGordat.Helpers.Hosting.Authentication;
using NGordat.Helpers.Hosting.Extensions;
using NGordat.Identity.Domain.Entities;
using NGordat.IdentityServer.Dal;
using NGordat.IdentityServer.Dal.Extensions;
using NGordat.IdentityServer.STS.Extensions;
using NGordat.IdentityServer.STS.Services;

var builder = WebApplication.CreateBuilder(args);

var rootConfiguration = NGordat.IdentityServer.STS.Extensions.ConfigurationExtensions.CreateRootConfiguration(builder.Configuration);
builder.Services.AddSingleton(rootConfiguration);

// Add services to the container.

// Register the Db context used by STS and specifies the primary key type for our user system.
builder.Services.RegisterDbContexts<Guid>(builder.Configuration);

// Register authentication and Identity Server
builder.Services.RegisterAuthentication<Guid>(builder.Configuration);

// Register pages and localization
builder.Services.AddRazorWithLocalization<UserIdentity<Guid>, Guid>(builder.Configuration);

// Register authorization
builder.Services.RegisterAuthorization(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();