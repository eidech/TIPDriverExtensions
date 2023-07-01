using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace TIPDriverExtensions
{
    public static class Config
    {
        private static bool _successfulStartup = false;
        private static String _terminalName, _reportPath, _databasePath, _screenshotPath;

        public static bool SuccessfulStartup { get { return _successfulStartup; } }
        public static string TerminalName { get { return _terminalName; } }
        public static string ReportPath { get { return _reportPath; } } 
        public static string DatabasePath { get { return _databasePath; } }
        public static string ScreenshotPath { get { return _screenshotPath; } }

        public static void StartUp(string input, StringBuilder output)
        {
            String[] terminalFiles, screenshotFiles;

            //Receive and parse arguments from TIP
            _terminalName = input.Substring(0, 10);
            _terminalName = _terminalName.Trim();

            _reportPath = input.Substring(10, 250);
            _reportPath = _reportPath.Trim();

            _databasePath = input.Substring(260, 250);
            _databasePath = _databasePath.Trim();

            _screenshotPath = input.Substring(510, 250);
            _screenshotPath = _screenshotPath.Trim();

            Console.WriteLine(_reportPath);
            Console.WriteLine(_databasePath);

            //Check for blank Terminal Name
            if (_terminalName.Equals(""))
            {
                output.Append("FNo terminal name was provided to the method.");
                return;
            }

            //Check for blank Report Path
            if (_reportPath.Equals(""))
            {
                output.Append("FNo path was provided to the method.");
                return;
            }

            //Check for blank Database Path
            if (_databasePath.Equals(""))
            {
                output.Append("FNo database path was provided to the method.");
                return;
            }

            //Check for blank Screenshot Path
            if (_screenshotPath.Equals(""))
            {
                output.Append("FNo screenshot path was provided to the method.");
                return;
            }

            //Check to see that report path points to a valid directory
            if (!Directory.Exists(_reportPath))
            {
                output.Append("FThe report path does not point to a valid directory.");
                return;
            }

            //Check to see that database path points to a valid directory
            if (!Directory.Exists(Path.GetDirectoryName(_databasePath)))
            {
                output.Append("FThe database path does not point to a valid directory.");
                return;
            }

            //Check to see that screenshot path points to a valid directory
            if (!Directory.Exists(_screenshotPath))
            {
                output.Append("FThe screenshot path does not point to a valid directory.");
                return;
            }

            //Check to see that database path points to a valid mdb file
            if (!File.Exists(_databasePath))
            {
                output.Append("FThe database path does not point to a valid .mdb file.");
                return;
            }

            try
            {
                //Find all report files that match the Terminal Name
                terminalFiles = Directory.GetFiles(_reportPath, _terminalName + "*");

                //Delete all report files that match Terminal Name
                foreach (String file in terminalFiles)
                {
                    File.Delete(file);
                }

                //Find all screenshot files that match the Terminal Name
                screenshotFiles = Directory.GetFiles(_screenshotPath, _terminalName + "*");

                //Delete all screenshot files that match Terminal Name
                foreach (String file in screenshotFiles)
                {
                    File.Delete(file);
                }
            }
            //Deal with any exceptions
            catch (Exception ex)
            {
                output.Append("F");
                if (ex.ToString().Length > 300)
                {
                    output.Append(ex.ToString().Substring(0, 300));
                }
                else
                {
                    output.Append(ex.ToString());
                }
            }

            output.Append("PClass successfully initialized.");

            _successfulStartup = true;
        }

        public static void GetPrinters(String input, StringBuilder output)
        {
            int numberOfPrinters = 0;
            List<string> listOfPrinters = new List<string>();

            //Get the list of printers from the system
            try
            {
                foreach (String printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    numberOfPrinters++;
                    listOfPrinters.Add(printer);
                }
            } catch (Exception ex)
            {
                output.Append("F");
                output.Append(ex.ToString());
                return;
            }

            output.Append("P");

            //Put the number of printers at the beginning of the output as 2-digit number
            if (numberOfPrinters < 10)
            {
                output.Append("0");
                output.Append(numberOfPrinters.ToString());
            }
            else
            {
                output.Append(numberOfPrinters.ToString());
            }

            //Add the printers to the output with a pipe separating names
            foreach (String printer in listOfPrinters)
            {
                output.Append(printer);
                output.Append("|");
            }
        }
    }
}
