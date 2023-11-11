using Microsoft.AspNetCore.Mvc;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.BLL.Interfaces;
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteServices _clienteServices;
        public ClienteController(IClienteServices clienteServices)
        {
            _clienteServices = clienteServices;
        }
        public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Index(VMTblReparacion modelo)
        {
            TblReparacion trancking_encontrado = await _clienteServices.TrackingNumber(modelo.NumberTracking);

            if(trancking_encontrado == null)
            {
				return RedirectToAction("Error", "Cliente");
			}
            return RedirectToAction("Index", "EstadoVehiculo", new { numberTracking = modelo.NumberTracking });
        }

		public IActionResult Error()
		{
			return View();
		}
	}
}
