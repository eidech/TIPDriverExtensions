using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TIPDriverExtensions
{
    public static class SnapScreen
    {
        // user32.dll imports for dealing with native Win32 UI
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static void SnapActiveWindow(string input, StringBuilder output)
        {
            String terminalName, fileName;

            // --- Receive and parse arguments from TIP ---
            terminalName = input.Substring(0, 10);
            terminalName = terminalName.Trim();

            fileName = input.Substring(10, 250);
            fileName = fileName.Trim();

            //--- Validate inputs ---
            //Terminal name cannot be blank or spaces (trimmed above)
            if (terminalName.Equals(""))
            {
                output.Append("FNo terminal name was provided to the method.");
                return;
            }
            //File name cannot be blank or spaced (trimmed above)
            if (fileName.Equals(""))
            {
                output.Append("FNo file name was provided to the method.");
                return;
            }
            //Check passed filename for invalid characters
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                if (fileName.Contains(invalidChar))
                {
                    output.Append("FProvided file name contains invalid character(s)");
                    return;
                }
            }

            //--- Get the screenshot ---
            //Get handle for active window
            IntPtr hWnd = GetForegroundWindow();
            
            //Get window rectangle
            RECT windowRect;
            GetWindowRect(hWnd, out windowRect);

            //Calculate the width and height of the window
            int width = windowRect.Right - windowRect.Left;
            int height =windowRect.Bottom - windowRect.Top;

            //Create a bitmap the size of the window
            Bitmap screenshot = new Bitmap(width, height);

            //Create a graphics object from the bitmap
            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(windowRect.Left, windowRect.Top, 0, 0, new Size(width, height));
            }

            //--- Save the file ---
            //Assemble filepath
            string outFileName = terminalName + fileName + ".jpg";
            string filePath = Path.Combine(Config.ScreenshotPath, outFileName);

            //Check to see if file exists; if it does, delete it
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //Save the bitmap
            try
            {
                screenshot.Save(filePath);
            }
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
                return;
            }

            //Report success
            output.Append("PScreenshot successfully saved to file.");
        }
    }
}
