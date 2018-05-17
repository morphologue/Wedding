using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wedding.Models;
using Wedding.Util;

namespace Wedding
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Ef>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                // Stand up the database.
                Ef ef = serviceScope.ServiceProvider.GetRequiredService<Ef>();
                ef.Database.Migrate();

                // Add seed data if necessary.
                if (ef.Invitees.Count() == 0) {
                    ef.Invitees.AddRange(Constants.SEED_INVITEES);
                    ef.SaveChanges();
                }
            }

            app.UseStaticFiles(Constants.URL_PATH_PREFIX_FOR_ASSETS);

            app.UseMvc(routes => routes.MapRoute("default", Constants.URL_PATH_PREFIX.Substring(1) + "/{action=Index}/{controller=Home}"));
        }
    }
}
