using System.Collections.Generic;
using System.Linq;

namespace GenFarm.Services
{
    public class BlogPostService
    {
        private readonly List<BlogPost> _blogPosts = new List<BlogPost>();

        public List<BlogPost> GetAllBlogPosts()
        {
            return _blogPosts;
        }

        public BlogPost GetBlogPostById(int id)
        {
            return _blogPosts.FirstOrDefault(bp => bp.GetHashCode() == id); // Example ID implementation
        }

        public void AddBlogPost(BlogPost blogPost)
        {
            _blogPosts.Add(blogPost);
        }
    }
}
