using BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Clientes.UnitTests
{
    public class GetClienteByFiltroQueryValidatorTests
    {
        private readonly GetClienteByFiltroQueryValidator _validator;

        public GetClienteByFiltroQueryValidatorTests()
        {
            _validator = new GetClienteByFiltroQueryValidator();
        }

        [Fact]
        public void Should_HaveError_When_DNI_IsEmpty()
        {
            var query = new GetClienteByFiltroQuery("", null, null, "CANAL");
            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.DNI)
                  .WithErrorMessage("El DNI es requerido");
        }

        [Fact]
        public void Should_HaveError_When_DNI_LengthInvalid()
        {
            var query = new GetClienteByFiltroQuery("123", null, null, "CANAL");
            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.DNI)
                  .WithErrorMessage("El DNI debe tener entre 7 y 10 caracteres");
        }

        [Fact]
        public void Should_HaveError_When_CanalInvocador_IsEmpty()
        {
            var query = new GetClienteByFiltroQuery("12345678", null, null, "");
            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.CanalInvocador)
                  .WithErrorMessage("El canal invocador es requerido");
        }

        [Fact]
        public void Should_HaveError_When_Entidad_IsNotGreaterThanZero()
        {
            var query = new GetClienteByFiltroQuery("12345678", null, 0, "CANAL");
            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.Entidad)
                  .WithErrorMessage("La entidad debe ser mayor a 0");
        }

        [Fact]
        public void Should_NotHaveErrors_When_QueryIsValid()
        {
            var query = new GetClienteByFiltroQuery("12345678", null, 10, "CANAL");
            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
