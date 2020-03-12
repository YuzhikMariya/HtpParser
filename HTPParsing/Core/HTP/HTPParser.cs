using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace HTPParsing.Core.HTP
{
    class HTPParser : IParser<EnterpriseInfo>
    {
        public int GetNumberOfPages(IHtmlDocument document)
        {
            var titles = document.QuerySelector(".plist"); 
            var pages = titles.QuerySelectorAll("a");
            int lastPage = 1;
            foreach (var a in pages)
            {
                if (a.GetAttribute("title") == "Last Page")
                {
                    int.TryParse(a.TextContent, out lastPage);
                }
                
            }
            return lastPage;
        }

        public string[] GetRedirectUrl(IHtmlDocument document)
        {
            var list = new List<string>();

            var titles = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("it_enterprise_intro"));  //it_enterprise_intro
            var items = document.QuerySelectorAll("a").Where(item => item.ClassName != null && item.ClassName.Contains("morelink") && !item.GetAttribute("href").Contains("/it/projects/") );  //it_enterprise_intro

            foreach (var i in items)
            {
                var href = i.GetAttribute("href");
                list.Add(href);

            }
            return list.ToArray();
        }

        public EnterpriseInfo Parse(IHtmlDocument document, ref  List<EnterpriseInfo> incompleteInformation)
        {
            
            var list = new List<EnterpriseInfo>();
            EnterpriseInfo enterpriseInfo = new EnterpriseInfo();

            var description = document.QuerySelector(".it_enterprise_description_title");
            enterpriseInfo.description = description.QuerySelector("h1").TextContent;           

            var logoInfo = document.QuerySelector(".it_enterprise_logo_lc");
            var spans = logoInfo?.QuerySelectorAll("span").Where(item => item.ClassName != null && item.ClassName.Contains("graybold"));

            bool isNull = false;

            foreach(var s in spans)
            {
                var p = s.ParentElement;

                if (s.TextContent == "Phone:")
                {
                    if(!String.IsNullOrEmpty(p?.TextContent?.Replace(s.TextContent, "")))
                    {
                        enterpriseInfo.phone = p?.TextContent?.Replace(s.TextContent, "");
                    }
                    else
                    {
                        isNull = true;
                    }
                }
                else
                {
                    if (s.TextContent == "Website:")
                    {
                        if (!String.IsNullOrEmpty(p?.TextContent?.Replace(s.TextContent, "")))
                        {
                            enterpriseInfo.address = p?.TextContent?.Replace(s.TextContent, "");
                        }
                        else
                        {
                            isNull = true;
                        }
                    }
                }
            }

            if(isNull)
            {
                incompleteInformation.Add(enterpriseInfo);
            }

            return enterpriseInfo; 
        }
    }
}
