using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeCOntroller : ControllerBase
    {
        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogPostLikeCOntroller(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody]AddLikeRequest addLikeRequest) { 
            if(addLikeRequest != null)
            {
                var model = new BlogPostLike
                {
                    BlogPostId = addLikeRequest.BlogPostId,
                    UserId = addLikeRequest.UserId,
                };

                await blogPostLikeRepository.AddLikeForBlog(model);
            }

            return Ok();
        }

        [HttpGet]
        [Route("TotalLikes/{blogPostId:Guid}")] // here the route param key is blogPostId of type Guid
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
        {
            var totalLokes = await blogPostLikeRepository.GetTotalLikesAsync(blogPostId);
            return Ok(totalLokes);
        }

    }
}
