
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace Resources.Shared.Services;

public class FileService : IFileService
{
    private readonly string _filePath;
    
    public FileService(string filePath)
    {
        _filePath = filePath;
    }
    
    public ResultResponse<string> GetFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
                return ResultResponseFactory.Empty<string>("File not found.");

            using var sr = new StreamReader(_filePath);
            var content = sr.ReadToEnd();
            return ResultResponseFactory.Success(content, "File read successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<string>(ex); }
    }
    
    public ResultResponse<bool> SaveToFile(string content)
    {
        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            using var sw = new StreamWriter(_filePath);
            sw.Write(content);
            return ResultResponseFactory.Success(true, "File saved successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<bool>(ex); }
    }
}