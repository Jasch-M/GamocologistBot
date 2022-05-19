using System.Threading.Tasks;
using Discord.Commands;

namespace Template.Modules;

public class SplatoonModule : ModuleBase<SocketCommandContext>
{
    [Command("splatperkinfo")]
    [Summary("Gives you a rundown on an ability's information and scalling.")]
    public async Task GearAbilityInfo(string ability, string weapon = "")
    {
        
    }
}