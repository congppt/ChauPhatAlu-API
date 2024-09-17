using MediatR;

namespace SharedKernel.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}
//for void type response
public interface ICommand : ICommand<Unit>
{
    
}