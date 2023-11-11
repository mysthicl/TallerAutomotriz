using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taller.BLL.Interfaces;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Taller.AplicacionWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;

        public AccesoController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }


        public IActionResult Login()
        {
            ClaimsPrincipal claimsUsuario = HttpContext.User;
            if (claimsUsuario.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(VMTblUsuario modelo)
        {
            TblUsuario usuario_encontrado = await _usuarioServices.ObtenerPorCredenciales(modelo.NombreDeUsuario, modelo.Contrasena);
            
            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "Usuario o contraseña incorrecta";
                return View();
            }

            ViewData["Mensaje"] = null;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,usuario_encontrado.NombreDeUsuario),
                new Claim(ClaimTypes.NameIdentifier,usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role,usuario_encontrado.IdRol.ToString()),
                 new Claim("UrlFoto",usuario_encontrado.UrlFoto),
                 

            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            if (usuario_encontrado.IdRol==2)
            {
                return RedirectToAction("Index", "Reparacion");
            }
            else if (usuario_encontrado.IdRol==3)
            {
                return RedirectToAction("Index", "Venta");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
