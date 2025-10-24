using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Infrastructure.Exceptions
{
    public class InfraTimeoutException : InfraDataAccessException
    {
        public InfraTimeoutException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }
}
