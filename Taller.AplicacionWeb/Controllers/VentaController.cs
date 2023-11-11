using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using System.Data;
using System.Security.Claims;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.AplicacionWeb.Utilidades.Response;
using Taller.BLL.Interfaces;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
    public class VentaController : Controller
    {
        private readonly IVentaServices _serviciosVenta;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private readonly historyCarsContext _dbContext;
        public VentaController(IVentaServices serviciosVenta, IMapper mapper, IConverter converter,historyCarsContext dbContext)
        {
            _serviciosVenta = serviciosVenta;
            _mapper = mapper;
            _converter = converter;
            _dbContext = dbContext;
            
            
        }

        [Authorize(Roles = "1,3")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HistorialVenta()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMTblProducto> VMProducto = _mapper.Map<List<VMTblProducto>>(await _serviciosVenta.ListarProductos(busqueda));

            return StatusCode(StatusCodes.Status200OK, VMProducto);
        
        }

		[HttpGet]
		public async Task<IActionResult> NumeroVenta()
		{
			string numeroVenta = await _serviciosVenta.ObtenerNumeroVenta();
			return Ok(numeroVenta);
		}


		[HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VMTblVenta venta)
        {
            GenericResponse<VMTblVenta> response = new GenericResponse<VMTblVenta>();

            try
            {
                ClaimsPrincipal claimsUser = HttpContext.User;

                string idUsuario = claimsUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                venta.IdUsuario = int.Parse(idUsuario);
                
                TblVenta venta_creada = await _serviciosVenta.Registrar(_mapper.Map<TblVenta>(venta));
				venta = _mapper.Map<VMTblVenta>(venta_creada);
                response.Estado = true;
                response.Objeto = venta;
            }
            catch (Exception es)
            {

                response.Estado = false;
                response.Mensaje = es.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);

        }

        [HttpGet]
        public async Task<IActionResult> Historial(string numeroVenta,string fechaInicio,string fechaFin)
        {
            List<VMTblVenta> venta = _mapper.Map<List<VMTblVenta>>(await _serviciosVenta.Historial(numeroVenta, fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, venta);

        }

        public async Task<IActionResult> PDFVenta(string numeroVenta, string fechaInicio, string fechaFin)
        {
            VMTblVenta vmVenta = _mapper.Map<VMTblVenta>(await _dbContext.TblVenta
                .Where(v => v.NumeroVenta == numeroVenta)
                .Include(v => v.TblDetalleVenta).ThenInclude(dv => dv.IdProductoNavigation)
                .FirstOrDefaultAsync());

            decimal subtotalTotal = 0; 

            foreach (var detalleVenta in vmVenta.TblDetalleVenta)
            {
                subtotalTotal += Convert.ToDecimal(detalleVenta.Subtotal); 
            }

            vmVenta.subtotal = subtotalTotal; 
            vmVenta.iva =Math.Round(Convert.ToDouble(subtotalTotal)* 0.13,2);
            
            VMPDFVenta modelo = new VMPDFVenta();
            modelo.venta = vmVenta;

            return View(modelo);
        }


        public IActionResult MostrarPDFVenta(string numeroVenta)
        {
            string url = $"{this.Request.Scheme}://{this.Request.Host}/Venta/PDFVenta?numeroVenta={numeroVenta}";
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
