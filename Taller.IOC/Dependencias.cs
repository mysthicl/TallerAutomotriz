using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller.BLL.Implementacion;
using Taller.BLL.Interfaces;
using Taller.DAL.Implementacion;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.IOC
{
    public static class Dependencias
    {

        public static void InyectarDependencia(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<historyCarsContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("conexion"));
            });
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUsuarioServices, UsuarioServices>();

            services.AddScoped<IFireBaseServices, FireBaseServices>();

            services.AddScoped<IClienteServices, ClienteServices>();

            services.AddScoped<IEstadoVehiculoServices, EstadoVehiculoServices>();

            services.AddScoped<IRolServices, RolServices>();

            services.AddScoped<IMenuServices, MenuServices>();

            services.AddScoped<IProductoServices, ProductoServices>();

            services.AddScoped<IServicioServices, ServicioServices>();

            services.AddScoped<IVehiculosServices, VehiculosServices>();

            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddScoped<IVentaServices, VentaServices>();

            services.AddScoped<IDashBoardServices, DashBoardServices>();

            services.AddScoped<ICompraServices, CompraServices>();

			services.AddScoped<ICompraRepository, CompraRepository>();

			services.AddScoped<ICotizacionServices, CotizacionServices>();

			services.AddScoped<ICotizacionRepository, CotizacionRepository>();

			services.AddScoped<IReparacionServices, ReparacionServices>();
		}


    }
}
