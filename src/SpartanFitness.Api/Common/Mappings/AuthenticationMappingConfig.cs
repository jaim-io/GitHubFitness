using Mapster;

using SpartanFitness.Application.Authentication.Commands;
using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Authentication.Queries;

namespace SpartanFitness.Api.Common.Mappings;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest, src => src.User);
    }
}