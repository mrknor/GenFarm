using System;
using System.Collections.Generic;
using System.Linq;

namespace GenFarm.Services
{
    public class SEOOptimizationAgent
    {
        public BlogPost OptimizeBlogPost(BlogPost blogPost, string seoKeyword)
        {
            // Add meta tags, keywords, etc.
            blogPost.Metadata.SEOKeywords = seoKeyword;

            // Optional: Ensure keyword density is within acceptable range
            var keywordCount = blogPost.SubHeaders
                .Select(sh => sh.BodyText)
                .SelectMany(body => body.Split(' '))
                .Count(word => word.Equals(seoKeyword, StringComparison.OrdinalIgnoreCase));

            // Implement your desired keyword density logic here
            // For example, ensure density is between 1% and 3%

            return blogPost;
        }
    }
}
