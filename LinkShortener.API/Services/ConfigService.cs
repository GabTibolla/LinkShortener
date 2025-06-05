namespace LinkShortener.API.Services
{
    public class ConfigService
    {
        private readonly IConfiguration _configuration;

        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T? GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}
