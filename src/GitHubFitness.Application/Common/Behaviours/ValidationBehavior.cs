using ErrorOr;

using FluentValidation;

using MediatR;

namespace GitHubFitness.Application.Common.Behaviours;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var errors = new List<Error>();
        foreach (var validator in _validators) {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) {
                errors.AddRange(
                    validationResult.Errors
                        .ConvertAll(validationFailure => Error.Validation(
                            validationFailure.PropertyName,
                            validationFailure.ErrorMessage)));
            }
        }

        if (!errors.Any())
        {
            return await next();
        }

        return (dynamic)errors;
    }
}