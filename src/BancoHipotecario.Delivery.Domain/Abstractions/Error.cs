using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Domain.Abstractions
{
    namespace BancoHipotecario.Delivery.Domain.Abstractions
    {
        public sealed class Error
        {
            public static readonly Error None = new(string.Empty, string.Empty);//representa que no hya error
            public static readonly Error NullValue = new("Error.NullValue", "Value is null");//error generico

            public string Code { get; }
            public string Message { get; }

            public Error(string code, string message)
            {
                Code = code;
                Message = message;
            }

            public override string ToString() => $"{Code}: {Message}";
        }
    }

}
