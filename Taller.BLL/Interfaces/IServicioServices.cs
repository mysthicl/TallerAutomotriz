using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
    public interface IServicioServices
    {
        Task<List<TblProducto>> lista();
        Task<TblProducto> Crear(TblProducto producto, Stream imagen = null, string nombreImagen = "");
        Task<TblProducto> Editar(TblProducto producto);
        Task<bool> Eliminar(int id);

    }
}
