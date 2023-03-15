using Mapster;

using SpartanFitness.Application.Administrators.Commands;
using SpartanFitness.Application.Administrators.Common;
using SpartanFitness.Application.Administrators.Queries.GetAdministratorById;
using SpartanFitness.Contracts.Administrators;

namespace SpartanFitness.Api.Common.Mappings;

public class AdministratorMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAdministratorRequest, CreateAdministratorCommand>();

        config.NewConfig<GetAdministratorRequest, GetAdministratorByIdQuery>();

        config.NewConfig<AdministratorResult, AdministratorResponse>()
            .Map(dest => dest.Id, src => src.Administrator.Id.Value)
            .Map(dest => dest.UserId, src => src.Administrator.UserId.Value)
            .Map(dest => dest, src => src.Administrator);
    }
}