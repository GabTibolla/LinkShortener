using LinkShortener.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace LinkShortener.API.Controllers
{
    [ApiController]
    public class LinkShtController : ControllerBase
    {
        private readonly ConfigService _configService;
        public LinkShtController(ConfigService configService)
        {
            _configService = configService;
        }

        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] LinkShortener.Data.ShortenRecord payload)
        {
            if (String.IsNullOrEmpty(payload.url))
            {
                return BadRequest("URL cannot be null or empty.");
            }

            // Check if the URL is already shortened
            if(!payload.url.StartsWith("http://") && !payload.url.StartsWith("https://"))
            {
                payload.url = "http://" + payload.url;
            }

            try
            {
                // Create an instance of the database class
                string? className = _configService.GetValue<string>("ClassName");
                string? projectName = _configService.GetValue<string>("ProjectName");
                string? connectionString = _configService.GetValue<string>("ConnectionString");

                if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(connectionString))
                {
                    return BadRequest("Configuration values are missing.");
                }

                LinkShortener.DAO.LinkShortenerDB? db = LinkShortener.DAO.LinkShortenerDB.Create(className, projectName, connectionString);

                if (db == null)
                {
                    return StatusCode(500, "Failed to create database instance.");
                }

                
                LinkShortener.Data.LinkShortener linkShortener = new LinkShortener.Data.LinkShortener(payload.url);
                bool result = db.AddLink(linkShortener);

                if (result)
                {
                    string fullUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    return Ok(new { shortUrl = $"{fullUrl}/{linkShortener.shortUrl}", longUrl = linkShortener.longUrl });
                }

                return BadRequest("Failed to save in database.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Please try again later." + ex.Message);
            }
        }

        [HttpGet("{shortUrl}")]
        public IActionResult GetLink([FromRoute] string shortUrl)
        {
            try
            {
                // Create an instance of the database class
                string? className = _configService.GetValue<string>("ClassName");
                string? projectName = _configService.GetValue<string>("ProjectName");
                string? connectionString = _configService.GetValue<string>("ConnectionString");

                if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(connectionString))
                {
                    return BadRequest("Configuration values are missing.");
                }

                LinkShortener.DAO.LinkShortenerDB? db = LinkShortener.DAO.LinkShortenerDB.Create(className, projectName, connectionString);
                if (db == null)
                {
                    return StatusCode(500, "Failed to create database instance.");
                }

                LinkShortener.Data.LinkShortener? link = db.GetLink(shortUrl);

                if (link == null || !link.isValid())
                {
                    return NotFound("Short URL not found.");
                }

                // Atualiza os cliques
                db.AddLink(link);

                // Redirect to the long URL
                return Redirect(link.longUrl);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error ocurred while processing your request. Please try again later.");
            }
        }

        [HttpGet("links")]
        public IActionResult GetAllLinks()
        {
            try
            {
                // Create an instance of the database class
                string? className = _configService.GetValue<string>("ClassName");
                string? projectName = _configService.GetValue<string>("ProjectName");
                string? connectionString = _configService.GetValue<string>("ConnectionString");

                if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(connectionString))
                {
                    return BadRequest("Configuration values are missing.");
                }

                LinkShortener.DAO.LinkShortenerDB? db = LinkShortener.DAO.LinkShortenerDB.Create(className, projectName, connectionString);
                if (db == null)
                {
                    return StatusCode(500, "Failed to create database instance.");
                }

                List<LinkShortener.Data.LinkShortener>? links = db.GetAllLinks();

                if (links == null || links.Count() == 0)
                {
                    return NotFound("Shorts URL not found.");
                }

                // Redirect to the long URL
                return Ok(links);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error ocurred while processing your request. Please try again later.");
            }
        }
    }
}
