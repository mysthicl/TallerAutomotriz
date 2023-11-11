using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.AplicacionWeb.Utilidades.Response;
using Taller.BLL.Interfaces;
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
    public class ServicioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServicioServices _services;

        public ServicioController(IMapper mapper, IServicioServices services)
        {
            _mapper = mapper;
            _services = services;
        }

        [Authorize(Roles ="1,2")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMServicio> VMProductoLista = _mapper.Map<List<VMServicio>>(await _services.lista());

            return StatusCode(StatusCodes.Status200OK, new { data = VMProductoLista });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] string modelo)
        {
            GenericResponse<VMServicio> response = new GenericResponse<VMServicio>();
            try
            {
                string nombreFoto = "";
                Stream fotoStream = null;
                string rutaArchivo = @"C:\Users\china\OneDrive\Documentos\servicio.png";
                using (var stream = new FileStream(rutaArchivo, FileMode.Open))
                {
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    var archivo = new FormFile(
                        baseStream: memoryStream,
                        baseStreamOffset: 0,
                        length: memoryStream.Length,
                        name: "servicio.png",
                        fileName: Path.GetFileName(rutaArchivo)
                    );
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(archivo.FileName);
                    nombreFoto = string.Concat(nombre_en_codigo, extension);
                    fotoStream = archivo.OpenReadStream();
                }

                VMServicio vmProducto = JsonConvert.DeserializeObject<VMServicio>(modelo);

                TblProducto productoCreate = await _services.Crear(_mapper.Map<TblProducto>(vmProducto),fotoStream,nombreFoto);

                vmProducto = _mapper.Map<VMServicio>(productoCreate);

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
        public async Task<IActionResult> Editar([FromForm] string modelo)
        {
            GenericResponse<VMServicio> response = new GenericResponse<VMServicio>();
            try
            {
                VMServicio vmProducto = JsonConvert.DeserializeObject<VMServicio>(modelo);


                

                TblProducto productEdit = await _services.Editar(_mapper.Map<TblProducto>(vmProducto));

                vmProducto = _mapper.Map<VMServicio>(productEdit);

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


                response.Estado = await _services.Eliminar(IdProducto);


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
