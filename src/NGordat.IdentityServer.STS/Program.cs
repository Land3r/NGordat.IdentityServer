using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NGordat.Helpers.Hosting.Authentication;
using NGordat.Helpers.Hosting.Extensions;
using NGordat.Identity.Domain.Entities;
using NGordat.IdentityServer.Dal;
using NGordat.IdentityServer.Dal.Extensions;
using NGordat.IdentityServer.STS.Extensions;
using NGordat.IdentityServer.STS.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

var rootConfiguration = NGordat.IdentityServer.STS.Extensions.ConfigurationExtensions.CreateRootConfiguration(builder.Configuration);
builder.Services.AddSingleton(rootConfiguration);

// Add services to the container.

// Register the Db context used by STS and specifies the primary key type for our user system.
builder.Services.RegisterDbContexts<Guid>(builder.Configuration);

// Register local services
builder.Services.AddScoped<AccountService>();

// Register authentication and Identity Server
builder.Services.RegisterAuthentication<Guid>(builder.Configuration);

// Register HSTS options
builder.Services.RegisterHstsOptions();

// Register pages and localization
builder.Services.AddRazorWithLocalization<UserIdentity<Guid>, Guid>(builder.Configuration);

// Register authorization
builder.Services.RegisterAuthorization(builder.Configuration);

// Register Health Checks
builder.Services.AddIdentityHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, IdentityServerIdentityDbContext<Guid>, IdentityServerDataProtectionDbContext>(builder.Configuration);

var app = builder.Build();

app.UseCookiePolicy();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UsePathBase(app.Configuration.GetValue<string>("BasePath"));

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

// Add custom security headers
app.UseSecurityHeaders(app.Configuration);

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();