using Frenchex.Dev.Mm.Net;
using Microsoft.Extensions.Options;

namespace MyAcme.Net;

public class MyAcmeNet : ModularMonolith
{
    public MyAcmeNet(IModuleAssemblyLoader moduleAssemblyLoader) : base(moduleAssemblyLoader)
    {
    }

    /// <exception cref="InvalidConfigurationException">Thrown when cannot load a specific assembly.</exception>
    /// <exception cref="System.Security.SecurityException">The caller does not have the required permissions.</exception>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var options = Options.Create<ModularMonolithOptions>(new ModularMonolithOptions()
        {
            Assemblies = new List<ModuleAssemblyInformation>()
            {
                new ModuleAssemblyInformation()
                {
                    Path = "./../../../../MyAcme.MyModule.Net/bin/Debug/net8.0/MyAcme.MyModule.Net.dll",
                    Name = "MyAcme.MyModule.Net"
                },
                new ModuleAssemblyInformation()
                {
                    Path = "./../../../../MyAcme.MyOtherModule.Net/bin/Debug/net8.0/MyAcme.MyOtherModule.Net.dll",
                    Name = "MyAcme.MyOtherModule.Net"
                }
            }
        });

        var modules = await InternalInitializeAsync(options, cancellationToken);


    }
}