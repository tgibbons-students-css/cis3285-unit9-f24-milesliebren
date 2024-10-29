using System;
using System.IO;
using System.Xml.Linq;
using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    public class ConsoleLogger : ILogger
    {
        private readonly string logFilePath;

        public ConsoleLogger(string logFilePath)
        {
            this.logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
        }

        public void LogWarning(string message, params object[] args)
        {
            Log("WARN", message, args);
        }

        public void LogInfo(string message, params object[] args)
        {
            Log("INFO", message, args);
        }

        private void Log(string type, string message, params object[] args)
        {
            string formattedMessage = string.Format(message, args);
            string consoleMessage = $"{type}: {formattedMessage}";

            // Log to the console
            Console.WriteLine(consoleMessage);

            // Create XML log entry
            var xmlLogEntry = new XElement("log",
                new XElement("type", type),
                new XElement("message", formattedMessage)
            );

            // Append XML log entry to the file
            AppendLogToFile(xmlLogEntry);
        }

        private void AppendLogToFile(XElement logEntry)
        {
            try
            {
                // Append the XML log entry to the file
                using (var writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to write to log file: {ex.Message}");
            }
        }
    }
}