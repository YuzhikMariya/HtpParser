using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTPParsing.Core;
using HTPParsing.Core.HTP;

namespace HTPParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            ParserWorker<EnterpriseInfo> parser = new ParserWorker<EnterpriseInfo>(new HTPParser());

            parser.OnCompleted += OnCompletedEvent;

            parser.ParserSettings = new HTPSettings("http://www.park.by", "it/enterprises/?lng=en&start={CurrentId}");
            parser.Start();

            Console.Read();
        }

        static private void OnCompletedEvent(object arg1, EnterpriseInfo[] enterprises, EnterpriseInfo[] incompleteInformation)
        {
            CsvWriter.Write("test.csv", enterprises, new string[] { "Description", "Website", "Phone" });
            foreach(var err in incompleteInformation)
            {
                Console.WriteLine(err.description);
            }
            
  
        }
    }
}
