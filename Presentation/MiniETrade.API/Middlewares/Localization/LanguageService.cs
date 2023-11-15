using Microsoft.Extensions.Localization;
using MiniETrade.Application.Common.Abstractions.Localization;
using System.Reflection;

namespace MiniETrade.API.Middlewares.Localization;

public class LanguageService : ILanguageService
{
    private readonly IStringLocalizer _localizer;
    public LanguageService(IStringLocalizerFactory factory)
    {
        var type = typeof(SharedResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        _localizer = factory.Create(nameof(SharedResource), assemblyName.Name);
    }

    public string GetKey(string key)
    {
        return _localizer.GetString(key).Value;
    }
}
// Dummy class to group shared resources
public class SharedResource
{}