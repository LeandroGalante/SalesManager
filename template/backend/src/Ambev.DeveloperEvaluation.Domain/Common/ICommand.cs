namespace Ambev.DeveloperEvaluation.Domain.Common;

public interface ICommand<TResult>
{
}

public interface ICommand : ICommand<Unit>
{
}

public class Unit
{
    public static Unit Value => new();
} 