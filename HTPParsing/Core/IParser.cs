using AngleSharp.Html.Dom;
using System.Collections.Generic;

namespace HTPParsing.Core
{
    interface IParser<T> where T: class
    {
        int GetNumberOfPages(IHtmlDocument document);
        string[] GetRedirectUrl(IHtmlDocument document);
        T Parse(IHtmlDocument document, ref List<T> enterprises);
    }
}
