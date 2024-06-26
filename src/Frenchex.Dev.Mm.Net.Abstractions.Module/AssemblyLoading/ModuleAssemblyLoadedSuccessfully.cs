﻿using Frenchex.Dev.Mm.Net.Abstractions.Module;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;

/// <summary>
/// 
/// </summary>
public class ModuleAssemblyLoadedSuccessfully : IModuleAssemblyLoadingResult
{
    /// <summary>
    /// 
    /// </summary>
    public required IModule LoadedModule { get; init; }
}