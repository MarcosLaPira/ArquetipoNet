using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Infrastructure.Exceptions
{

    public class InfraDataAccessException : Exception
    {
        public InfraDataAccessException(string message, Exception inner)
            : base(message, inner) { }
    }
}
