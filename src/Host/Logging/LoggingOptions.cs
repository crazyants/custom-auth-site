namespace Microsoft.Extensions.Logging
{
    public class LoggingOptions
    {
        public bool Verbose { get; set; }
        public string FileName { get; set; }
        public bool WriteToConsole { get; set; }
        public string[] Filters { get; set; }
    }
}

