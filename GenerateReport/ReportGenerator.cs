using System;
using System.Collections.Generic;
using System.Text;
using MsAccess = Microsoft.Office.Interop.Access;
using System.IO;

namespace PARTSExtensions
{
    public static class ReportGenerator
    {

        public static void GenerateReport(string input, StringBuilder output)
        {

            if (!Config.SuccessfulStartup)
            {
                output.Append("FPARTS Extensions have not been initialized.");
                return;
            }

            String reportName, whereClause;
            MsAccess.Application app = new MsAccess.Application();

            try { 

                //Receive and parse arguments from TIP
                reportName = input.Substring(0, 50);
                reportName = reportName.Trim();

                whereClause = input.Substring(50);
                whereClause = whereClause.Trim();

                //Generate FileName from Report Name and Current Time/Date
                StringBuilder FileName = new StringBuilder();
                FileName.Append(Config.TerminalName);
                FileName.Append(reportName);
                FileName.Append(System.DateTime.Now.Year.ToString());
                FileName.Append(System.DateTime.Now.Month.ToString());
                FileName.Append(System.DateTime.Now.Day.ToString());
                FileName.Append(System.DateTime.Now.Hour.ToString());
                FileName.Append(System.DateTime.Now.Minute.ToString());
                FileName.Append(System.DateTime.Now.Second.ToString());
                FileName.Append(".pdf");

                //Generate Report, Return no errors
                app.OpenCurrentDatabase(Config.DatabasePath, false, "");
                app.DoCmd.OpenReport(reportName, MsAccess.AcView.acViewPreview, Type.Missing, whereClause, MsAccess.AcWindowMode.acWindowNormal, Type.Missing);
                app.Visible = false;
                app.DoCmd.OutputTo(MsAccess.AcOutputObjectType.acOutputReport, reportName, MsAccess.Constants.acFormatPDF, Config.ReportPath + FileName.ToString(), true, Type.Missing, Type.Missing);
                app.CloseCurrentDatabase();
                app.Quit();
                output.Append("PSystem encountered no errors");

            //Deal with any exceptions, Return error
            } catch (Exception ex) {
                app.Quit();
                output.Append("F");
                if (ex.ToString().Length > 300)
                {
                    output.Append(ex.ToString().Substring(0, 300));
                } else
                {
                    output.Append(ex.ToString());
                }
            }
        }

        
    }
}
