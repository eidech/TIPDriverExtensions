using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TIPDriverExtensions;

namespace ScreenshotTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //Holders for input and outputs
            string input;
            StringBuilder output = new StringBuilder();

            //Set up input
            string directory = Directory.GetCurrentDirectory() + @"\..\..\screenshots\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            //Config startup expects 250 char path for reports, database, and screenshot
            //Only screenshot matters for tester, so just provide same path 3x
            //Dbpath expects fully qualified path to mdb file; dummy mdb provided for tester
            string databasePath = directory + "db.mdb";
            databasePath = databasePath.PadRight(250);
            directory = directory.PadRight(250);

            input = "Terminal".PadRight(10) + directory + databasePath + directory;

            //Start up the toolkit
            Config.StartUp(input, output);
            Console.WriteLine(output.ToString());
            if (output.ToString().Substring(0,1) == "F")
            {
                Console.WriteLine("StartUp Failed; Aborting...");
                Console.ReadLine();
                return;
            }

            //Reset holder vars
            input = "Terminal  screensnaptest                                                                                                                                                                                                                                            ";
            output = new StringBuilder();
            Console.ReadLine();

            //Get the screenshot
            SnapScreen.SnapActiveWindow(input, output);
            Console.WriteLine(output.ToString());
            Console.ReadLine();
        }
    }
}
