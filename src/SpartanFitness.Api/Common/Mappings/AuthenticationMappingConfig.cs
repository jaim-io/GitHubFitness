using Mapster;

using SpartanFitness.Application.Authentication.Commands.ForgotPassword;
using SpartanFitness.Application.Authentication.Commands.RefreshJwtToken;
using SpartanFitness.Application.Authentication.Commands.Register;
using SpartanFitness.Application.Authentication.Commands.ResetPassword;
using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Authentication.Queries.Login;
using SpartanFitness.Contracts.Authentication;
using SpartanFitness.Domain.Common.Identity;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Api.Common.Mappings;

public class AuthenticationMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<RegisterRequest, RegisterCommand>();

    config.NewConfig<LoginRequest, LoginQuery>();

    config.NewConfig<RefreshTokenRequest, RefreshJwtTokenCommand>()
      .Map(dest => dest.RefreshTokenId, src => src.RefreshToken)
      .Map(dest => dest, src => src);

    config.NewConfig<AuthenticationResult, AuthenticationResponse>()
      .Map(dest => dest.Id, src => src.User.Id.Value.ToString())
      .Map(dest => dest.SavedExerciseIds, src => src.User.SavedExerciseIds.Select(id => id.Value.ToString()))
      .Map(dest => dest.SavedMuscleIds, src => src.User.SavedMuscleIds.Select(id => id.Value.ToString()))
      .Map(dest => dest.SavedMuscleGroupIds, src => src.User.SavedMuscleGroupIds.Select(id => id.Value.ToString()))
      .Map(dest => dest.SavedWorkoutIds, src => src.User.SavedWorkoutIds.Select(id => id.Value.ToString()))
      .Map(dest => dest.RefreshToken, src => src.RefreshToken.Id.Value.ToString())
      .Map(dest => dest, src => src.User);

    config.NewConfig<IdentityRole, AuthenticationResponseRole>()
      .Map(dest => dest.Id, src => src.RoleId.Value.ToString())
      .Map(dest => dest.Name, src => src.Name)
      .Map(dest => dest, src => src);

    config.NewConfig<ForgotPasswordRequest, ForgotPasswordCommand>()
      .Map(dest => dest, src => src);

    config.NewConfig<(string UserId, string Token), ResetPasswordCommand>()
      .Map(dest => dest.Token, src => src.Token)
      .Map(dest => dest.UserId, src => src.UserId);
  }
}