
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Taller.BLL.Interfaces;
using Taller.Entity;
using Taller.DAL.Interfaz;
namespace Taller.BLL.Implementacion
{
    public class MenuServices : IMenuServices
    {
        private readonly IGenericRepository<Menu> _repositoryMenu;
        private readonly IGenericRepository<RolMenu> _repositoryRolMenu;
        private readonly IGenericRepository<TblUsuario> _repositoryUsuario;

        public MenuServices(IGenericRepository<Menu> repositoryMenu, IGenericRepository<RolMenu> repositoryRolMenu, IGenericRepository<TblUsuario> repositoryUsuario)
        {
            _repositoryMenu = repositoryMenu;
            _repositoryRolMenu = repositoryRolMenu;
            _repositoryUsuario = repositoryUsuario;
        }
        public async Task<List<Menu>> ObtenerMenu(int idUsuario)
        {
            IQueryable<TblUsuario> usuario = await _repositoryUsuario.Consultar(u=>u.IdUsuario==idUsuario);
            IQueryable<RolMenu> rolMenu = await _repositoryRolMenu.Consultar();
            IQueryable<Menu> menu = await _repositoryMenu.Consultar();

            IQueryable<Menu> MenuP = (from u in usuario
                                      join rm in rolMenu on u.IdRol equals rm.IdRol
                                      join m in menu on rm.IdMenu equals m.IdMenu
                                      join mpadre in menu on m.IdMenuPadre equals mpadre.IdMenu
                                      select mpadre
                                    ).Distinct().AsQueryable();
            IQueryable<Menu> subMenus=(from u in usuario
                                       join rm in rolMenu on u.IdRol equals rm.IdRol
                                       join m in menu on rm.IdMenu equals m.IdMenu
                                       where m.IdMenu!=m.IdMenuPadre
                                       select m).Distinct().AsQueryable();

            List<Menu> listaMenu = (from mpadre in MenuP
                                    select new Menu()
                                    {
                                        Descripcion = mpadre.Descripcion,
                                        Icono= mpadre.Icono,
                                        Controlador=mpadre.Controlador,
                                        PaginaAccion=mpadre.PaginaAccion,
                                        InverseIdMenuPadreNavigation=(from subMen in subMenus
                                                                      where subMen.IdMenuPadre==mpadre.IdMenu
                                                                      select subMen).ToList()
                                    }).ToList();

            return listaMenu;
        }
    }
}
