using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taller.BLL.Interfaces
{
    public interface IFireBaseServices
    {
        Task<string> SubirStorage(Stream streamArchivo, string CarpetaDestino, string NombreArchivo);

        Task<bool> EliminarStorage(string CarpetaDestino, string NombreArchivo);

    }
}
