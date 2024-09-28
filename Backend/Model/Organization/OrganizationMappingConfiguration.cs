using Mapster;
using Model.Mapping;

namespace Model.Organization;

public class OrganizationMappingConfiguration : IMappingConfigurationInstaller
{
    public void AddConfiguration(TypeAdapterConfig config)
    {
        config.ForType<OrganizationModel, OrganizationDto>()
            .PreserveReference(true);
    }
}