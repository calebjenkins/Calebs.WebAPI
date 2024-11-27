using CommandLine;

namespace Calebs.WebAPI;

public class Options
{
    [Option('m', "models", Required = false, HelpText = "The path to a folder or file that contains json models to be used. Each model will become a an entity with restfull endpoints to interact with.")]
    public string ModelPath { get; set; } = String.Empty;
    
    public bool ProcessModels
    {
        get { return ModelPath == string.Empty; }
    }

    [Option('d', "data", Required = false, HelpText = "url path to interact with models. Default is /data/")]
    public string DataPath { get; set; } = "/data/";

    [Option ('a', "auth", Required = false, HelpText = "When True, no bearer token needed to model updates (POST, DELETE, PATH, etc) default is False. Never needed to GETs")]
    public bool SkipAuthChecksForDataUpdates { get; set; } = false;

    // string[] is causing parser to choke. 
    //[Option("urls", Required = false, HelpText = "Used by ASP.NET to set incoming URLs, usefull when you want to specify PORT rather than it being random.")]
    //public string[] urls { get; set; } = Array.Empty<string>() ;
}
