using System;
using System.IO;

namespace HTPParsing.Core
{
    class CsvWriter
    {
        static public void Write(string path, EnterpriseInfo[] enterpriseInfoArray, string[] fields)
        {
            StreamWriter csv = new StreamWriter("../../../info.csv", false, System.Text.Encoding.GetEncoding(1251));
            if(fields.Length != 3)
            {
                csv.WriteLine("Description; Website; Phone;");
            }
            else
            {
                csv.WriteLine($"{fields[0]}; {fields[1]}; {fields[2]}");
            }
            
            foreach (var e in enterpriseInfoArray)
            {
                csv.WriteLine(e.description + ";" + e.address + ";" + e.phone);
            }
            csv.Close();
        }
        
    }
}
