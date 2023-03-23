using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository
{
    public class TagRepository : ITagRepository
    {

        private readonly BloggieDbContext bloggieDbContext;
        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        async Task<Tag> ITagRepository.AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        async Task<Tag?> ITagRepository.DeleteAsync(Guid Id)
        {
            var tag = await bloggieDbContext.Tags.FindAsync(Id);
            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);
                await bloggieDbContext.SaveChangesAsync();
            }
            return tag;
        }

        async Task<IEnumerable<Tag>> ITagRepository.GetAllAsync()
        {
            return await bloggieDbContext.Tags.ToListAsync();
        }

        async Task<Tag?> ITagRepository.GetAsync(Guid Id)
        {
            return await bloggieDbContext.Tags.FirstOrDefaultAsync(t => t.Id == Id);
        }

        async Task<Tag?> ITagRepository.UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);
            if(existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await bloggieDbContext.SaveChangesAsync();
            }
            return existingTag;
        }
    }
}
