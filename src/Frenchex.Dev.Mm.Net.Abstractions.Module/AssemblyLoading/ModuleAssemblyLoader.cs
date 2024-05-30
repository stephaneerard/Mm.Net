using System.Reflection;
using System.Runtime.Loader;
using Frenchex.Dev.Mm.Net.Abstractions.Module;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;

/// <summary>
/// 
/// </summary>
public class ModuleAssemblyLoader : IModuleAssemblyLoader
{
    /// <summary>
    /// </summary>
    /// <param name="moduleAssemblyInformation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"><paramref name="moduleAssemblyInformation.Path" /> is not found, or the module you are trying to load does not specify a filename extension.</exception>
    /// <exception cref="FileLoadException">A file that was found could not be loaded.
    /// -or-
    /// The ability to execute code in remote assemblies is disabled. See &lt;loadFromRemoteSources&gt;.</exception>
    /// <exception cref="BadImageFormatException"><paramref name="moduleAssemblyInformation.Path" /> is not a valid assembly; for example, a 32-bit assembly in a 64-bit process. See the exception topic for more information.
    ///  -or-
    ///  Version 2.0 or later of the common language runtime is currently loaded and <paramref name="moduleAssemblyInformation.Path" /> was compiled with a later version.</exception>
    /// <exception cref="System.Security.SecurityException">A codebase that does not start with "file://" was specified without the required <see cref="WebPermission" />.</exception>
    /// <exception cref="MethodAccessException">The caller does not have permission to call this constructor.
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="MemberAccessException" />, instead.</exception>
    /// <exception cref="TargetInvocationException">The constructor being called throws an exception.</exception>
    /// <exception cref="MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="TypeLoadException"><paramref name="type" /> is not a valid type.</exception>
    /// <exception cref="MissingMethodException">No matching public constructor was found.
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="MissingMemberException" />, instead.</exception>
    /// <exception cref="System.Runtime.InteropServices.COMException"><paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
    /// <exception cref="InvalidCastException">Thrown when loaded assembly .</exception>
    /// <exception cref="UnauthorizedAccessException">Access to <paramref name="moduleAssemblyInformation.Path" /> is denied.</exception>
    public Task<IModuleAssemblyLoadingResult> LoadAsync(ModuleAssemblyInformation moduleAssemblyInformation, CancellationToken cancellationToken = default)
    {
        var fileInfo = new FileInfo(moduleAssemblyInformation.Path);
        if (!fileInfo.Exists)
            return Task.FromResult<IModuleAssemblyLoadingResult>(new ModuleAssemblyNotFoundError()
            { ModuleAssemblyInformation = moduleAssemblyInformation });

        var alc = AssemblyLoadContext.Default;
        alc.Resolving += (_, assemblyName) => Assembly.LoadFrom(@$"{fileInfo.Directory.FullName}\{assemblyName.Name}.dll");

        var loadedAssembly = alc.LoadFromAssemblyPath(fileInfo.FullName);
        var assemblyTypeInAssembly = loadedAssembly.GetExportedTypes().FirstOrDefault(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(IModule)));

        if (assemblyTypeInAssembly is null)
            return Task.FromResult<IModuleAssemblyLoadingResult>(
                new ModuleAssemblyNotExportingExpectedModuleAssemblyInformationError()
                { ModuleAssemblyInformation = moduleAssemblyInformation });

        var assemblyTypeInAssemblyInstance = Activator.CreateInstance(assemblyTypeInAssembly);

        if (assemblyTypeInAssemblyInstance is null)
            return Task.FromResult<IModuleAssemblyLoadingResult>(new ModuleAssemblyCannotBeInstantiatedError() { ModuleAssemblyInformation = moduleAssemblyInformation });

        return Task.FromResult<IModuleAssemblyLoadingResult>(new ModuleAssemblyLoadedSuccessfully()
        {
            LoadedModule = (IModule)assemblyTypeInAssemblyInstance ?? throw new InvalidCastException(nameof(assemblyTypeInAssemblyInstance))
        });
    }
}