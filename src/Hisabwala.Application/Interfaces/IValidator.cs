using Hisabwala.Core.Common;

public interface IValidator<T>
{
    Task<Result<bool>> ValidateAsync(T request, CancellationToken cancellationToken);
}
