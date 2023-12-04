using Blog.Frontend.Data.FileManager;
using Blog.Frontend.Mappers;
using Blog.Frontend.Models;
using Blog.Frontend.Repositories;
using Blog.Frontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blog.Frontend.Controllers;

[Authorize(Roles = "Admin")]
public class PanelController(IRepository repository, IFileManager fileManager) : Controller
{
    public IActionResult Index()
    {
        var posts = repository.GetAllPosts();
        return View(posts);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new PostViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostViewModel model)
    {
        var post = await CreatePost(model);
        repository.AddPost(post);

        if (await repository.SaveChangesAsync())
            return RedirectToAction("Index");
        return View(model);
    }

    private async Task<Post> CreatePost(PostViewModel model)
    {
        var post = model.ToPost();
        if (model.Image == null)
        {
            post.Image = model.CurrentImage;
        }
        else
            post.Image = await fileManager.SaveImage(model.Image);

        return post;
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var post = repository.GetPost(id);
        var vm = post.ToPostViewModel();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PostViewModel model)
    {
        var post = await CreatePost(model);
        repository.UpdatePost(post);
        if (await repository.SaveChangesAsync())
            return RedirectToAction("Index");

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Remove(int id)
    {
        repository.RemovePost(id);
        await repository.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}