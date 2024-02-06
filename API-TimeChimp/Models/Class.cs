namespace Api.Devion.Models
{
    public class Config
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfigValue(string key)
        {
            return _configuration.GetValue<string>(key);
        }
    }
}
