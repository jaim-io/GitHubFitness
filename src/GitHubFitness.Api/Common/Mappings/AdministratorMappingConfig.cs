using GitHubFitness.Application.Administrators.Commands;
using GitHubFitness.Contracts.Administrators;
using GitHubFitness.Domain.Aggregates;

using Mapster;

namespace GitHubFitness.Api.Common.Mappings;

public class AdministratorMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAdministratorRequest, CreateAdministratorCommand>();

        config.NewConfig<Administrator, AdministratorResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest, src => src);
    }
}