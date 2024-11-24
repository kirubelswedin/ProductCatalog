using Resources.Shared.Models;

namespace Resources.Shared.Interfaces;

public interface IFileService
{
    ResultResponse<string> GetFromFile();
    ResultResponse<bool> SaveToFile(string content);
}