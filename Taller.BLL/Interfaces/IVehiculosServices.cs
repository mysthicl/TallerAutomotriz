using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
    public interface IVehiculosServices
    {
        Task<List<TblCarro>> lista();
        Task<TblCarro> Crear(TblCarro carro);
        Task<TblCarro> Editar(TblCarro carro);
        Task<bool> Eliminar(int id);
        Task<List<TblHistorialCarro>> HistorialReparacion();

        Task<List<TblHistorialCarro>> Historial(string placa);
        

    }
}
