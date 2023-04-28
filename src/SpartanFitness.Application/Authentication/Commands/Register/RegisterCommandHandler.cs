using ErrorOr;

using MediatR;

using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
  : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IPasswordHasher _passwordHasher;
  private readonly IRoleRepository _roleRepository;
  private readonly IRefreshTokenRepository _refreshTokenRepository;

  public RegisterCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IRefreshTokenRepository refreshTokenRepository)
  {
    _userRepository = userRepository;
    _jwtTokenGenerator = jwtTokenGenerator;
    _passwordHasher = passwordHasher;
    _roleRepository = roleRepository;
    _refreshTokenRepository = refreshTokenRepository;
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

    var (token, refreshToken) = _jwtTokenGenerator.GenerateToken(user, roles);

    await _refreshTokenRepository.AddAsync(refreshToken);

    return new AuthenticationResult(
      user,
      token,
      refreshToken);
  }
}