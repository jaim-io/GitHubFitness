using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Administrators.Commands;

public class CreateAdministratorCommandHandler
    : IRequestHandler<CreateAdministratorCommand, ErrorOr<Administrator>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAdministratorRepository _administratorRepository;
    public CreateAdministratorCommandHandler(
        IUserRepository userRepository,
        IAdministratorRepository administratorRepository)
    {
        _userRepository = userRepository;
        _administratorRepository = administratorRepository;
    }

    public async Task<ErrorOr<Administrator>> Handle(
        CreateAdministratorCommand command,
        CancellationToken cancellationToken)
    {
        var userId = UserId.Create(command.UserId);

        if (!await _userRepository.ExistsAsync(userId))
        {
            return Errors.User.NotFound;
        }

        if (await _administratorRepository.GetByUserIdAsync(userId) is Administrator)
        {
            return Errors.Administrator.DuplicateUserId;
        }
        
        var admin = Administrator.Create(
            userId);

        await _administratorRepository.AddAsync(admin);

        return admin;
    }
}