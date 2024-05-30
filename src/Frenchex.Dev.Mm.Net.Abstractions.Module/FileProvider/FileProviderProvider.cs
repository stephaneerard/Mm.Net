using Microsoft.Extensions.FileProviders;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module.FileProvider;

public class FileProviderProvider : IFileProviderProvider
{
    public IFileProvider Provide(string rootPath)
    {
        return new PhysicalFileProvider(rootPath);
    }
}