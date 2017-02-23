using System;
using System.IO;
using System.Text;

namespace Mobilize
{
    public class ErrorLog
    {
        private string sLogFormat;
        private string sErrorTime;
        public ErrorLog()
        {
            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
        }

        public void generateErrorLog(object message, string stackTrace)
        {
            string filepath = Environment.CurrentDirectory + @"\DesktopAgentLog.txt";

            // Verify if the file exists.
            if (!File.Exists(filepath))
            {
                // Create the file.
                using (FileStream fs = File.Create(filepath))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(sErrorTime + ":" + message + "Stack Trace" + stackTrace);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            else
            {
                string text = File.ReadAllText(filepath);
                string ErrorMessage = "Error log" + Environment.NewLine + message + Environment.NewLine +"Stack Trace" + stackTrace;
                text = text.Insert(text.Length, ErrorMessage);
                File.WriteAllText(filepath, text);
            }
        }
    }
}