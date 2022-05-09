using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.InterfacesRepositorio;
using CasosUso;
using Repositorios;

namespace ProyectoMvc
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
            services.AddControllersWithViews();
            services.AddSession();

            // PLANTAS
            services.AddScoped<IManejadorPlantas, ManejadorPlantas>();
            services.AddScoped<IRepositorioPlanta, RepositorioPlantasADO>();

            // TIPOS
            services.AddScoped<IManejadorTipos, ManejadorTipos>();
            services.AddScoped<IRepositorioTipo, RepositoriosTipoADO>();

            // USUARIOS
            services.AddScoped<IManejadorUsuarios, ManejadorUsuarios>();
            services.AddScoped<IRepositorioUsuario, RepositorioUsuariosADO>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
