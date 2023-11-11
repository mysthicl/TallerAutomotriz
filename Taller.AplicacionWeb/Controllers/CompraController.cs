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
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
	public class CompraController : Controller
	{
        private readonly ICompraServices _serviciosCompra;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private readonly historyCarsContext _dbContext;
        public CompraController(ICompraServices serviciosCompra, IMapper mapper, IConverter converter, historyCarsContext dbContext)
        {
            _serviciosCompra = serviciosCompra;
            _mapper = mapper;
            _converter = converter;
            _dbContext = dbContext;


        }

        [Authorize(Roles = "1,3")]
        public IActionResult Index()
		{
			return View();
		}

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMTblProducto> VMProducto = _mapper.Map<List<VMTblProducto>>(await _serviciosCompra.ListarProductos(busqueda));

            return StatusCode(StatusCodes.Status200OK, VMProducto);

        }
        [HttpGet]
        public async Task<IActionResult> NumeroCompra()
        {
            string numeroCompra = await _serviciosCompra.ObtenerNumeroCompra();
            return Ok(numeroCompra);
        }


        [HttpPost]
        public async Task<IActionResult> RegistrarCompra([FromBody] VMTblCompra compra)
        {
            GenericResponse<VMTblCompra> response = new GenericResponse<VMTblCompra>();

            try
            {
                ClaimsPrincipal claimsUser = HttpContext.User;

                string idUsuario = claimsUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                compra.IdUsuario = int.Parse(idUsuario);

                TblCompra venta_creada = await _serviciosCompra.Registrar(_mapper.Map<TblCompra>(compra));
                compra = _mapper.Map<VMTblCompra>(venta_creada);
                response.Estado = true;
                response.Objeto = compra;
            }
            catch (Exception es)
            {

                response.Estado = false;
                response.Mensaje = es.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);

        }

		[HttpGet]
		public async Task<IActionResult> Historial(string numeroVenta, string fechaInicio, string fechaFin)
		{
			List<VMTblCompra> venta = _mapper.Map<List<VMTblCompra>>(await _serviciosCompra.Historial(numeroVenta, fechaInicio, fechaFin));
			return StatusCode(StatusCodes.Status200OK, venta);
		}

		public IActionResult HistorialCompra()
		{
			return View();
		}

        public async Task<IActionResult> PDFCompra(string numeroCompra, string fechaInicio, string fechaFin)
        {
            VMTblCompra vmVenta = _mapper.Map<VMTblCompra>(await _dbContext.TblCompras
                .Where(v => v.NumeroCompra == numeroCompra)
                .Include(v => v.TblDetalleCompras).ThenInclude(dv => dv.IdProductoNavigation)
                .FirstOrDefaultAsync());

            decimal subtotalTotal = 0;

            foreach (var detalleCompra in vmVenta.TblDetalleCompras)
            {
                subtotalTotal += Convert.ToDecimal(detalleCompra.Subtotal);
            }

            vmVenta.subtotal = subtotalTotal;
            vmVenta.iva = Math.Round(Convert.ToDouble(subtotalTotal) * 0.13, 2);

            VMPDFCompra modelo = new VMPDFCompra();
            modelo.compra = vmVenta;

            return View(modelo);
        }

        public IActionResult MostrarPDFCompra(string numeroCompra)
        {
            string url = $"{this.Request.Scheme}://{this.Request.Host}/Compra/PDFCompra?numeroCompra={numeroCompra}";
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
