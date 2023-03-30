using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {

        private readonly BloggieDbContext bloggieDbContext;
        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public BloggieDbContext BloggieDbContext { get; }

        async Task<int> IBlogPostLikeRepository.GetTotalLikesAsync(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostsLike.CountAsync(blogPostLike => blogPostLike.BlogPostId == blogPostId);
        }

        async Task<BlogPostLike> IBlogPostLikeRepository.AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await bloggieDbContext.BlogPostsLike.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        async Task<IEnumerable<BlogPostLike>> IBlogPostLikeRepository.GetLikesForBlog(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostsLike.Where(x => x.BlogPostId.Equals(blogPostId)).ToListAsync();
        }
    }
}
