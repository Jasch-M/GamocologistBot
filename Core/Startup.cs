﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Modules.Utils.Discord;
using Template.Services;
using TranslatorBot.Services;

namespace TranslatorBot
{
    public class Startup
    {
        internal static Bot Bot;

        internal static EmailService Email = new();
        public Startup(string[] args)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string configFilePath = $"{baseDirectory}/../../../";
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(configFilePath)
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public static async Task RunAsync(string[] args)
        {
            Startup startup = new(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            ServiceCollection services = new ServiceCollection(); // Create a new instance of a service collection
            ConfigureServices(services);

            ServiceProvider provider = services.BuildServiceProvider(); // Build the service provider
            provider.GetRequiredService<LoggingService>(); // Start the logging service
            provider.GetRequiredService<CommandHandler>(); // Start the command handler service

            // Start the startup service and keep the program alive
            await provider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            DiscordSocketClient bot = new DiscordSocketClient(new DiscordSocketConfig
            {
                // Add discord to the collection
                LogLevel = LogSeverity.Verbose, // Tell the logger to give Verbose amount of info
                MessageCacheSize = 1000 // Cache 1,000 messages per channel
            });

            Bot = new Bot(bot);
            
            services.AddSingleton(bot)
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    // Add the command service to the collection
                    LogLevel = LogSeverity.Verbose, // Tell the logger to give Verbose amount of info
                    DefaultRunMode = RunMode.Async // Force all commands to run async by default
                }))
                .AddSingleton<CommandHandler>() // Add the command handler to the collection
                .AddSingleton<StartupService>() // Add startup service to the collection
                .AddSingleton<LoggingService>() // Add logging service to the collection
                .AddSingleton<Random>() // Add random to the collection
                .AddSingleton(Configuration); // Add the configuration to the collection
        }
    }
}