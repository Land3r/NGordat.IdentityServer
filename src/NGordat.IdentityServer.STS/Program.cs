using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using NGordat.Helpers.Hosting.Authentication;
using NGordat.Helpers.Hosting.Extensions;
using NGordat.Identity.Domain.Entities;
using NGordat.IdentityServer.Dal;
using NGordat.IdentityServer.Dal.Extensions;
using NGordat.IdentityServer.STS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register the Db context used by STS and specifies the primary key type for our user system.
builder.Services.RegisterDbContexts<Guid>(builder.Configuration);

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