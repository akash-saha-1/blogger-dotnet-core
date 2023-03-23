﻿using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag?> GetAsync(Guid Id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid Id);
    }
}
