using Resources.Shared.Services;

namespace ProductCatalog.Tests.Infrastructure;

    // got help from gpt with these tests
public class FileService_Tests : IDisposable
{
    private readonly string _testFilePath;
    private readonly FileService _fileService;

    public FileService_Tests()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), "test_catalog.json");
        _fileService = new FileService(_testFilePath);
    }

    [Fact]
    public void GetFromFile_FileDoesNotExist_ReturnsEmpty()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);

        // Act
        var result = _fileService.GetFromFile();

        // Assert
        Assert.True(result.Success);
        Assert.Equal("File not found.", result.Message);
        Assert.Null(result.Result);
    }

    [Fact]
    public void SaveToFile_ValidContent_SavesSuccessfully()
    {
        // Arrange
        const string testContent = "test content";

        // Act
        var result = _fileService.SaveToFile(testContent);

        // Assert
        Assert.True(result.Success);
        Assert.True(File.Exists(_testFilePath));
        Assert.Equal(testContent, File.ReadAllText(_testFilePath));
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }
}