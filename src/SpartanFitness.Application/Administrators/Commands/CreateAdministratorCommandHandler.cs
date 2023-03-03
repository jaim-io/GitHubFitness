using ErrorOr;

using MediatR;

using SpartanFitness.Application.Administrators.Common;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Administrators.Commands;

public class CreateAdministratorCommandHandler
    : IRequestHandler<CreateAdministratorCommand, ErrorOr<AdministratorResult>>
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

    public async Task<ErrorOr<AdministratorResult>> Handle(CreateAdministratorCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.UserId);

        if (!await _userRepository.CheckIfExistsAsync(userId))
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

        return new AdministratorResult(admin);
    }
}