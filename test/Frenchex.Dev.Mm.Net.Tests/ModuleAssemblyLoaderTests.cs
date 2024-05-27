using Frenchex.Dev.Mm.Net.Abstractions.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Shouldly;

namespace Frenchex.Dev.Mm.Net.Tests;

public class Tests
{
    [Test]
    public async Task Can_Load_One_Assembly_Test()
    {
        var assemblyToLoad = new ModuleAssemblyInformation()
        {
            Path = "./../../../../MyAcme.MyModule.Net/bin/Debug/net8.0/MyAcme.MyModule.Net.dll",
            Name = "MyAcme.MyModule.Net"
        };

        var moduleAssemblyLoader = new ModuleAssemblyLoader();

        var loadingResult = await moduleAssemblyLoader.LoadAsync(assemblyToLoad);

        loadingResult.ShouldNotBeNull();
        loadingResult.ShouldBeAssignableTo<ModuleAssemblyLoadedSuccessfully>();

        var loadedAssembly = (loadingResult as ModuleAssemblyLoadedSuccessfully)!.LoadedModule;

        loadedAssembly.ShouldNotBeNull();

        var configurationManager = new ConfigurationManager();
        configurationManager.Sources.Clear();

        var moduleLoadingInformation = new ModuleLoadingInformation()
        {
            HostEnvironment = new FakeHostEnvironment()
            {
                ApplicationName = "MyAcme.dll",
                EnvironmentName = "Test",
                ContentRootPath = Path.GetFullPath(assemblyToLoad.Path)
            },
            ModuleHostPath = "./../../../../MyAcme.MyModule.Net/bin/Debug/net8.0/"
        };

        configurationManager.Sources.Count.ShouldBeEquivalentTo(0);

        var fileProvider = new PhysicalFileProvider(Path.GetFullPath(moduleLoadingInformation.ModuleHostPath));

        await loadedAssembly.ConfigureConfigurationAsync(configurationManager, moduleLoadingInformation, new ConfigurationSourceBuilder(), fileProvider);

        configurationManager.Sources.Count.ShouldBeEquivalentTo(2);
    }

    [Test]
    public async Task Can_Load_Two_Assemblies_Test()
    {
        var assemblyToLoad = new ModuleAssemblyInformation()
        {
            Path = "./../../../../MyAcme.MyModule.Net/bin/Debug/net8.0/MyAcme.MyModule.Net.dll",
            Name = "MyAcme.MyModule.Net"
        };

        var moduleAssemblyLoader = new ModuleAssemblyLoader();

        var loadingResult = await moduleAssemblyLoader.LoadAsync(assemblyToLoad);

        loadingResult.ShouldNotBeNull();
        loadingResult.ShouldBeAssignableTo<ModuleAssemblyLoadedSuccessfully>();

        var loadedAssembly = (loadingResult as ModuleAssemblyLoadedSuccessfully)!.LoadedModule;

        loadedAssembly.ShouldNotBeNull();

        var configurationManager = new ConfigurationManager();
        configurationManager.Sources.Clear();

        var moduleLoadingInformation = new ModuleLoadingInformation()
        {
            HostEnvironment = new FakeHostEnvironment()
            {
                ApplicationName = "MyAcme.dll",
                EnvironmentName = "Test",
                ContentRootPath = Path.GetFullPath(assemblyToLoad.Path)
            },
            ModuleHostPath = "./../../../../MyAcme.MyModule.Net/bin/Debug/net8.0/"
        };

        configurationManager.Sources.Count.ShouldBeEquivalentTo(0);

        var fileProvider = new PhysicalFileProvider(Path.GetFullPath(moduleLoadingInformation.ModuleHostPath));

        await loadedAssembly.ConfigureConfigurationAsync(configurationManager, moduleLoadingInformation, new ConfigurationSourceBuilder(), fileProvider);

        configurationManager.Sources.Count.ShouldBeEquivalentTo(2);

        var anotherAssemblyToLoad = new ModuleAssemblyInformation()
        {
            Path = "./../../../../MyAcme.MyOtherModule.Net/bin/Debug/net8.0/MyAcme.MyOtherModule.Net.dll",
            Name = "MyAcme.MyModule.Net"
        };

        var anotherLoadingResult = await moduleAssemblyLoader.LoadAsync(anotherAssemblyToLoad);
        anotherLoadingResult.ShouldNotBeNull();
        anotherLoadingResult.ShouldBeAssignableTo<ModuleAssemblyLoadedSuccessfully>();
        var anotherLoadedAssembly = (anotherLoadingResult as ModuleAssemblyLoadedSuccessfully)!.LoadedModule;

        var anotherModuleLoadingInformation = new ModuleLoadingInformation()
        {
            HostEnvironment = new FakeHostEnvironment()
            {
                ApplicationName = "MyAcme.dll",
                EnvironmentName = "Test",
                ContentRootPath = Path.GetFullPath(assemblyToLoad.Path)
            },
            ModuleHostPath = "./../../../../MyAcme.MyOtherModule.Net/bin/Debug/net8.0/"
        };

        var anotherFileProvider =
            new PhysicalFileProvider(Path.GetFullPath(anotherModuleLoadingInformation.ModuleHostPath));

        await anotherLoadedAssembly.ConfigureConfigurationAsync(configurationManager, anotherModuleLoadingInformation, new ConfigurationSourceBuilder(), anotherFileProvider);

        configurationManager["Default"].ShouldBeEquivalentTo("Value");
        configurationManager["AnotherDefault"].ShouldBeEquivalentTo("AnotherValue");
    }
}

public class FakeHostEnvironment : IHostEnvironment
{
    public string EnvironmentName { get; set; }
    public string ApplicationName { get; set; }
    public string ContentRootPath { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }
}