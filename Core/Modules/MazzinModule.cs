using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Template.Modules
{
    public class MazzinModule : ModuleBase<SocketCommandContext>
    {
        [Command("performhandshake")]
        [Summary("")]
        public async Task DisplayStatus()
        {
            
        }
    }
}