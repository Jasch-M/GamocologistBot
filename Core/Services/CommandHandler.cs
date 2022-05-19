using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace TranslatorBot.Services
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        internal readonly DiscordSocketClient Discord;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config,
            IServiceProvider provider)
        {
            Discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            Discord.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            if (s is not SocketUserMessage msg) return; // Ensure the message is from a user/bot
            if (msg.Author.Id == Discord.CurrentUser.Id) return; // Ignore self when checking commands
            
            
            SocketCommandContext context = new SocketCommandContext(Discord, msg); // Create the command context
            //TODO: Add server message content updating in bot data
            
            int argPos = 0; // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) ||
                msg.HasMentionPrefix(Discord.CurrentUser, ref argPos))
            {
                // Execute the command
                IResult result = await _commands.ExecuteAsync(context, argPos, _provider);

                // If not successful, reply with the error.
                if (!result.IsSuccess) await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}