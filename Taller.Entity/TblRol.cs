using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblRol
    {
        public TblRol()
        {
            RolMenus = new HashSet<RolMenu>();
            TblUsuarios = new HashSet<TblUsuario>();
        }

        public int IdRol { get; set; }
        public string? NombreRol { get; set; }

        public virtual ICollection<RolMenu> RolMenus { get; set; }
        public virtual ICollection<TblUsuario> TblUsuarios { get; set; }
    }
}
