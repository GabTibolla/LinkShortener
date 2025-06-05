using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace LinkShortener.API.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public class UrlRequest
        {
            public string Url { get; set; }
        }

        public class UrlResponse
        {
            public string ShortUrl { get; set; }
        }

        public async Task<IActionResult> OnPostShortenUrlAsync([FromBody] UrlRequest request)
        {
            if (string.IsNullOrEmpty(request?.Url))
            {
                return BadRequest("URL inválida");
            }

            using var httpClient = new HttpClient();

            string jsonContent = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var req = HttpContext.Request;
            var baseUrl = $"{req.Scheme}://{req.Host}";
            var response = await httpClient.PostAsync($"{baseUrl}/shorten", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erro ao encurtar a URL");
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            UrlResponse urlResponse = System.Text.Json.JsonSerializer.Deserialize<UrlResponse>(responseBody, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive =true });

            return new JsonResult(urlResponse);
        }

    }
}
