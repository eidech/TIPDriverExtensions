using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPDriverExtensions;

namespace GenerateReportTester_Framework_
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            StringBuilder output = new StringBuilder();

            Config.GetPrinters(input, output);

            Console.WriteLine(output.ToString());

            Console.ReadLine();

        }
    }
}
