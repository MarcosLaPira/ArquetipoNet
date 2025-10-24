using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro
{
    public class GetClienteByFiltroQueryValidator : AbstractValidator<GetClienteByFiltroQuery>
    {
        public GetClienteByFiltroQueryValidator()
        {
            RuleFor(x => x.DNI)
                .NotEmpty().WithMessage("El DNI es requerido")
                .Length(7, 11).WithMessage("El DNI debe tener entre 7 y 10 caracteres");

            RuleFor(x => x.CanalInvocador)
                .NotEmpty().WithMessage("El canal invocador es requerido")
                .Length(1, 50).WithMessage("El canal invocador debe tener entre 1 y 50 caracteres");

            RuleFor(x => x.Entidad)
                .GreaterThan(0).When(x => x.Entidad.HasValue)
                .WithMessage("La entidad debe ser mayor a 0");
        }
    }

}
