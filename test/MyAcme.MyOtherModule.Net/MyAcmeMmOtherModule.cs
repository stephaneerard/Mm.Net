using System.Reflection;
using Frenchex.Dev.Mm.Net.Abstractions.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace MyAcme.MyOtherModule.Net;
/// <summary>
/// 
/// </summary>
public class MyAcmeMmOtherModule : IModule
{
    /// <summary>
    /// 
    /// </summary>
    public Assembly Assembly => Assembly.GetAssembly(typeof(MyAcmeMmOtherModule))!;

    /// <summary>
    /// </summary>
    /// <param name="configurationManager"></param>
    /// <param name="moduleLoadingInformation"></param>
    /// <param name="configurationSourceBuilder"></param>
    /// <param name="fileProvider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task ConfigureConfigurationAsync(IConfigurationManager configurationManager, ModuleLoadingInformation moduleLoadingInformation, ConfigurationSourceBuilder configurationSourceBuilder, IFileProvider fileProvider, CancellationToken cancellationToken = default)
    {
        const string moduleName = "my_acme_other_module";

        configurationManager.Add(configurationSourceBuilder
            .NewJsonBuilder()
            .ReloadOnChange(true)
            .Path($"appsettings_{moduleName}.json")
            .Required()
            .FileProvider(fileProvider)
            .Build());

        configurationManager.Add(configurationSourceBuilder
            .NewJsonBuilder()
            .ReloadOnChange(true)
            .Path($"appsettings_{moduleName}_{moduleLoadingInformation.HostEnvironment.EnvironmentName}.json")
            .Optional()
            .FileProvider(fileProvider)
            .Build());

        return Task.CompletedTask;
    }
}
