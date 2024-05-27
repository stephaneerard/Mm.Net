namespace Frenchex.Dev.Mm.Net.Abstractions.Module;

public class ConfigurationSourceBuilder
{
    public JsonConfigurationSourceBuilder NewJsonBuilder()
    {
        return new JsonConfigurationSourceBuilder();
    }
}