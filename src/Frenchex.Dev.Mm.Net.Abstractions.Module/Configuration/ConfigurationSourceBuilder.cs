namespace Frenchex.Dev.Mm.Net.Abstractions.Module.Configuration;

public class ConfigurationSourceBuilder
{
    public JsonConfigurationSourceBuilder NewJsonBuilder()
    {
        return new JsonConfigurationSourceBuilder();
    }
}