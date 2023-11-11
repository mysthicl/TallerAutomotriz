using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.AplicacionWeb.Utilidades.Response;
using Taller.BLL.Interfaces;
using Taller.Entity;

namespace Taller.AplicacionWeb.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
	{
        private readonly IMapper _mapper;
        private readonly IUsuarioServices _usuarioServicio;
        private readonly IRolServices _rolServicio;
      

        public UsuarioController(IMapper mapper, IUsuarioServices usuarioServicio, IRolServices rolServicio, IWebHostEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _usuarioServicio = usuarioServicio;
            _rolServicio = rolServicio;
            
        }
        [Authorize(Roles = "1")]
        public IActionResult Index()
		{
			return View();
		}

        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            var lista = await _rolServicio.Lista();
            List<VMTblRol> vmListaRoles = _mapper.Map<List<VMTblRol>>(lista);
            return StatusCode(StatusCodes.Status200OK, vmListaRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            string IdUsuario = claimUser.Claims
           .Where(c => c.Type == ClaimTypes.NameIdentifier)
           .Select(c => c.Value).SingleOrDefault();

            int idUsuario = int.Parse(IdUsuario);

            var lista = await _usuarioServicio.Lista(idUsuario);
            List<VMTblUsuario> vmLista = _mapper.Map<List<VMTblUsuario>>(lista);
            return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
        }



        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<VMTblUsuario> gResponse = new GenericResponse<VMTblUsuario>();

            try
            {
                VMTblUsuario vmUsuario = JsonConvert.DeserializeObject<VMTblUsuario>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_en_codigo, extension);
                    fotoStream = foto.OpenReadStream();
                }
                else
                {
                    string rutaArchivo = @"C:\Users\china\Downloads\sinFoto.png";
                    using (var stream = new FileStream(rutaArchivo, FileMode.Open))
                    {
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);

                        var archivo = new FormFile(
                            baseStream: memoryStream,
                            baseStreamOffset: 0,
                            length: memoryStream.Length,
                            name: "sinFoto.png",
                            fileName: Path.GetFileName(rutaArchivo)
                        );
                        string nombre_en_codigo = Guid.NewGuid().ToString("N");
                        string extension = Path.GetExtension(archivo.FileName);
                        nombreFoto = string.Concat(nombre_en_codigo, extension);
                        fotoStream = archivo.OpenReadStream();
                    }
                    

                }


                TblUsuario usuarioCreado = await _usuarioServicio.Crear(_mapper.Map<TblUsuario>(vmUsuario), fotoStream, nombreFoto);

                vmUsuario = _mapper.Map<VMTblUsuario>(usuarioCreado);

                gResponse.Estado = true;
                gResponse.Objeto = vmUsuario;

            }
            catch (Exception ex)
            {

                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

		[HttpPut]
		public async Task<IActionResult> Editar([FromForm] IFormFile foto, [FromForm] string modelo)
		{
			GenericResponse<VMTblUsuario> GenericR = new GenericResponse<VMTblUsuario>();

			try
			{
				VMTblUsuario vmUsuario = JsonConvert.DeserializeObject<VMTblUsuario>(modelo);
				string nombreFoto = "";
				Stream fotoStream = null;

				if (foto != null)
				{
					string nombre_en_codigo = Guid.NewGuid().ToString("N");
					string extension = Path.GetExtension(foto.FileName);
					nombreFoto = string.Concat(nombre_en_codigo, extension);
					fotoStream = foto.OpenReadStream();
				}



				TblUsuario usuarioEditado = await _usuarioServicio.Editar(_mapper.Map<TblUsuario>(vmUsuario), fotoStream, nombreFoto);
                
				vmUsuario = _mapper.Map<VMTblUsuario>(usuarioEditado);
				GenericR.Estado = true;
				GenericR.Objeto = vmUsuario;
			}
			catch (Exception ex)
			{

				GenericR.Estado = false;
				GenericR.Mensaje = ex.Message;
			}
			return StatusCode(StatusCodes.Status200OK, GenericR);
		}

        [HttpDelete]

        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            GenericResponse<string> GenericR = new GenericResponse<string>();
            try
            {
                GenericR.Estado = await _usuarioServicio.Eliminar(idUsuario);
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
