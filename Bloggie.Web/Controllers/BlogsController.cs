using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, IBlogPostCommentRepository blogPostCommentRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        async public Task<IActionResult> Index(String urlHandle)
        {
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel { };
            bool liked = false;

            if (blogPost != null)
            {
                if (signInManager.IsSignedIn(User))
                {
                    var allLikesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);
                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var likesFromUser = allLikesForBlog.FirstOrDefault(like => like.UserId == Guid.Parse(userId));
                        liked = likesFromUser != null;
                    }
                }

                // Get comments for blog post
                var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogId(blogPost.Id);
                var blogCommentsViewModel = new List<BlogCommentViewModel>();

                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsViewModel.Add(new BlogCommentViewModel
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment?.UserId.ToString()))?.UserName
                    });
                }

                if (blogCommentsViewModel != null && blogCommentsViewModel.Any()) blogCommentsViewModel.Reverse();

                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Urlhandle = blogPost.Urlhandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = await blogPostLikeRepository.GetTotalLikesAsync(blogPost.Id),
                    Liked = liked,
                    Comments = blogCommentsViewModel
                };
            }

            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    Description = blogDetailsViewModel.commentDescription,
                    DateAdded = DateTime.Now
                };

                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.Urlhandle});
            }

            return View();
        }

    }
}
