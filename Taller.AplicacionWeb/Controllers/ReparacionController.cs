using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.AplicacionWeb.Utilidades.Response;
using Taller.BLL.Interfaces;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
	public class ReparacionController : Controller
	{
		private readonly IReparacionServices _serviciosReparacion;
        private readonly IGenericRepository<TblHistorialCarro> _historialCarro;
		private readonly IMapper _mapper;
		private readonly IConverter _converter;
		private readonly historyCarsContext _dbContext;
		public ReparacionController(IReparacionServices serviciosReparacion, IMapper mapper, IConverter converter, historyCarsContext dbContext,IGenericRepository<TblHistorialCarro> historialCarro)
		{
			_serviciosReparacion = serviciosReparacion;
			_mapper = mapper;
			_converter = converter;
			_dbContext = dbContext;
            _historialCarro= historialCarro;
		}

        [Authorize(Roles = "1,2")]
        public IActionResult Index()
		{
			return View();
		}

		public IActionResult HistorialReparacion()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerCotizacion(string busqueda)
		{
			List<VMTblCotizacion> VMProducto = _mapper.Map<List<VMTblCotizacion>>(await _serviciosReparacion.ListarCotizacion(busqueda));

			return StatusCode(StatusCodes.Status200OK, VMProducto);

		}



        [HttpGet]
        public async Task<IActionResult> ObtenerVehiculo(string busqueda)
        {
            List<VMTblCarro> VMProducto = _mapper.Map<List<VMTblCarro>>(await _serviciosReparacion.ListarVehiculo(busqueda));

            return StatusCode(StatusCodes.Status200OK, VMProducto);

        }

        [HttpPost]
        public async Task<IActionResult> RegistrarReparacion([FromBody] VMTBLReparacionInsert reparacion)
        {
            GenericResponse<VMTBLReparacionInsert> response = new GenericResponse<VMTBLReparacionInsert>();

            try
            {
                //SE LO TENEMOS QUE CAMBIAR POR LA COOKIES >:V EL IDUSUARIO
                ClaimsPrincipal claimsUser = HttpContext.User;

                string idUsuario = claimsUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();


                reparacion.IdUsuario = int.Parse(idUsuario);
                TblReparacion venta_creada = await _serviciosReparacion.Crear(_mapper.Map<TblReparacion>(reparacion),reparacion.IdCarro);
                reparacion = _mapper.Map<VMTBLReparacionInsert>(venta_creada);
                response.Estado = true;
                response.Objeto = reparacion;
            }
            catch (Exception es)
            {

                response.Estado = false;
                response.Mensaje = es.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);

        }

		[HttpGet]
		public async Task<IActionResult> HistorialVehiculos()
		{
			List<VMTblReparacion> VMProducto = _mapper.Map<List<VMTblReparacion>>(await _serviciosReparacion.HistorialVehiculo());

			return StatusCode(StatusCodes.Status200OK, new {data= VMProducto });

		}
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMTBLReparacionInsert reparacion)
        {
            GenericResponse<VMTBLReparacionInsert> GenericR = new GenericResponse<VMTBLReparacionInsert>();

            try
            {
               
                TblReparacion reparacionActual = await _dbContext.TblReparacions.FindAsync(reparacion.IdReparacion);

                if (reparacionActual != null)
                {
                    reparacionActual.Status = reparacion.Status;
                    
                 
                    await _dbContext.SaveChangesAsync();

                    GenericR.Estado = true;
                    GenericR.Objeto = reparacion;
                }
                else
                {
                    GenericR.Estado = false;
                    GenericR.Mensaje = "No se encontró la reparación especificada.";
                }
            }
            catch (Exception ex)
            {
                GenericR.Estado = false;
                GenericR.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, GenericR);
        }


        [HttpDelete]

        public async Task<IActionResult> Eliminar(int idHistorialCarro)
        {
            GenericResponse<string> GenericR = new GenericResponse<string>();
            try
            {
                GenericR.Estado = await _serviciosReparacion.Eliminar(idHistorialCarro);
            }
            catch (Exception e)
            {

                GenericR.Estado = false;
                GenericR.Mensaje = e.Message;
            }
            return StatusCode(StatusCodes.Status200OK, GenericR);
        }

        public async Task<IActionResult> PDFReparacion(string numberTracking)
        {
            VMTblReparacion vmVenta = _mapper.Map<VMTblReparacion>(await _dbContext.TblReparacions
                .Where(v => v.NumberTracking == numberTracking)
                .Include(v => v.IdCotizacionNavigation).ThenInclude(dv => dv.TblDetalleCotizacions).ThenInclude(dc=>dc.IdProductoNavigation)
                .FirstOrDefaultAsync());

            decimal subtotalTotal = 0;

            foreach (var detalleVenta in vmVenta.TblDetalleCotizacions)
            {
                subtotalTotal += Convert.ToDecimal(detalleVenta.Subtotal);
            }

            vmVenta.subtotal = subtotalTotal;
            vmVenta.iva = Math.Round(Convert.ToDouble(subtotalTotal) * 0.13, 2);

            VMPDFReparacion modelo = new VMPDFReparacion();
            modelo.reparacion = vmVenta;

            return View(modelo);
        }


        public IActionResult MostrarPDFReparacion(string numberTracking)
        {
            string url = $"{this.Request.Scheme}://{this.Request.Host}/Reparacion/PDFReparacion?numberTracking={numberTracking}";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page= url
                    }
                }
            };
            var archivoPDF = _converter.Convert(pdf);

            return File(archivoPDF, "application/pdf");
        }


        [HttpDelete]

        public async Task<IActionResult> EliminarHistorial(int idHistorialCarro)
        {
            GenericResponse<string> GenericR = new GenericResponse<string>();
            try
            {
                GenericR.Estado = await _serviciosReparacion.EliminarHistorialVehiculo(idHistorialCarro);
            }
            catch (Exception e)
            {

                GenericR.Estado = false;
                GenericR.Mensaje = e.Message;
            }
            return StatusCode(StatusCodes.Status200OK, GenericR);
        }

    }
}
