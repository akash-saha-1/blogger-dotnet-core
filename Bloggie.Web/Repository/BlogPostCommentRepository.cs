using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        async Task<BlogPostComment> IBlogPostCommentRepository.AddAsync(BlogPostComment comment)
        {
            await bloggieDbContext.BlogPostsComment.AddAsync(comment);
            await bloggieDbContext.SaveChangesAsync();
            return comment;
        }

        async Task<IEnumerable<BlogPostComment>> IBlogPostCommentRepository.GetCommentsByBlogId(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostsComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
