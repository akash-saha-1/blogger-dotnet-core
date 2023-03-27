using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        async Task<BlogPost> IBlogPostRepository.AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        async Task<BlogPost?> IBlogPostRepository.DeleteAsync(Guid Id)
        {
            var blogPost = await bloggieDbContext.BlogPosts.FindAsync(Id);
            if (blogPost != null)
            {
                bloggieDbContext.BlogPosts.Remove(blogPost);
                await bloggieDbContext.SaveChangesAsync();
            }
            return blogPost;
        }

        async Task<IEnumerable<BlogPost>> IBlogPostRepository.GetAllAsync()
        {
            return await bloggieDbContext.BlogPosts.Include(post => post.Tags).ToListAsync();
        }

        async Task<BlogPost?> IBlogPostRepository.GetAsync(Guid Id)
        {
            return await bloggieDbContext.BlogPosts.Include(post => post.Tags).FirstOrDefaultAsync(t => t.Id == Id);
        }

        async Task<BlogPost?> IBlogPostRepository.GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(post => post.Tags).FirstOrDefaultAsync(t => t.Urlhandle == urlHandle);
        }

        async Task<BlogPost?> IBlogPostRepository.UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await bloggieDbContext.BlogPosts.Include(blog => blog.Tags).FirstOrDefaultAsync(blog => blog.Id == blogPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Urlhandle = blogPost.Urlhandle;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.Tags = blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();
            }
            return existingBlogPost;
        }
    }
}
