using Microsoft.Extensions.FileProviders;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module.FileProvider;

public interface IFileProviderProvider
{
    IFileProvider Provide(string rootPath);
}