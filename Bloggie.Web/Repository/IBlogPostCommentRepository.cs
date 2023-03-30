using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repository
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment comment);

        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogId(Guid blogPostId);
    }
}
