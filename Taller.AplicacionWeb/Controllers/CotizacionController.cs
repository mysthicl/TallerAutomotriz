using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.AplicacionWeb.Utilidades.Response;
using Taller.BLL.Interfaces;
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
	public class CotizacionController : Controller
	{
		private readonly ICotizacionServices _serviciosCotizacion;
		private readonly IMapper _mapper;
		private readonly IConverter _converter;
		private readonly historyCarsContext _dbContext;
		public CotizacionController(ICotizacionServices serviciosCotizacion, IMapper mapper, IConverter converter, historyCarsContext dbContext)
		{
			_serviciosCotizacion = serviciosCotizacion;
			_mapper = mapper;
			_converter = converter;
			_dbContext = dbContext;


		}

        [Authorize(Roles = "1,2")]
        public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerProductos(string busqueda)
		{
			List<VMTblProducto> VMProducto = _mapper.Map<List<VMTblProducto>>(await _serviciosCotizacion.ListarProductos(busqueda));

			return StatusCode(StatusCodes.Status200OK, VMProducto);

		}
		[HttpGet]
		public async Task<IActionResult> NumeroCotizacion()
		{
			string numeroCompra = await _serviciosCotizacion.ObtenerNumeroCotizacion();
			return Ok(numeroCompra);
		}


		[HttpPost]
		public async Task<IActionResult> RegistraCotizacion([FromBody] VMTblCotizacion cotizacion)
		{
			GenericResponse<VMTblCotizacion> response = new GenericResponse<VMTblCotizacion>();

			try
			{
                ClaimsPrincipal claimsUser = HttpContext.User;

                string idUsuario = claimsUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                cotizacion.IdUsuario = int.Parse(idUsuario);

				TblCotizacion cotizacionCreada = await _serviciosCotizacion.Registrar(_mapper.Map<TblCotizacion>(cotizacion));
				cotizacion = _mapper.Map<VMTblCotizacion>(cotizacionCreada);
				response.Estado = true;
				response.Objeto = cotizacion;
			}
			catch (Exception es)
			{

				response.Estado = false;
				response.Mensaje = es.Message;
			}

			return StatusCode(StatusCodes.Status200OK, response);

		}
        [HttpGet]
        public async Task<IActionResult> Historial(string numeroCotizacion, string fechaInicio, string fechaFin)
        {
            List<VMTblCotizacion> venta = _mapper.Map<List<VMTblCotizacion>>(await _serviciosCotizacion.Historial(numeroCotizacion, fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, venta);
        }

        public IActionResult HistorialCotizacion()
        {
            return View();
        }

        public async Task<IActionResult> PDFCotizacion(string numeroCotizacion, string fechaInicio, string fechaFin)
        {
            VMTblCotizacion vmVenta = _mapper.Map<VMTblCotizacion>(await _dbContext.TblCotizacions
                .Where(v => v.NumeroCotizacion == numeroCotizacion)
                .Include(v => v.TblDetalleCotizacions).ThenInclude(dv => dv.IdProductoNavigation)
                .FirstOrDefaultAsync());

            decimal subtotalTotal = 0;

            foreach (var detalleCompra in vmVenta.TblDetalleCotizacions)
            {
                subtotalTotal += Convert.ToDecimal(detalleCompra.Subtotal);
            }

            vmVenta.subtotal = subtotalTotal;
            vmVenta.iva = Math.Round(Convert.ToDouble(subtotalTotal) * 0.13, 2);

            VMPDFCotizacion modelo = new VMPDFCotizacion();
            modelo.cotizazion = vmVenta;

            return View(modelo);
        }

        public IActionResult MostrarPDFCotizacion(string numeroCotizacion)
        {
            string url = $"{this.Request.Scheme}://{this.Request.Host}/Cotizacion/PDFCotizacion?numeroCotizacion={numeroCotizacion}";
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




    }
}
