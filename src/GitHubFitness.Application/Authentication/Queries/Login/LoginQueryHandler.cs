using ErrorOr;

using GitHubFitness.Application.Authentication.Common;
using GitHubFitness.Application.Common.Interfaces.Authentication;
using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;

using MediatR;

namespace GitHubFitness.Application.Authentication.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;

    public LoginQueryHandler(
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
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(query.Email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (!_passwordHasher.VerifyPassword(query.Password, user.Password, user.Salt))
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);

        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        return new AuthenticationResult(
            user,
            token);
    }
}