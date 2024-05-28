using MyAcme.Net;
using Shouldly;

namespace Frenchex.Dev.Mm.Net.Tests
{
    class MyAcmeNetTests
    {
        [Test]
        public async Task Can_Instantiate()
        {
            var moduleLoader = new ModuleAssemblyLoader();
            var myAcmeNet = new MyAcmeNet(moduleLoader);

            await myAcmeNet.InitializeAsync(CancellationToken.None);

            var configuration = myAcmeNet.Configuration;

            configuration["Default"].ShouldBeEquivalentTo("Value");
            configuration["AnotherDefault"].ShouldBeEquivalentTo("AnotherValue");
        }
    }
}
