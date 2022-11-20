global using Microsoft.EntityFrameworkCore;
global using System.Reflection;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.Extensions.DependencyInjection;
global using Pluralize.NET;
global using System.Linq.Expressions;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Builder;
global using System.Threading;
global using Microsoft.IdentityModel.Tokens;
global using Mapster;
global using System.Linq;
global using Common.Models.Entity;
global using Repos.BaseRepositories.Contracts;
global using Domain.Entities;
global using Repos.Models;
global using Repos.Services.Contracts;
global using Common.Models;
global using Repos.Interfaces;
global using Common.Extensions;
global using Repos.Extensions;
global using Domain.Enums;








namespace Repos;
public static class ReposConfig
{
    public static async Task ReposInit(this IApplicationBuilder app)
    {
        var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var identityDbInitialize = scope.ServiceProvider.GetService<IDbInitializerService>();
            if (identityDbInitialize != null)
            {
                identityDbInitialize.Initialize();
                await identityDbInitialize.SeedDate();
            }
        }
    }
}