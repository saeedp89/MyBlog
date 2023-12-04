namespace Blog.Frontend.Data.FileManager;

public interface IFileManager
{
    FileStream ImageStream(string image);
    Task<string> SaveImage(IFormFile image);
}