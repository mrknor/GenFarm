using System;
using System.Collections.Generic;
using System.IO;

namespace GenFarm.Services
{
    public class ErrorHandlerService
    {
        private readonly List<ErrorLog> _errorLogs = new List<ErrorLog>();

        public void LogError(Exception ex)
        {
            // Create a new error log and add it to the list
            var errorLog = new ErrorLog
            {
                Timestamp = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };

            _errorLogs.Add(errorLog);
        }

        public void SaveErrorLogs(string filePath)
        {
            // Save the error logs to a file for debugging and analysis
            var logContent = string.Join("\n", _errorLogs.Select(log =>
                $"{log.Timestamp}: {log.Message}\n{log.StackTrace}"));

            File.WriteAllText(filePath, logContent);
        }
    }
}
