using Mapster;

using SpartanFitness.Application.Authentication.Commands.Register;
using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Authentication.Queries.Login;
using SpartanFitness.Contracts.Authentication;

namespace SpartanFitness.Api.Common.Mappings;

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