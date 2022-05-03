using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;

namespace Template.Modules.Mazzin;

public static class EmbedGenerator
{
    public static List<Embed> GenerateGoalListEmbeds(GoalList goals)
    {
        List<Embed> goalListEmbeds = new();
        List<Goal> goalsToAdd;
        Embed goalListEmbed;
        
        for (int i = 0; i < goals.NumberOfGoals; i += 10)
        {
            goalsToAdd = goals.Goals.GetRange(i, 10);
            goalListEmbed = GenerateGoalListEmbed(goalsToAdd);
            goalListEmbeds.Add(goalListEmbed);
        }
        
        int lastIndex = goals.NumberOfGoals / 10;
        int numberOfGoalsLeft = goals.NumberOfGoals % 10;
        goalsToAdd = goals.Goals.GetRange(lastIndex, numberOfGoalsLeft);
        goalListEmbed = GenerateGoalListEmbed(goalsToAdd);
        goalListEmbeds.Add(goalListEmbed);
        
        return goalListEmbeds;
    }

    private static Embed GenerateGoalListEmbed(List<Goal> goals)
    {
        EmbedBuilder goalListEmbedBuilder = BuildBotEmbedBase("Mazzin's Goal List");
        StringBuilder goalListStringBuilder = new ();
        
        foreach (Goal goal in goals)
        {
            string goalString = $":arrow_right: {goal.Name}\n";
            goalListStringBuilder.Append(goalString);
        }
        
        string goalListString = goalListStringBuilder.ToString();

        EmbedFieldBuilder goalListField = new()
        {
            Name = "Goals",
            IsInline = false,
            Value = goalListString
        };

        goalListEmbedBuilder.AddField(goalListField);
        Embed goalListEmbed = goalListEmbedBuilder.Build();
        return goalListEmbed;
    }

    public static Embed GenerateGoalListAddEmbed(bool wasSuccess)
    {
        EmbedBuilder goalListAddEmbedBuilder = BuildBotEmbedBase("Mazzin's Goal List");

        string goalListAddEmbedString = 
            wasSuccess 
                ? "The goal was successfully added! <a:coolcheckmark:744653262514421871>" 
                : "The goal already exists. So the goal was not added. <a:crossduswag:867153820667478036>";
        
        EmbedFieldBuilder goalListAddField = new()
        {
            Name = "Goal Adding Result",
            IsInline = false,
            Value = goalListAddEmbedString
        };

        goalListAddEmbedBuilder.AddField(goalListAddField);
        Embed goalListAddEmbed = goalListAddEmbedBuilder.Build();
        return goalListAddEmbed;
    }
    
    /// <summary>
    /// Generates a basic embed builder to be used in other embed generators.
    /// </summary>
    /// <returns>A basic embed builder to be used in other embed generators.</returns>
    private static EmbedBuilder BuildBotEmbedBase(string title)
    {
        EmbedBuilder botEmbedBase = new EmbedBuilder
        {
            Author = GenerateBotAuthor(),
            Color = Color.DarkBlue,
            Timestamp = DateTimeOffset.Now,
            Title = title,
            //botEmbedBase.ImageUrl = "https://cdns.c3dt.com/preview/9825728-com.deepl.translate.alllanguagetranslator.jpg";
            Url = "https://gamocologist.com/bots/gamotranslator"
        };
        return botEmbedBase;
    }

    /// <summary>
    /// Generates the author information for use in embeds.
    /// </summary>
    /// <returns>An embed author builder containing the author information for use in embeds.</returns>
    private static EmbedAuthorBuilder GenerateBotAuthor()
    {
        EmbedAuthorBuilder embedAuthorBuilder = new EmbedAuthorBuilder
        {
            Name = "GamoMazzin",
            IconUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fapitracker.io%2Fa%2Fdeepl&psig=AOvVaw028t84t0CBC87q4q-IuCMc&ust=1650642593447000&source=images&cd=vfe&ved=0CAwQjRxqFwoTCMDqzf3ApfcCFQAAAAAdAAAAABAO",
            Url = "https://gamocologist.com/bots/mazzin"
        };
        return embedAuthorBuilder;
    }
}