using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
	public interface IClienteServices
	{
		Task<TblReparacion> TrackingNumber(string tracking);
	}
}
