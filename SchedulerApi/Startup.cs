using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;

namespace TaskScheduler
{
    public class Startup
    {
        private string _connectionString = "Server=.\\mssqlserver02;Database=SchedulerSampleHangfire;User Id=sa;Password=password;";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                var assembly = Assembly.GetEntryAssembly();
                c.SwaggerDoc("v1", new Info { Title = assembly.GetName().Name, Version = "v1" });
                c.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(assembly.Location), $"{assembly.GetName().Name}.xml"));
            });

            services.AddHangfire(c =>
            {
                c.UseSqlServerStorage(_connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            GlobalConfiguration.Configuration.UseSqlServerStorage(_connectionString);
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
