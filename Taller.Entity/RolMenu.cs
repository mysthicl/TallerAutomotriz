using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class RolMenu
    {
        public int IdRolMenu { get; set; }
        public int? IdRol { get; set; }
        public int? IdMenu { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Menu? IdMenuNavigation { get; set; }
        public virtual TblRol? IdRolNavigation { get; set; }
    }
}
