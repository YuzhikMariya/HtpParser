namespace HTPParsing.Core.HTP
{
    class HTPSettings : IParserSettings
    {
        public HTPSettings(string baseUrl, string prefix)
        {
            BaseUrl = baseUrl;
            Prefix = prefix;
    }

        public string BaseUrl { get; set; }
        public string Prefix { get; set; } 
    }
}
