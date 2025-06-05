using System.Net.Http;

namespace LinkShortener.Data
{
    public class LinkShortener
    {
        public string? shortUrl { get; set; }
        public string? longUrl { get; set; }
        public int clicks { get; set; }

        public LinkShortener() { }

        public LinkShortener(string originalUrl)
        {
            longUrl = originalUrl;
            shortUrl = GenerateShortUrl(originalUrl);
            clicks = 0;
        }

        public bool isValid()
        {
            if (string.IsNullOrEmpty(longUrl) || string.IsNullOrEmpty(shortUrl))
                return false;

            return true;
        }

        private string GenerateShortUrl(string originalUrl )
        {
            var hash = originalUrl.GetHashCode().ToString("X");

            string currentUrl = hash.Substring(0, 6);
            return currentUrl;
        }
    }
}
