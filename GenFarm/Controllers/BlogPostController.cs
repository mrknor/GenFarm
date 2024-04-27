using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using GenFarm.Services;

namespace GenFarm.Controllers
{
    [ApiController]
    [Route("api/blogpost")]
    public class BlogPostController : ControllerBase
    {
        private readonly BlogPostService _blogPostService;

        public BlogPostController(BlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet("all")]
        public IActionResult GetAllBlogPosts()
        {
            var blogPosts = _blogPostService.GetAllBlogPosts();

            if (blogPosts == null || blogPosts.Count == 0)
            {
                return NotFound("No blog posts found.");
            }

            return Ok(blogPosts);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlogPostById(int id)
        {
            var blogPost = _blogPostService.GetBlogPostById(id);

            if (blogPost == null)
            {
                return NotFound($"Blog post with ID {id} not found.");
            }

            return Ok(blogPost);
        }
    }
}
