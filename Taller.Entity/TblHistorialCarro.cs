using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblHistorialCarro
    {
        public TblHistorialCarro()
        {
            TblReparacions = new HashSet<TblReparacion>();
        }

        public int IdHistorialCarro { get; set; }
        public int? IdCarro { get; set; }
        public int? IdPlaca { get; set; }

        public virtual TblCarro? IdCarroNavigation { get; set; }
        public virtual ICollection<TblReparacion> TblReparacions { get; set; }
    }
}
