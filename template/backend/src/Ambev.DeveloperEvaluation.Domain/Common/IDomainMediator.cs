namespace Ambev.DeveloperEvaluation.Domain.Common;

public interface IDomainMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
} 