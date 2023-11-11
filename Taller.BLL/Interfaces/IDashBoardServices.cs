using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taller.BLL.Interfaces
{
    public interface IDashBoardServices
    {
        Task<int> TotalVentasUltimaSemana();
        Task<string> TotalIngresoUltimaSemana();
        Task<int> TotalProductos();
        Task<Dictionary<string, int>> VentasUltimaSemana();
        Task<Dictionary<string, int>> ProductosTopUltimaSemana();
        Task<int> TotalServicios();
    }
}
