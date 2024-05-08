using Mapster;

namespace Model.Mapping;

public interface IMappingConfigurationInstaller
{
    void AddConfiguration(TypeAdapterConfig config);
}