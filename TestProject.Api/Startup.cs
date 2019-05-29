using System.Reflection;

using JsonApiDotNetCore.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NJsonSchema;

using TestProject.Api.Data;

namespace TestProject.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration["ConnectionStrings:DefaultConnection"];
            string assemblyName = Assembly.GetAssembly(typeof(ApplicationDbContext)).FullName;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connection,
                    sqlServerOption => sqlServerOption.MigrationsAssembly(assemblyName)));

            services.AddJsonApi<ApplicationDbContext>();

            services.AddOpenApiDocument(settings =>
                settings.SchemaType = SchemaType.OpenApi3);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // To generate the database automatically instead of in the deploy-pipeline.
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseJsonApi();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
