using System.Reflection;
using Mapster;

namespace Model.Mapping;

public static class MappingConfiguration
{
    public static TypeAdapterConfig Get()
    {
        return GetFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static TypeAdapterConfig GetFromAssembliesContaining(params Type[] markers)
    {
        Assembly[] assemblies = markers.Select(x => x.Assembly).ToArray();
        return GetFromAssembly(assemblies);
    }

    public static TypeAdapterConfig GetFromAssembly(params Assembly[] assemblies)
    {
        TypeAdapterConfig config = new();

        foreach (Assembly assembly in assemblies)
        {
            IEnumerable<TypeInfo> installerTypes = assembly.DefinedTypes.Where(x =>
                typeof(IMappingConfigurationInstaller).IsAssignableFrom(x)
                && !x.IsInterface && !x.IsAbstract);

            IEnumerable<IMappingConfigurationInstaller> installers = installerTypes.Select(Activator.CreateInstance)
                .Cast<IMappingConfigurationInstaller>();

            foreach (IMappingConfigurationInstaller? installer in installers) installer.AddConfiguration(config);
        }

        return config;
    }
}