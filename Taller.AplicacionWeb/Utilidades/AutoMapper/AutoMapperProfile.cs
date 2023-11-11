using Taller.Entity;
using System.Globalization;
using AutoMapper;
using Taller.AplicacionWeb.Models.ViewModels;

namespace Taller.AplicacionWeb.Utilidades.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            #region Usuario
            
			CreateMap<TblUsuario, VMTblUsuario>().ForMember(destino => destino.nombreRol, opt => opt.MapFrom(origen =>
            origen.IdRolNavigation.NombreRol));


			CreateMap<VMTblUsuario, TblUsuario>()
                .ForMember(destino => destino.IdRolNavigation, opt => opt.Ignore())
				.ForMember(destino => destino.FechaRegistro, opt => opt.MapFrom(origen => TimeZoneInfo.ConvertTimeFromUtc((DateTime)origen.FechaRegistro, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")).ToString("yyyy-MM-dd HH:mm:ss")));
        #endregion
      
            #region Reparacion
            CreateMap<TblReparacion, VMTblReparacion>()
                .ForMember(destino=>destino.FechaDeInicio,opt=>opt.MapFrom(origen=> origen.FechaDeInicio.GetValueOrDefault().ToShortDateString()))
                
                   .ForMember(destino => destino.usuario, opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.NombreDeUsuario))
                   .ForMember(destino => destino.TblDetalleCotizacions, opt => opt.MapFrom(origen => origen.IdCotizacionNavigation.TblDetalleCotizacions))
                   
                   .ForMember(destino => destino.Placa, opt => opt.MapFrom(origen => origen.IdHistorialCarroNavigation.IdCarroNavigation.Placa))
                   .ForMember(destino => destino.Marca, opt => opt.MapFrom(origen => origen.IdHistorialCarroNavigation.IdCarroNavigation.Marca))
                   .ForMember(destino => destino.IdCarro, opt => opt.Ignore())
                   ;

            CreateMap<VMTBLReparacionInsert, TblReparacion>()
    .ForMember(destino => destino.IdReparacion, opt => opt.MapFrom(origen => origen.IdReparacion))
    .ForMember(destino => destino.IdUsuario, opt => opt.MapFrom(origen => origen.IdUsuario))
    .ForMember(destino => destino.DescripcionDeLaReparacion, opt => opt.MapFrom(origen => origen.DescripcionDeLaReparacion))
    .ForMember(destino => destino.FechaDeInicio, opt => opt.MapFrom(origen => DateTime.Parse(origen.FechaDeInicio)))
    .ForMember(destino => destino.FechaDeFin, opt => opt.MapFrom(origen => (DateTime?)null))
    .ForMember(destino => destino.IdCotizacion, opt => opt.MapFrom(origen => origen.IdCotizacion))
    .ForMember(destino => destino.IdHistorialCarro, opt => opt.MapFrom(origen => origen.IdHistorialCarro))
    .ForMember(destino => destino.Status, opt => opt.MapFrom(origen => origen.Status))
    .ForMember(destino => destino.NumberTracking, opt => opt.MapFrom(origen => origen.NumberTracking))
    .ForMember(destino => destino.IdCotizacionNavigation, opt => opt.Ignore())
    .ForMember(destino => destino.IdHistorialCarroNavigation, opt => opt.Ignore())
    .ForMember(destino => destino.IdUsuarioNavigation, opt => opt.Ignore());


            CreateMap<TblReparacion, VMTBLReparacionInsert>()
    .ForMember(destino => destino.IdReparacion, opt => opt.MapFrom(origen => origen.IdReparacion))
    .ForMember(destino => destino.IdUsuario, opt => opt.MapFrom(origen => origen.IdUsuario))
    .ForMember(destino => destino.DescripcionDeLaReparacion, opt => opt.MapFrom(origen => origen.DescripcionDeLaReparacion))
    .ForMember(destino => destino.FechaDeInicio, opt => opt.MapFrom(origen => origen.FechaDeInicio.Value.ToString("dd/MM/yyyy")))
    .ForMember(destino => destino.IdCotizacion, opt => opt.MapFrom(origen => origen.IdCotizacion))
    .ForMember(destino => destino.IdHistorialCarro, opt => opt.MapFrom(origen => origen.IdHistorialCarro))
    .ForMember(destino => destino.Status, opt => opt.MapFrom(origen => origen.Status))
    .ForMember(destino => destino.NumberTracking, opt => opt.MapFrom(origen => origen.NumberTracking));

            CreateMap<VMTblReparacion, TblReparacion>();
            #endregion

            CreateMap<TblRol, VMTblRol>().ReverseMap();

            #region Menu
            CreateMap<Menu, VMMenu>()
                .ForMember(destino => destino.SubMenus, opt => opt.MapFrom(origen => origen.InverseIdMenuPadreNavigation));
            #endregion

            #region Producto

            CreateMap<TblProducto, VMTblProducto>()
                .ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Math.Round(origen.Precio.Value, 2).ToString("N2", new CultureInfo("es-SV"))))
                .ForMember(destino=>destino.Ganancia,opt=>opt.MapFrom(origen=>(int)origen.Ganancia.Value));

            CreateMap<VMTblProducto, TblProducto>()
                .ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-SV"))))
                .ForMember(destino => destino.Ganancia, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Ganancia, new CultureInfo("es-SV"))));
            #endregion

            #region Servicio
            CreateMap<VMServicio, TblProducto>()
                .ForMember(destino=>destino.Precio,opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-SV"))));

            CreateMap<TblProducto, VMServicio>()
                .ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Math.Round(origen.Precio.Value, 2).ToString("N2", new CultureInfo("es-SV"))));
            #endregion

            #region Vehiculo
            CreateMap<VMTblCarro, TblCarro>().ReverseMap();


            CreateMap<TblHistorialCarro, VMTblHistorialCarro>()

                   .ForMember(destino => destino.Marca, opt => opt.MapFrom(origen => origen.IdCarroNavigation.Marca));

            CreateMap<VMTblHistorialCarro, TblHistorialCarro>()
                     .ForMember(destino => destino.IdHistorialCarro, opt => opt.Ignore())
                    ;
            #endregion

            #region Venta
            CreateMap<TblVenta, VMTblVenta>()
			  .ForMember(destino => destino.Usuario, opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.NombreDeUsuario))
			  .ForMember(destino => destino.TotalDeLaVenta, opt => opt.MapFrom(origen => Convert.ToString(origen.TotalDeLaVenta.Value, new CultureInfo("en-US"))))
			  .ForMember(destino => destino.Fecha, opt => opt.MapFrom(origen => origen.Fecha.Value.ToString("dd/MM/yyyy")));

			CreateMap<VMTblVenta, TblVenta>()
				.ForMember(destino => destino.TotalDeLaVenta, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalDeLaVenta, new CultureInfo("en-US"))));
			#endregion

			#region DetalleVenta
			CreateMap<TblDetalleVenta, VMTblDetalleVenta>()
                .ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("en-US"))))
                .ForMember(destino => destino.Subtotal, opt => opt.MapFrom(origen => Convert.ToString(origen.Subtotal.Value, new CultureInfo("en-US"))))
                .ForMember(destino=>destino.DescripcionProducto,opt=>opt.MapFrom(origen=>origen.IdProductoNavigation.Descripcion))
                .ForMember(destino => destino.NombreProducto, opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre));


            CreateMap<VMTblDetalleVenta, TblDetalleVenta>()
                .ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("en-US"))))
                .ForMember(destino => destino.Subtotal, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Subtotal, new CultureInfo("en-US"))))
                .ForMember(destino=>destino.IdProductoNavigation,opt=>opt.Ignore());
            #endregion

            #region Compra
            CreateMap<TblCompra, VMTblCompra>()
				 .ForMember(destino => destino.Usuario, opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.NombreDeUsuario))
			 .ForMember(destino => destino.TotalDeLaCompra, opt => opt.MapFrom(origen => Convert.ToString(origen.TotalDeLaCompra.Value, new CultureInfo("en-US"))))
			 .ForMember(destino => destino.Fecha, opt => opt.MapFrom(origen => origen.Fecha.Value.ToString("dd/MM/yyyy")));
			
            CreateMap<VMTblCompra, TblCompra>()
				.ForMember(destino => destino.TotalDeLaCompra, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalDeLaCompra, new CultureInfo("en-US"))));

            #endregion

            #region DetalleCompra
            CreateMap<TblDetalleCompra, VMTblDetalleCompra>()
               .ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("en-US"))))
               .ForMember(destino => destino.Subtotal, opt => opt.MapFrom(origen => Convert.ToString(origen.Subtotal.Value, new CultureInfo("en-US"))))
               .ForMember(destino => destino.DescripcionProducto, opt => opt.MapFrom(origen => origen.IdProductoNavigation.Descripcion));


            CreateMap<VMTblDetalleCompra, TblDetalleCompra>()
				.ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("en-US"))))
				.ForMember(destino => destino.Subtotal, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Subtotal, new CultureInfo("en-US"))))
				
                .ForMember(destino => destino.IdProductoNavigation, opt => opt.Ignore());
			#endregion

			#region Cotizacion
			CreateMap<TblCotizacion, VMTblCotizacion>()
				 .ForMember(destino => destino.Usuario, opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.NombreDeUsuario))
			 .ForMember(destino => destino.TotalDeLaCotizacion, opt => opt.MapFrom(origen => Convert.ToString(origen.TotalDeLaCotizacion.Value, new CultureInfo("en-US"))))
			 .ForMember(destino => destino.FechaCotizacion, opt => opt.MapFrom(origen => origen.FechaCotizacion.Value.ToString("dd/MM/yyyy")));

			CreateMap<VMTblCotizacion, TblCotizacion>()
				.ForMember(destino => destino.TotalDeLaCotizacion, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalDeLaCotizacion, new CultureInfo("en-US"))));

			#endregion

			#region DetalleCotizacion

			CreateMap<TblDetalleCotizacion, VMTblDetalleCotizacion>()
			  .ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("en-US"))))
			  .ForMember(destino => destino.Subtotal, opt => opt.MapFrom(origen => Convert.ToString(origen.Subtotal.Value, new CultureInfo("en-US"))))
			  .ForMember(destino => destino.DescripcionProducto, opt => opt.MapFrom(origen => origen.IdProductoNavigation.Descripcion));


			CreateMap<VMTblDetalleCotizacion, TblDetalleCotizacion>()
				.ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("en-US"))))
				.ForMember(destino => destino.Subtotal, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Subtotal, new CultureInfo("en-US"))))

				.ForMember(destino => destino.IdProductoNavigation, opt => opt.Ignore());
			#endregion

		}


	}
}
