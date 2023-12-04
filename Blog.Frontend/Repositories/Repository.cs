using Blog.Frontend.Data;
using Blog.Frontend.Models;

namespace Blog.Frontend.Repositories;

public class Repository(AppDbContext context) : IRepository
{
    public Post GetPost(int id)
    {
        return context.Posts.First(x => x.Id == id);
    }

    public List<Post> GetAllPosts()
    {
        return context.Posts.ToList();
    }

    public List<Post> GetAllPosts(string category)
    {
        return context.Posts.Where(x => x.Category.ToLower() == category.ToLower()).ToList();
    }

    public void AddPost(Post post)
    {
        context.Posts.Add(post);
    }

    public void RemovePost(int id)
    {
        context.Posts.Remove(GetPost(id));
    }

    public void UpdatePost(Post post)
    {
        context.Posts.Update(post);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}