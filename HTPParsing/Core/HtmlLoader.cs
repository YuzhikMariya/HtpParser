using System.Net;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTPParsing.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;
        readonly IParserSettings sett;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            url = $"{settings.BaseUrl}/{settings.Prefix}";
            sett = settings;
        }

        public async Task<string> GetRedirectPage(string prefix)
        {
            var currentUrl = $"{sett.BaseUrl}{prefix}";
            var res = await client.GetAsync(currentUrl);
            string source = null;

            if (res != null && res.StatusCode == HttpStatusCode.OK)
            {
                source = await res.Content.ReadAsStringAsync();              
            }
            return source;
        }

        public async Task<string> GetSourcePageId(int id)
        {
            var currentUrl = url.Replace("{CurrentId}", id.ToString());           
            var res = await client.GetAsync(currentUrl);
            string source = null;

            if (res != null && res.StatusCode == HttpStatusCode.OK)
            {
                source = await res.Content.ReadAsStringAsync();               
            }

            return source;
        }
    }
}
