
using BancoHipotecario.Delivery.Domain.Abstractions.BancoHipotecario.Delivery.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Messaging
{
    
    public interface IBaseCommand
    {
        // This interface can be used to mark commands that do not return a response.
        // It can be extended in the future if needed.
    }
    public interface ICommand : IRequest<Result>, IBaseCommand
    {

    }


    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {

    }
    
    
}