using Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;
using Frenchex.Dev.Mm.Net.Abstractions.Module.FileProvider;
using MyAcme.Net;
using Shouldly;

namespace Frenchex.Dev.Mm.Net.Tests
{
    class MyAcmeNetTests
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidConfigurationException">Thrown when cannot load a specific assembly.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permissions.</exception>
        [Test]
        public async Task Can_Instantiate()
        {
            var moduleLoader = new ModuleAssemblyLoader();
            var fileProviderProvider = new FileProviderProvider();
            var myAcmeNet = new MyAcmeNet(moduleLoader, fileProviderProvider);

            await myAcmeNet.InitializeAsync(CancellationToken.None);

            var configuration = myAcmeNet.Configuration;

            configuration["Default"].ShouldBeEquivalentTo("Value");
            configuration["AnotherDefault"].ShouldBeEquivalentTo("AnotherValue");
        }
    }
}
