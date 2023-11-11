using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taller.AplicacionWeb.Models.ViewModels;
using Taller.BLL.Interfaces;
using Taller.Entity;

namespace Taller.AplicacionWeb.Utilidades.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuServices _servicios;
        private readonly IMapper _mapper;

        public MenuViewComponent(IMenuServices servicios, IMapper mapper)
        {
            _servicios = servicios;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            List<VMMenu> listaMenu;

            if (claimUser.Identity.IsAuthenticated)
            {
                string IdUsuario = claimUser.Claims
           .Where(c => c.Type == ClaimTypes.NameIdentifier)
           .Select(c => c.Value).SingleOrDefault();

                listaMenu =_mapper.Map<List<VMMenu>>(await _servicios.ObtenerMenu(int.Parse(IdUsuario)));

            }
            else
            {
                listaMenu=new List<VMMenu> { };
            }
            return View(listaMenu);
           
        }

    }
}
