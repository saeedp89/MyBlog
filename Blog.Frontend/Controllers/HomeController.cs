using Azure.Core;
using Blog.Frontend.Data;
using Blog.Frontend.Data.FileManager;
using Blog.Frontend.Models;
using Blog.Frontend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Frontend.Controllers;

public class HomeController(IRepository repository, IFileManager fileManager) : Controller
{
    public IActionResult Index(string category)
    {
        var posts = string.IsNullOrEmpty(category)
            ? repository.GetAllPosts()
            : repository.GetAllPosts(category);
        return View(posts);
    }

    public IActionResult Post(int id)
    {
        Post post = repository.GetPost(id);

        return View(post);
    }

    [HttpGet("/image/{image}")]
    public IActionResult Image(string image)
    {
        var mime = image.Substring(image.LastIndexOf('.') + 1);
        return new FileStreamResult(fileManager.ImageStream(image), $"image/{mime}");
    }
}