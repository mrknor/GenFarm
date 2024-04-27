using System;
using System.Text.RegularExpressions;

namespace GenFarm.Services
{
    public class SEOKeywordAgent
    {
        public bool ValidateSEOKeyword(string seoPhrase)
        {
            // Simple validation: check for length and allowed characters
            if (string.IsNullOrWhiteSpace(seoPhrase))
            {
                throw new ArgumentException("SEO phrase cannot be empty or whitespace.");
            }

            if (seoPhrase.Length > 100)
            {
                throw new ArgumentException("SEO phrase is too long. Must be under 100 characters.");
            }

            // Optional: Check for allowed characters
            var allowedCharacters = new Regex(@"^[a-zA-Z0-9\s]+$");
            if (!allowedCharacters.IsMatch(seoPhrase))
            {
                throw new ArgumentException("SEO phrase contains invalid characters.");
            }

            return true; // If all validations pass
        }

        public string ProcessSEOKeyword(string seoPhrase)
        {
            // Additional processing if needed (e.g., trimming, converting to lowercase)
            return seoPhrase.Trim().ToLower();
        }
    }
}
