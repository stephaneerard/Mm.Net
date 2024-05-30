﻿using System.Reflection;
using Frenchex.Dev.Mm.Net.Abstractions.Module;
using Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;
using Frenchex.Dev.Mm.Net.Abstractions.Module.Configuration;
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
    /// <param name="moduleAssemblyLoadingInformation"></param>
    /// <param name="configurationSourceBuilder"></param>
    /// <param name="fileProvider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task ConfigureConfigurationAsync(IConfigurationManager configurationManager, ModuleAssemblyLoadingInformation moduleAssemblyLoadingInformation, ConfigurationSourceBuilder configurationSourceBuilder, IFileProvider fileProvider, CancellationToken cancellationToken = default)
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
            .Path($"appsettings_{moduleName}_{moduleAssemblyLoadingInformation.HostEnvironment.EnvironmentName}.json")
            .Optional()
            .FileProvider(fileProvider)
            .Build());

        return Task.CompletedTask;
    }
}
