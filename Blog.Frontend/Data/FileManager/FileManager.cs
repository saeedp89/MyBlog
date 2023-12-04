namespace Blog.Frontend.Data.FileManager;

public class FileManager(IConfiguration configuration) : IFileManager
{
    private readonly string? _imagePath = configuration["Path:Images"];

    public FileStream ImageStream(string image)
    {
        return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
    }

    public async Task<string> SaveImage(IFormFile image)
    {
        try
        {
            var savePath = Path.Combine(_imagePath);
            if (Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));

            var fileName = $"img{DateTime.Now:dd-MM-yyyy-HH-mm-ss}{mime}";

            await using var fileStream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create);
            await image.CopyToAsync(fileStream);

            return fileName;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "Error";
        }
    }
}