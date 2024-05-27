using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module;

public class JsonConfigurationSourceBuilder
{
    private string _path;
    private bool _reloadOnChange;
    private IFileProvider? _fileProvider;
    private bool _optional;

    public JsonConfigurationSourceBuilder Path(string path)
    {
        _path = path;
        return this;
    }

    public JsonConfigurationSourceBuilder ReloadOnChange(bool reloadOnChange)
    {
        _reloadOnChange = reloadOnChange;
        return this;
    }

    public JsonConfigurationSourceBuilder FileProvider(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
        return this;
    }

    public JsonConfigurationSourceBuilder Required()
    {
        _optional = false;
        return this;
    }


    public JsonConfigurationSourceBuilder Optional()
    {
        _optional = true;
        return this;
    }

    public JsonConfigurationSource Build()
    {
        return new JsonConfigurationSource()
        {
            Path = _path,
            ReloadOnChange = _reloadOnChange,
            FileProvider = _fileProvider,
            Optional = _optional
        };
    }
}