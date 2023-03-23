using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")] //optional as dotnent will be alos able to identify without this
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag { Name = addTagRequest.Name, DisplayName = addTagRequest.DisplayName };

            await tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // use dbContext to vew the data
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            //var tag = await bloggieDbContext.Tags.Find(id);
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest { Id = id, Name = tag.Name, DisplayName = tag.DisplayName };
                return View(editTagRequest);
            }

            return View(null);

        }

        [HttpPost]
        [ActionName("Edit")] //optional as dotnent will be alos able to identify without this
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag { Id = editTagRequest.Id, Name = editTagRequest.Name, DisplayName = editTagRequest.DisplayName };

            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = tag.Id });
        }

        [HttpPost]
        [ActionName("Delete")] //optional as dotnent will be alos able to identify without this
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
