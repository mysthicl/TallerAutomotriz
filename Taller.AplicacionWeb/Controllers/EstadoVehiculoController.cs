using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.AplicacionWeb.Utilidades.Response;
using AutoMapper;
using Taller.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Taller.AplicacionWeb.Controllers
{

	public class EstadoVehiculoController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IEstadoVehiculoServices _estadoVehiculoServices;

		public EstadoVehiculoController(IMapper mapper, IEstadoVehiculoServices estadoVehiculoServices)
		{
			_mapper = mapper;
			_estadoVehiculoServices = estadoVehiculoServices;
		}


		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Obtener(string numberTracking)
		{
			GenericResponse<VMTblReparacion> response = new GenericResponse<VMTblReparacion>();

			try
			{
				VMTblReparacion vMTblReparacion = _mapper.Map<VMTblReparacion>(await _estadoVehiculoServices.Obtener(numberTracking));
				response.Estado = true;
				response.Objeto = vMTblReparacion;
			}
			catch (Exception ex)
			{
				response.Estado = false;
				response.Mensaje = ex.Message;
			}
			return StatusCode(StatusCodes.Status200OK, response);

		}	
	}
}
