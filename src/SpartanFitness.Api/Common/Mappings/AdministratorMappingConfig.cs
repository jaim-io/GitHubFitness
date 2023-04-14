using Mapster;

using SpartanFitness.Application.Administrators.Commands;
using SpartanFitness.Contracts.Administrators;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Api.Common.Mappings;

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