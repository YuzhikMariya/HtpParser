using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTPParsing.Core
{
    class ParserWorker<T> where T : class
    {
        IParser<T> parser;
        IParserSettings parserSettings;
        public List<T> incompleteInformation;
        HtmlLoader loader;
        bool isActive;
        public List<T> enterpriseInfo;
        int lastPage = 1;

        public event Action<object, T[], T[]> OnCompleted;

        public IParser<T> Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }
        public IParserSettings ParserSettings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }



        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
            this.enterpriseInfo = new List<T>();
            this.incompleteInformation = new List<T>();
            
        }

        public ParserWorker(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        private async void Worker()
        {
            var startPage = await loader.GetSourcePageId(1);
            var domParser = new HtmlParser();
            var startDocument = await domParser.ParseDocumentAsync(startPage);
            lastPage = parser.GetNumberOfPages(startDocument);

            for (int i = 1; i <= lastPage; i++)
            {
                if (!isActive)
                {
                    OnCompleted?.Invoke(this, enterpriseInfo.ToArray(), incompleteInformation.ToArray());
                    return;
                }

                startPage = await loader.GetSourcePageId(i);
                domParser = new HtmlParser();
                startDocument = await domParser.ParseDocumentAsync(startPage);

               
                var redirectPrefix = parser.GetRedirectUrl(startDocument); 

                foreach (var pref in redirectPrefix)
                {
                    var redirectPage = await loader.GetRedirectPage(pref);
                    var redirectDocument = await domParser.ParseDocumentAsync(redirectPage);
                    var result = parser.Parse(redirectDocument, ref incompleteInformation);

                    enterpriseInfo.Add(result);                  
                }   
            }

            OnCompleted?.Invoke(this, enterpriseInfo.ToArray(), incompleteInformation.ToArray());
            isActive = false;
            
        }
    }
}
