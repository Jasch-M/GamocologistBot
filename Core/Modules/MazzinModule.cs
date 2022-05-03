using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Template.Modules.Mazzin;

namespace Template.Modules
{
    public class MazzinModule : ModuleBase<SocketCommandContext>
    {
        [Command("mazzin-show-list")]
        [Summary("Show the current list of potential goals")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinShowList()
        {
            GoalList goalList = GoalListService.GoalList;
            List<Embed> goalListEmbeds = EmbedGenerator.GenerateGoalListEmbeds(goalList);
            foreach (Embed goalListEmbed in goalListEmbeds)
            {
                await ReplyAsync(embed: goalListEmbed);
            }
        }
        
        [Command("mazzin-add-list")]
        [Summary("Add a new goal to the list")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddList(string goal)
        {
            GoalList goalList = GoalListService.GoalList;
            bool wasAdded = goalList.Add(goal);
            Embed userAddedEmbed = EmbedGenerator.GenerateGoalListAddEmbed(wasAdded);
            if (wasAdded)
                goalList.Save("../../../Modules/Mazzin/goalListData.txt");
            await ReplyAsync(embed: userAddedEmbed);
        }
        
        [Command("mazzin-add-list")]
        [Summary("Add a new goal to the list")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddList(int value, string name, bool isActive = false, bool isCompleted = false)
        {
            GoalList goalList = GoalListService.GoalList;
            Goal goalToAdd = new Goal(name, value, isActive, isCompleted);
            bool wasAdded = goalList.Add(goalToAdd);
        }
    }
}