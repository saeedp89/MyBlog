using Blog.Frontend.Data.FileManager;
using Blog.Frontend.Models;
using Blog.Frontend.ViewModels;

namespace Blog.Frontend.Mappers;

public static class MappersExtensions
{
    public static Post ToPost(this PostViewModel vm)
        => new()
        {
            Id = vm.Id,
            Body = vm.Body,
            Title = vm.Title,
            Category = vm.Category,
            Description = vm.Description,
            Tags = vm.Tags,
        };

    public static IEnumerable<Post> ToPost(this IEnumerable<PostViewModel> viewModels)
        => viewModels.Select(ToPost);

    public static PostViewModel ToPostViewModel(this Post post)
        => new()
        {
            Id = post.Id,
            Body = post.Body,
            Title = post.Title,
            Category = post.Category,
            Description = post.Description,
            Tags = post.Tags,
            CurrentImage = post.Image
        };

    public static IEnumerable<PostViewModel> ToPostViewModel(this IEnumerable<Post> posts)
        => posts.Select(ToPostViewModel);
}