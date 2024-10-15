using AutoMapper;
using Framework.Abstractions.Mappings;

namespace Framework.Infrastructure.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !assembly.IsDynamic &&
                               !assembly.FullName.StartsWith("System") &&
                               !assembly.FullName.StartsWith("Microsoft"));

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => Array.Exists(type.GetInterfaces(), tp => tp == typeof(IMap))).ToList();

            foreach (var type in types)
            {
                var methodInfo = type.GetMethod(nameof(IMap.Mapping));
                methodInfo?.Invoke(Activator.CreateInstance(type), new object[] { this });
            }
        }
    }
}