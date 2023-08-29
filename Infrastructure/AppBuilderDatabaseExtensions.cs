using Infrastructure.ClassifiedAd;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Infrastructure
{
    public static class AppBuilderDatabaseExtensions
    {
        public static void EnsureDatabase(this IApplicationBuilder app)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<ClassifiedAdDbContext>();
                    context.Database.Migrate();
                }

            }
        }
    }
