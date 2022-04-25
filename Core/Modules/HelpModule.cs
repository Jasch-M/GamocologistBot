using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace TranslatorBot.Modules
{
    /// <summary>
    /// This is the bot's help module and contains the logic for help commands.
    /// </summary>
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// The <see cref="CommandService"/> operated by the bot.
        /// </summary>
        private readonly CommandService _service;
        
        /// <summary>
        /// The <see cref="IConfigurationRoot"/> used for the bot.
        /// </summary>
        private readonly IConfigurationRoot _config;

        /// <summary>
        /// A simple constructor for the <see cref="HelpModule"/>.
        /// It takes in the bot's <see cref="CommandService"/> and <see cref="IConfigurationRoot"/> configuration data
        /// and initializes the respective values in the object.
        /// </summary>
        /// <param name="service">The command service the bot uses for commands.</param>
        /// <param name="config">The configuration data for the bot.</param>
        public HelpModule(CommandService service, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
        }

        /// <summary>
        /// Send an embed containing the help declared before each function in other command modules.
        /// </summary>
        [Command("help")]
        public async Task HelpAsync()
        {
            string prefix = _config["prefix"];
            EmbedBuilder builder = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Description = "Available Commands"
            };

            foreach (ModuleInfo module in _service.Modules)
            {
                string description = null;
                foreach (CommandInfo cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"{prefix}{cmd.Aliases.First()}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }
            
            await ReplyAsync("", false, builder.Build());
        }
    }
}

