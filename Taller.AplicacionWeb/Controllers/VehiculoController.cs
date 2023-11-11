using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.AplicacionWeb.Utilidades.Response;
using Taller.BLL.Interfaces;
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVehiculosServices _vehiculoServices;
        

        public VehiculoController(IMapper mapper, IVehiculosServices vehiculoServices)
        {
            _mapper = mapper;
            _vehiculoServices = vehiculoServices;
        }


        [Authorize(Roles = "1,2")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMTblCarro> VMProductoLista = _mapper.Map<List<VMTblCarro>>(await _vehiculoServices.lista());

            return StatusCode(StatusCodes.Status200OK, new { data = VMProductoLista });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] string modelo)
        {
            GenericResponse<VMTblCarro> response = new GenericResponse<VMTblCarro>();
            try
            {
                VMTblCarro vmProducto = JsonConvert.DeserializeObject <VMTblCarro>(modelo);


                TblCarro productoCreate = await _vehiculoServices.Crear(_mapper.Map<TblCarro>(vmProducto));

                vmProducto = _mapper.Map<VMTblCarro>(productoCreate);

                response.Objeto = vmProducto;
                response.Estado = true;


            }
            catch (Exception ex)
            {

                response.Estado = false;
                response.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile imagen, [FromForm] string modelo)
        {
            GenericResponse<VMTblCarro> response = new GenericResponse<VMTblCarro>();
            try
            {
                VMTblCarro vmProducto = JsonConvert.DeserializeObject<VMTblCarro>(modelo);


                Stream imagenStream = null;

                if (imagen != null)
                {
                    imagenStream = imagen.OpenReadStream();
                }

                TblCarro productEdit = await _vehiculoServices.Editar(_mapper.Map<TblCarro>(vmProducto));

                vmProducto = _mapper.Map<VMTblCarro>(productEdit);

                response.Objeto = vmProducto;
                response.Estado = true;


            }
            catch (Exception ex)
            {

                response.Estado = false;
                response.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdProducto)
        {
            GenericResponse<string> response = new GenericResponse<string>();
            try
            {

                response.Estado = await _vehiculoServices.Eliminar(IdProducto);

            }
            catch (Exception ex)
            {

                response.Estado = false;
                response.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }



        public IActionResult HistorialVehiculo()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> HistorialVehiculoLista()
        {
            List<VMTblHistorialCarro> VMProductoLista = _mapper.Map<List<VMTblHistorialCarro>>(await _vehiculoServices.HistorialReparacion());

            return StatusCode(StatusCodes.Status200OK, new { data = VMProductoLista });
        }



        [HttpGet]
        public async Task<IActionResult> Historial(string placa)
        {
            List<VMTblHistorialCarro> venta = _mapper.Map<List<VMTblHistorialCarro>>(await _vehiculoServices.Historial(placa));
            return StatusCode(StatusCodes.Status200OK, venta);

        }

       

    }
}
