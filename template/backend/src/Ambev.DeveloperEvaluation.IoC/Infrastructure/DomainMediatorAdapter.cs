using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.IoC.Infrastructure;

public class DomainMediatorAdapter : IDomainMediator
{
    private readonly IMediator _mediator;

    public DomainMediatorAdapter(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        if (command is IRequest<TResult> request)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        throw new ArgumentException($"Command {typeof(TResult).Name} does not implement IRequest<TResult>");
    }
} 