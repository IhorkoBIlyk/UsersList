using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Data;
using Web.Data.Repositories;
using Web.Models;
using Web.Service;
using Web.Service.Interfaces;

namespace Web
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(option => option.EnableEndpointRouting = false);
                        
            services.AddScoped<IUnitOfWork, UnitOfWork>(servicesProvider =>
            {
                ApplicationContext context = servicesProvider.GetRequiredService<ApplicationContext>();
                UnitOfWork unitOfWork = new UnitOfWork(context);
                unitOfWork.RegisterRepositoriesFromAssembly(typeof(IGenericRepository<,>).Assembly);
                return unitOfWork;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            

            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
