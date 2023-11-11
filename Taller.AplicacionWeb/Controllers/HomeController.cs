using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Taller.AplicacionWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Taller.BLL.Interfaces;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.BLL.Implementacion;
using Taller.AplicacionWeb.Utilidades.Response;
using SistemaVenta.AplicacionWeb.Models.ViewModels;

namespace Taller.AplicacionWeb.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashBoardServices _dashboardServices;
        public HomeController(ILogger<HomeController> logger, IDashBoardServices dashboardServices)
        {
            _logger = logger;
            _dashboardServices = dashboardServices;
        }

        [Authorize(Roles ="1")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {
            GenericResponse<VMDashBoard> respone = new GenericResponse<VMDashBoard>();
            
            try
            {
                VMDashBoard vmDashBoard = new VMDashBoard();
                vmDashBoard.TotalVentas = await _dashboardServices.TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await _dashboardServices.TotalIngresoUltimaSemana();
                vmDashBoard.TotalProductos = await _dashboardServices.TotalProductos();
                vmDashBoard.TotalServicios = await _dashboardServices.TotalServicios();

                List<VMVentasSemana> VentaSemana = new List<VMVentasSemana>();
                List<VMProductosSemana> ProductosSemana = new List<VMProductosSemana>();
                foreach(KeyValuePair<string,int> item in await _dashboardServices.VentasUltimaSemana())
                {
                    VentaSemana.Add(new VMVentasSemana()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                foreach (KeyValuePair<string, int> item in await _dashboardServices.ProductosTopUltimaSemana())
                {
                    ProductosSemana.Add(new VMProductosSemana()
                    {
                        Producto = item.Key,
                        Cantidad = item.Value
                    });
                }
                vmDashBoard.VentasUltimaSemana = VentaSemana;
                vmDashBoard.ProductosTopUltimaSemana=ProductosSemana;
               
                respone.Estado = true;
                respone.Objeto = vmDashBoard;
               

            }
            catch (Exception ex)
            {

                respone.Estado = false;
                respone.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, respone);

            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       
    }
}