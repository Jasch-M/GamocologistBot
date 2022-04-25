using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace TranslatorBot.Services
{
    /// <summary>
    /// Service run at startup to initialize all key services and fetch configurations.
    /// Does not handle logging.
    /// </summary>
    public class StartupService
    {
        /// <summary>
        /// The <see cref="CommandService"/> _commands attribute
        /// handles user commands.
        /// </summary>
        private readonly CommandService _commands;
        
        /// <summary>
        /// The <see cref="IConfigurationRoot"/> _config attribute stores the
        /// bot's configuration. 
        /// </summary>
        private readonly IConfigurationRoot _config;
        
        /// <summary>
        /// Represents the discord account associated with the bot.
        /// </summary>
        private readonly DiscordSocketClient _discord;
        
        /// <summary>
        /// The service provider used by other services in the code.
        /// </summary>
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;
        }
        
        /// <summary>
        /// Starts services, logs in and loads module.
        /// </summary>
        /// <exception cref="Exception">If the bot token is empty, an exception is thrown.</exception>

        public async Task StartAsync()
        {
            var discordToken = _config["tokens:discord"]; // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception(
                    "Please enter your bots token into the `appsettings.json` file found in the applications root directory.");

            await _discord.LoginAsync(TokenType.Bot, discordToken); // Login to discord
            await _discord.StartAsync(); // Connect to the websocket

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(),
                _provider); // Load commands and modules into the command service
        }
    }
}