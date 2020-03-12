using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTPParsing.Core
{
    interface IParserSettings
    {
        string BaseUrl { set; get; }
        string Prefix { set; get; }
    }
}
