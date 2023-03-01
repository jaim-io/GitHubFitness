using ErrorOr;

using MediatR;

using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;

namespace SpartanFitness.Application.Authentication.Commands;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleManager _roleManager;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IRoleManager roleManager)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _roleManager = roleManager;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (await _userRepository.GetByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        byte[] salt;
        var hashedPassword = _passwordHasher.HashPassword(command.Password, out salt);

        var user = User.Create(
            command.FirstName,
            command.LastName,
            command.ProfileImage,
            command.Email,
            hashedPassword,
            salt);

        _userRepository.Add(user);

        var roles = await _roleManager.GetByUserIdAsync(user.Id);

        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        return new AuthenticationResult(
            user,
            token);
    }
}