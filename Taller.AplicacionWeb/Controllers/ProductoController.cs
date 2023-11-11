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
    public class ProductoController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IProductoServices _servicesProducto;

        public ProductoController(IMapper mapper, IProductoServices servicesProducto)
        {
            _mapper = mapper;
            _servicesProducto = servicesProducto;
        }

        [Authorize(Roles = "1,3")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMTblProducto> VMProductoLista = _mapper.Map<List<VMTblProducto>>(await _servicesProducto.lista());

            return StatusCode(StatusCodes.Status200OK, new { data = VMProductoLista });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile imagen, [FromForm] string modelo)
        {
            GenericResponse<VMTblProducto> response = new GenericResponse<VMTblProducto>();
            try
            {
                VMTblProducto vmProducto = JsonConvert.DeserializeObject<VMTblProducto>(modelo);

                string imagenNombre = "";
                Stream imagenStream = null;

                if (imagen != null)
                {
                    string nombreAleatorio=Guid.NewGuid().ToString("N");
                    string extension = imagen.FileName;
                    imagenNombre = string.Concat(nombreAleatorio,extension);
                    imagenStream = imagen.OpenReadStream();
                }

                TblProducto productoCreate = await _servicesProducto.Crear(_mapper.Map<TblProducto>(vmProducto),imagenStream,imagenNombre);

                vmProducto = _mapper.Map<VMTblProducto>(productoCreate);

                response.Objeto = vmProducto;
                response.Estado= true;


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
            GenericResponse<VMTblProducto> response = new GenericResponse<VMTblProducto>();
            try
            {
                VMTblProducto vmProducto = JsonConvert.DeserializeObject<VMTblProducto>(modelo);

                
                Stream imagenStream = null;

                if (imagen != null)
                {
                    imagenStream = imagen.OpenReadStream();
                }

                TblProducto productEdit = await _servicesProducto.Editar(_mapper.Map<TblProducto>(vmProducto), imagenStream);

                vmProducto = _mapper.Map<VMTblProducto>(productEdit);

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
                
                
                response.Estado = await _servicesProducto.Eliminar(IdProducto);


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
