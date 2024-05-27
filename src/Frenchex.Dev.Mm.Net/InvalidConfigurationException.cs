namespace Frenchex.Dev.Mm.Net;

/// <summary>
/// 
/// </summary>
public class InvalidConfigurationException : MmException
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleInformation"></param>
    public InvalidConfigurationException(ModuleAssemblyInformation moduleInformation) : base(
        $"Cannot load module {moduleInformation.Name} using path {moduleInformation.Path}.")
    {
    }
}