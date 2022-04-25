using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TranslatorBot.Services
{
    public class LoggingService
    {
        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public LoggingService(DiscordSocketClient discord, CommandService commands)
        {
            LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            discord.Log += OnLogAsync;
            commands.Log += OnLogAsync;
        }

        private string LogDirectory { get; }
        private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.log");

        private Task OnLogAsync(LogMessage msg)
        {
            // Create log directory and file if needed
            if (!Directory.Exists(LogDirectory)) Directory.CreateDirectory(LogDirectory);
            if (!File.Exists(LogFile)) File.Create(LogFile).Dispose();

            // Write the log text to the current log file
            var logText =
                $"{DateTime.UtcNow:hh:mm:ss} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            File.AppendAllText(LogFile, logText + "\n");

            // Write the log text to the console as well
            return Console.Out.WriteLineAsync(logText);
        }
    }
}