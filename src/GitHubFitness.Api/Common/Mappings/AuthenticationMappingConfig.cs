using GitHubFitness.Application.Authentication.Commands.Register;
using GitHubFitness.Application.Authentication.Common;
using GitHubFitness.Application.Authentication.Queries.Login;
using GitHubFitness.Contracts.Authentication;

using Mapster;

namespace GitHubFitness.Api.Common.Mappings;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.User.Id.Value)
            .Map(dest => dest, src => src.User);
    }
}