using BancoHipotecario.Delivery.Domain.Abstractions.BancoHipotecario.Delivery.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Messaging
{
    //
    public interface IQueryHandler<TQuery, TResponse>//TQuery - tipo de query a utilizar y TResponse - tipo de respuesta que devuelve la query
        : IRequestHandler<TQuery, Result<TResponse>>//hereda de IRequestHandler que es de mediatR y define que el manejador de la query recibe un TQuery y devuelve un Result<TResponse>
        where TQuery : IQuery<TResponse>// TQuery debe implementar IQuery<TResponse> para que el manejador pueda procesarlo
    {

    }
}