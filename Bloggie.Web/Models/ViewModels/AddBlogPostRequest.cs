using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string Urlhandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public String MinDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //display tags
        public IEnumerable<SelectListItem> Tags { get; set; }

        //Collect Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
