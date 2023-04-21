using ErrorOr;

using GitHubFitness.Application.Authentication.Common;
using GitHubFitness.Application.Common.Interfaces.Authentication;
using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.Enums;

using MediatR;

namespace GitHubFitness.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(command.Email) is not null)
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

        var roles = new HashSet<Role> { Role.User };

        await _userRepository.AddAsync(user);

        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        return new AuthenticationResult(
            user,
            token);
    }
}