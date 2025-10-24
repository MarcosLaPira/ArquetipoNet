using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Infrastructure.Exceptions
{
    public class InfraSqlException : InfraDataAccessException
    {
        public InfraSqlException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }

}
