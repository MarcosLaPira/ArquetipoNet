using BancoHipotecario.Delivery.Domain.Abstractions.BancoHipotecario.Delivery.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Messaging
{
    //lo que hace esto es definir que toda quiery debe devolver el objeto que pidio englobado en un result
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {

    }
}