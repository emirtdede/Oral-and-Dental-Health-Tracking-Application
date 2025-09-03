using System.IO;

namespace DisSagligiTakip.Helpers
{
    public class FileLogService : ILogService
    {
        private readonly string logFilePath;

        public FileLogService()
        {
            logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.txt");

            var logDir = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        public void Log(string userEmail, string action)
        {
            var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {userEmail} | {action}";
            File.AppendAllText(logFilePath, message + Environment.NewLine);
        }
    }
}
