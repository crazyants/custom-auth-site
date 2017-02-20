namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T Get<T>(this IConfiguration section, string key)
        {
            return section.GetSection(key).Get<T>();
        }
    }
}