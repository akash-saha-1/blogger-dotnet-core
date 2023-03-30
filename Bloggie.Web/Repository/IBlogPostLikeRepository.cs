using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repository
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikesAsync(Guid blogPostId);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
    }
}
