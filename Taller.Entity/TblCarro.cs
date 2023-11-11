using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblCarro
    {
        public TblCarro()
        {
            TblHistorialCarros = new HashSet<TblHistorialCarro>();
        }

        public string Placa { get; set; } = null!;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? Ano { get; set; }
        public int IdCarro { get; set; }

        public virtual ICollection<TblHistorialCarro> TblHistorialCarros { get; set; }
    }
}
