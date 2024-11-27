using Calebs.Extensions.SystemIO;

namespace Calebs.WebAPI;

public class ModelHandler
{
    IFileIO _fileIO;

    public ModelHandler(IFileIO fileIO)
    {
        _fileIO = fileIO ?? new FileIO();
    }

    public void LoadModels(WebApplication app, Options opt)
    {
        if (opt == null || !opt.ProcessModels)
        {
            return;
        }

        bool isFile = opt.ModelPath.EndsWith(".json") && _fileIO.FileExists(opt.ModelPath);
        bool isDirectory = (!isFile) && _fileIO.DirectoryExists(opt.ModelPath);

        var files = new List<string>();

        if (isDirectory)
        {
            files = (List<string>)_fileIO.GetFiles(opt.ModelPath, ".json").AsEnumerable<string>();
            isDirectory = files.Count > 0;
        }

        // We can't find anything .. bail out! 
        if (!isDirectory && !isFile )
        {
            throw new FileNotFoundException("File or directory not found or does not exist. Make sure it exists and model files are .json files");
        }

        if (isFile) { files.Add(opt.ModelPath); }

        foreach (var file in files)
        {
            var name = _fileIO.GetFileInfo(file).Name;
            var json = _fileIO.ReadAllText(file);
        }

    }

    public string GetJasonData()
    {
        string rawModel = """
            {
                person {
                    "name": "Joe Smith",
                    "age": 32,
                    "city": Mainville
                },
                movie {
                    "title": "Ferris Bueler's Day Off",
                    "publish year": 1988,
                    "id": 1
                },

            }
            """;

        return rawModel;
    }
}
