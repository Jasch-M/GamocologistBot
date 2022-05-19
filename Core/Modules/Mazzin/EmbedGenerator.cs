using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Template.Modules.Utils;

namespace Template.Modules.Mazzin;

public static class EmbedGenerator
{
    public static List<Embed> GenerateGoalListEmbeds(GoalList goals)
    {
        List<Embed> goalListEmbeds = new();
        List<Goal> goalsToAdd;
        Embed goalListEmbed;

        if (goals.NumberOfGoals > 0)
        {
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
        }
        else
        {
            Embed emptyGoalListEmbed = GenerateEmptyGoalListEmbed();
            goalListEmbeds.Add(emptyGoalListEmbed);
        }

        return goalListEmbeds;
    }

    private static Embed GenerateGoalListEmbed(List<Goal> goals)
    {
        EmbedBuilder goalListEmbedBuilder = BuildBotEmbedBase("Mazzin's Goal List");
        goalListEmbedBuilder.Color = Color.Default;
        StringBuilder goalListStringBuilder = new();

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

    private static Embed GenerateEmptyGoalListEmbed()
    {
        EmbedBuilder emptyGoalListEmbedBuilder = BuildBotEmbedBase("Mazzin's Goal List:");
        emptyGoalListEmbedBuilder.Color = Color.Default;

        EmbedFieldBuilder emptyGoalListField = new()
        {
            Name = "Empty Goal List",
            IsInline = true,
            Value = "The goal list is empty. To add goals, do §mazzin-add-list"
        };

        emptyGoalListEmbedBuilder.AddField(emptyGoalListField);
        Embed emptyGoalListEmbed = emptyGoalListEmbedBuilder.Build();

        return emptyGoalListEmbed;
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
        
        goalListAddEmbedBuilder.Color = wasSuccess ? Color.DarkGreen : Color.Red;

        goalListAddEmbedBuilder.AddField(goalListAddField);
        Embed goalListAddEmbed = goalListAddEmbedBuilder.Build();
        return goalListAddEmbed;
    }

    public static Embed GenerateGoalListRemoveEmbed(bool wasSuccess)
    {
        EmbedBuilder goalListRemoveEmbedBuilder = BuildBotEmbedBase("Mazzin's Goal List");

        string goalListRemoveEmbedString =
            wasSuccess
                ? "The goal was successfully removed! <a:coolcheckmark:744653262514421871>"
                : "The goal does not exist. So the goal was not removed. <a:crossduswag:867153820667478036>";

        EmbedFieldBuilder goalListRemoveField = new()
        {
            Name = "Goal Removing Result",
            IsInline = false,
            Value = goalListRemoveEmbedString
        };

        goalListRemoveEmbedBuilder.Color = wasSuccess ? Color.DarkGreen : Color.Red;

        goalListRemoveEmbedBuilder.AddField(goalListRemoveField);
        Embed goalListRemoveEmbed = goalListRemoveEmbedBuilder.Build();
        return goalListRemoveEmbed;
    }

    public static Embed GenerateGoalListClearEmbed()
    {
        EmbedBuilder goalListClearEmbedBuilder = BuildBotEmbedBase("Mazzin's Goal List");
        goalListClearEmbedBuilder.Color = Color.Teal;

        EmbedFieldBuilder goalListClearField = new()
        {
            Name = "Goal List Clearing Result",
            IsInline = false,
            Value = "The goal list was successfully cleared! <a:coolcheckmark:744653262514421871>"
        };

        goalListClearEmbedBuilder.AddField(goalListClearField);
        Embed goalListClearEmbed = goalListClearEmbedBuilder.Build();
        return goalListClearEmbed;
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
            Color = Color.Red,
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
        EmbedAuthorBuilder embedAuthorBuilder = new()
        {
            Name = "GamoMazzin",
            IconUrl =
                "https://www.google.com/url?sa=i&url=https%3A%2F%2Fapitracker.io%2Fa%2Fdeepl&psig=AOvVaw028t84t0CBC87q4q-IuCMc&ust=1650642593447000&source=images&cd=vfe&ved=0CAwQjRxqFwoTCMDqzf3ApfcCFQAAAAAdAAAAABAO",
            Url = "https://gamocologist.com/bots/mazzin"
        };
        return embedAuthorBuilder;
    }

    public static List<Embed> GenerateAuthorizedMazzinUsersEmbed(
        (HashSet<ulong> authorizedUsers, HashSet<ulong> authorizedRoles, HashSet<ulong> authorizedChannels) authorized)
    {
        EmbedBuilder authorizedMazzinUsersEmbedBuilder = BuildBotEmbedBase("Authorized Entities");
        authorizedMazzinUsersEmbedBuilder.Color = Color.Default;

        List<EmbedBuilder> authorizedUsersEmbedBuilders = GenerateAuthorizedElements(authorized.authorizedUsers,
            Identification.GetServerUserById,
            (SocketGuildUser element) => element.Nickname ?? element.Username, "User");
        List<EmbedBuilder> authorizedRolesEmbedBuilders = GenerateAuthorizedElements(authorized.authorizedRoles,
            Identification.GetServerRoleById,
            (SocketRole element) => element.Name, "Role");
        List<EmbedBuilder> authorizedChannelsEmbedBuilders = GenerateAuthorizedElements(authorized.authorizedChannels,
            Identification.GetServerTextChannelById,
            (SocketTextChannel element) => element.Name, "Channel");

        List<Embed> authorizedMazzinUsersEmbeds = new List<Embed>();

        foreach (EmbedBuilder authorizedUsersEmbedBuilder in authorizedUsersEmbedBuilders)
        {
            Embed authorizedMazzinUsersEmbed = authorizedUsersEmbedBuilder.Build();
            authorizedMazzinUsersEmbeds.Add(authorizedMazzinUsersEmbed);
        }

        foreach (EmbedBuilder authorizedRolesEmbedBuilder in authorizedRolesEmbedBuilders)
        {
            Embed authorizedMazzinUsersEmbed = authorizedRolesEmbedBuilder.Build();
            authorizedMazzinUsersEmbeds.Add(authorizedMazzinUsersEmbed);
        }

        foreach (EmbedBuilder authorizedChannelsEmbedBuilder in authorizedChannelsEmbedBuilders)
        {
            Embed authorizedMazzinUsersEmbed = authorizedChannelsEmbedBuilder.Build();
            authorizedMazzinUsersEmbeds.Add(authorizedMazzinUsersEmbed);
        }

        return authorizedMazzinUsersEmbeds;
    }

    private static List<EmbedBuilder> GenerateAuthorizedElements<TElement>(HashSet<ulong> authorizedelements,
        Func<ulong, TElement> findingFunction, Func<TElement, string> elementNameFunction,
        string elementNameLabel)
    {
        string elementNameLabelUpper = elementNameLabel.ToUpper();
        List<EmbedFieldBuilder> fieldBuilders = BuildAuthorizedElementFields(authorizedelements, findingFunction,
            elementNameFunction, elementNameLabel, elementNameLabelUpper);
        List<EmbedBuilder> embedBuilders = Utils.DivideUpFields(fieldBuilders);

        return embedBuilders;
    }

    private static List<EmbedFieldBuilder> BuildAuthorizedElementFields<TElement>(HashSet<ulong> authorizedelements,
        Func<ulong, TElement> findingFunction, Func<TElement, string> elementNameFunction, string elementNameLabel,
        string elementNameLabelUpper)
    {
        int numberOfCharacters = 0;
        List<EmbedFieldBuilder> fieldBuilders = new();
        EmbedFieldBuilder currentFieldBuilder = new();
        StringBuilder currentFieldBuilderStringBuilder = new();
        foreach (ulong authorizedId in authorizedelements)
        {
            TElement authorizedElement = findingFunction(authorizedId);
            string authorizedElementName = elementNameFunction(authorizedElement);
            string authorizedName = authorizedElement == null
                ? authorizedElementName
                : $"NOT FOUND {elementNameLabelUpper} WITH ID: {authorizedId}";
            string authorizedString = $":arrow_right: {authorizedName}\n";
            int entryLength = authorizedString.Length;
            numberOfCharacters += entryLength;
            if (numberOfCharacters > 1024)
            {
                currentFieldBuilder.Name = "Authorized Users:";
                currentFieldBuilder.IsInline = false;
                currentFieldBuilder.Value = currentFieldBuilderStringBuilder.ToString();
                fieldBuilders.Add(currentFieldBuilder);
                currentFieldBuilder = new();
                numberOfCharacters = entryLength;
            }

            currentFieldBuilderStringBuilder.Append(authorizedString);
        }

        currentFieldBuilder.Name = $"Authorized {elementNameLabel}s:";
        currentFieldBuilder.IsInline = false;
        currentFieldBuilder.Value = currentFieldBuilderStringBuilder.ToString();
        fieldBuilders.Add(currentFieldBuilder);

        return fieldBuilders;
    }





    public static Embed GenerateAuthorizedMazzinUsersAddedEmbed(AuthorizationResult wasAdded,
        ulong id, string name = "")
    {
        EmbedBuilder authorizedMazzinUsersAddedEmbedBuilder = BuildBotEmbedBase("Authorized Entities Added");

        EmbedFieldBuilder authorizedMazzinUsersAddedField = new()
        {
            Name = "Authorized User Addition",
            IsInline = false,
            Value = MazzinAddedAuthorizedUserResponseGenerator(wasAdded, id, name)
        };
        
        authorizedMazzinUsersAddedEmbedBuilder.Color = wasAdded switch
        {
            AuthorizationResult.SUCCESS => Color.DarkGreen,
            AuthorizationResult.ALREADY_PRESENT => Color.Teal,
            AuthorizationResult.FAILED => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(wasAdded), wasAdded, null)
        };

        authorizedMazzinUsersAddedEmbedBuilder.AddField(authorizedMazzinUsersAddedField);
        Embed authorizedMazzinUsersAddedEmbed = authorizedMazzinUsersAddedEmbedBuilder.Build();

        return authorizedMazzinUsersAddedEmbed;
    }

    private static string MazzinAddedAuthorizedUserResponseGenerator(AuthorizationResult wasAdded,
        ulong id, string name)
    {
        return wasAdded switch
        {
            AuthorizationResult.SUCCESS => name == ""
                ? $"<a:coolcheckmark:744653262514421871> user id {id.ToString()} added successfully to the list of authorized users."
                : $"<a:coolcheckmark:744653262514421871> user @{name} ({id.ToString()} id) added successfully to the list of authorized users.",
            AuthorizationResult.ALREADY_PRESENT => name == ""
                ? $"<a:crossduswag:867153820667478036> user id {id.ToString()} is already present in the list of authorized users."
                : $"<a:crossduswag:867153820667478036> user {name} ({id.ToString()} id) is already authorized.",
            AuthorizationResult.FAILED => name == ""
                ? $"<a:crossduswag:867153820667478036> user id {id.ToString()} failed to be added to the list of authorized users."
                : $"<a:crossduswag:867153820667478036> user {name} ({id.ToString()} id) could not be added to the list of authorized users.",
            _ => throw new ArgumentOutOfRangeException(nameof(wasAdded), wasAdded, null)
        };
    }

    public static Embed GenerateAuthorizedMazzinUsersRemovedEmbed(AuthorizationResult wasRemoved,
        ulong id, string name = "")
    {
        EmbedBuilder authorizedMazzinUsersRemovedBuilder = BuildBotEmbedBase("Authorized Entities Removed");

        EmbedFieldBuilder authorizedMazzinUsersRemovedField = new()
        {
            Name = "Authorized User Removal",
            IsInline = false,
            Value = MazzinRemovedAuthorizedUserResponseGenerator(wasRemoved, id, name)
        };
        
        authorizedMazzinUsersRemovedBuilder.Color = wasRemoved switch
        {
            AuthorizationResult.SUCCESS => Color.DarkGreen,
            AuthorizationResult.ALREADY_PRESENT => Color.Teal,
            AuthorizationResult.FAILED => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(wasRemoved), wasRemoved, null)
        };

        authorizedMazzinUsersRemovedBuilder.AddField(authorizedMazzinUsersRemovedField);
        Embed authorizedMazzinUsersRemovedEmbed = authorizedMazzinUsersRemovedBuilder.Build();

        return authorizedMazzinUsersRemovedEmbed;
    }

    private static string MazzinRemovedAuthorizedUserResponseGenerator(
        AuthorizationResult wasRemoved, ulong id, string name)
    {
        return wasRemoved switch
        {
            AuthorizationResult.SUCCESS => name == ""
                ? $"<a:coolcheckmark:744653262514421871> user id {id.ToString()} removed successfully from the list of authorized users."
                : $"<a:coolcheckmark:744653262514421871> user @{name} ({id.ToString()} id) removed successfully from the list of authorized users.",
            AuthorizationResult.ALREADY_PRESENT => name == ""
                ? $"<a:crossduswag:867153820667478036> user id {id.ToString()} is not present in the list of authorized users."
                : $"<a:crossduswag:867153820667478036> user {name} ({id.ToString()} id) is not present in the list of authorized users.",
            AuthorizationResult.FAILED => name == ""
                ? $"<a:crossduswag:867153820667478036> user id {id.ToString()} failed to be removed from the list of authorized users."
                : $"<a:crossduswag:867153820667478036> user {name} ({id.ToString()} id) could not be removed from the list of authorized users.",
            _ => throw new ArgumentOutOfRangeException(nameof(wasRemoved), wasRemoved, null)
        };
    }





    public static Embed GenerateAuthorizedMazzinRolesAddedEmbed(AuthorizationResult wasAdded,
        ulong id, string name = "")
    {
        EmbedBuilder authorizedMazzinRolesAddedEmbedBuilder = BuildBotEmbedBase("Authorized Entities Added");

        EmbedFieldBuilder authorizedMazzinRolesAddedField = new()
        {
            Name = "Authorized Role Addition",
            IsInline = false,
            Value = MazzinAddedAuthorizedRoleResponseGenerator(wasAdded, id, name)
        };
        
        authorizedMazzinRolesAddedEmbedBuilder.Color = wasAdded switch
        {
            AuthorizationResult.SUCCESS => Color.DarkGreen,
            AuthorizationResult.ALREADY_PRESENT => Color.Teal,
            AuthorizationResult.FAILED => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(wasAdded), wasAdded, null)
        };

        authorizedMazzinRolesAddedEmbedBuilder.AddField(authorizedMazzinRolesAddedField);
        Embed authorizedMazzinRolesAddedEmbed = authorizedMazzinRolesAddedEmbedBuilder.Build();

        return authorizedMazzinRolesAddedEmbed;
    }

    private static string MazzinAddedAuthorizedRoleResponseGenerator(AuthorizationResult wasAdded,
        ulong id, string name)
    {
        return wasAdded switch
        {
            AuthorizationResult.SUCCESS => name == ""
                ? $"<a:coolcheckmark:744653262514421871> role id {id.ToString()} added successfully to the list of authorized roles."
                : $"<a:coolcheckmark:744653262514421871> role @{name} ({id.ToString()} id) added successfully to the list of authorized roles.",
            AuthorizationResult.ALREADY_PRESENT => name == ""
                ? $"<a:crossduswag:867153820667478036> role id {id.ToString()} is already present in the list of authorized roles."
                : $"<a:crossduswag:867153820667478036> role {name} ({id.ToString()} id) is already authorized.",
            AuthorizationResult.FAILED => name == ""
                ? $"<a:crossduswag:867153820667478036> role id {id.ToString()} failed to be added to the list of authorized roles."
                : $"<a:crossduswag:867153820667478036> role @{name} ({id.ToString()} id) could not be added to the list of authorized roles.",
            _ => throw new ArgumentOutOfRangeException(nameof(wasAdded), wasAdded, null)
        };
    }

    public static Embed GenerateAuthorizedMazzinRolesRemovedEmbed(AuthorizationResult wasRemoved,
        ulong id, string name = "")
    {
        EmbedBuilder authorizedMazzinRolesRemovedBuilder = BuildBotEmbedBase("Authorized Entities Removed");

        EmbedFieldBuilder authorizedMazzinRolesRemovedField = new()
        {
            Name = "Authorized Role Removal",
            IsInline = false,
            Value = MazzinRemovedAuthorizedRoleResponseGenerator(wasRemoved, id, name)
        };
        
        authorizedMazzinRolesRemovedBuilder.Color = wasRemoved switch
        {
            AuthorizationResult.SUCCESS => Color.DarkGreen,
            AuthorizationResult.ALREADY_PRESENT => Color.Teal,
            AuthorizationResult.FAILED => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(wasRemoved), wasRemoved, null)
        };

        authorizedMazzinRolesRemovedBuilder.AddField(authorizedMazzinRolesRemovedField);
        Embed authorizedMazzinRolesRemovedEmbed = authorizedMazzinRolesRemovedBuilder.Build();

        return authorizedMazzinRolesRemovedEmbed;
    }

    private static string MazzinRemovedAuthorizedRoleResponseGenerator(
        AuthorizationResult wasRemoved, ulong id, string name)
    {
        return wasRemoved switch
        {
            AuthorizationResult.SUCCESS => name == ""
                ? $"<a:coolcheckmark:744653262514421871> role id {id.ToString()} removed successfully from the list of authorized roles."
                : $"<a:coolcheckmark:744653262514421871> role @{name} ({id.ToString()} id) removed successfully from the list of authorized roles.",
            AuthorizationResult.ALREADY_PRESENT => name == ""
                ? $"<a:crossduswag:867153820667478036> role id {id.ToString()} is not present in the list of authorized roles."
                : $"<a:crossduswag:867153820667478036> role {name} ({id.ToString()} id) is not present in the list of authorized roles.",
            AuthorizationResult.FAILED => name == ""
                ? $"<a:crossduswag:867153820667478036> role id {id.ToString()} failed to be removed from the list of authorized roles."
                : $"<a:crossduswag:867153820667478036> role {name} ({id.ToString()} id) could not be removed from the list of authorized roles.",
            _ => throw new ArgumentOutOfRangeException(nameof(wasRemoved), wasRemoved, null)
        };
    }





    public static Embed GenerateAuthorizedMazzinChannelsAddedEmbed(AuthorizationResult wasAdded,
        ulong id, string name = "")
    {
        EmbedBuilder authorizedMazzinTextChannelsAddedEmbedBuilder = BuildBotEmbedBase("Authorized Entities Added");

        EmbedFieldBuilder authorizedMazzinTextChannelsAddedField = new()
        {
            Name = "Authorized Text Channels Addition",
            IsInline = false,
            Value = MazzinAddedAuthorizedChannelResponseGenerator(wasAdded, id, name)
        };
        
        authorizedMazzinTextChannelsAddedEmbedBuilder.Color = wasAdded switch
        {
            AuthorizationResult.SUCCESS => Color.DarkGreen,
            AuthorizationResult.ALREADY_PRESENT => Color.Teal,
            AuthorizationResult.FAILED => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(wasAdded), wasAdded, null)
        };

        authorizedMazzinTextChannelsAddedEmbedBuilder.AddField(authorizedMazzinTextChannelsAddedField);
        Embed authorizedMazzinTextChannelsAddedEmbed = authorizedMazzinTextChannelsAddedEmbedBuilder.Build();

        return authorizedMazzinTextChannelsAddedEmbed;
    }

    private static string MazzinAddedAuthorizedChannelResponseGenerator(
        AuthorizationResult wasAdded, ulong id, string name)
    {
        return wasAdded switch
        {
            AuthorizationResult.SUCCESS => name == ""
                ? $"<a:coolcheckmark:744653262514421871> text channel id {id.ToString()} added successfully to the list of authorized text channels."
                : $"<a:coolcheckmark:744653262514421871> text channel #{name} ({id.ToString()} id) added successfully to the list of authorized text channels.",
            AuthorizationResult.ALREADY_PRESENT => name == ""
                ? $"<a:crossduswag:867153820667478036> text channel id {id.ToString()} is already present in the list of authorized text channels."
                : $"<a:crossduswag:867153820667478036> text channel #{name} ({id.ToString()} id) is already authorized.",
            AuthorizationResult.FAILED => name == ""
                ? $"<a:crossduswag:867153820667478036> text channel id {id.ToString()} failed to be added to the list of authorized text channels."
                : $"<a:crossduswag:867153820667478036> text channel #{name} ({id.ToString()} id) could not be added to the list of authorized text channels.",
            _ => throw new ArgumentOutOfRangeException(nameof(wasAdded), wasAdded, null)
        };
    }

    public static Embed GenerateAuthorizedMazzinChannelsRemovedEmbed(
        AuthorizationResult wasRemoved,
        ulong id, string name = "")
    {
        EmbedBuilder authorizedMazzinTextChannelsRemovedBuilder = BuildBotEmbedBase("Authorized Entities Removed");

        EmbedFieldBuilder authorizedMazzinTextChannelsRemovedField = new()
        {
            Name = "Authorized Text Channel Removal",
            IsInline = false,
            Value = MazzinRemovedAuthorizedChannelResponseGenerator(wasRemoved, id, name)
        };

        authorizedMazzinTextChannelsRemovedBuilder.Color = wasRemoved switch
        {
            AuthorizationResult.SUCCESS => Color.DarkGreen,
            AuthorizationResult.ALREADY_PRESENT => Color.Teal,
            AuthorizationResult.FAILED => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(wasRemoved), wasRemoved, null)
        };

        authorizedMazzinTextChannelsRemovedBuilder.AddField(authorizedMazzinTextChannelsRemovedField);
        Embed authorizedMazzinTextChannelsRemovedEmbed = authorizedMazzinTextChannelsRemovedBuilder.Build();

        return authorizedMazzinTextChannelsRemovedEmbed;
    }

    private static string MazzinRemovedAuthorizedChannelResponseGenerator(
        AuthorizationResult wasRemoved, ulong id, string name)
    {
        return wasRemoved switch
        {
            AuthorizationResult.SUCCESS => name == ""
                ? $"<a:coolcheckmark:744653262514421871> text channel id {id.ToString()} removed successfully from the list of authorized text channels."
                : $"<a:coolcheckmark:744653262514421871> text channel #{name} ({id.ToString()} id) removed successfully from the list of authorized text channels.",
            AuthorizationResult.ALREADY_PRESENT => name == ""
                ? $"<a:crossduswag:867153820667478036> text channel id {id.ToString()} is not present in the list of authorized text channels."
                : $"<a:crossduswag:867153820667478036> text channel #{name} ({id.ToString()} id) is not present in the list of authorized text channels.",
            AuthorizationResult.FAILED => name == ""
                ? $"<a:crossduswag:867153820667478036> text channel id {id.ToString()} failed to be removed from the list of authorized text channels."
                : $"<a:crossduswag:867153820667478036> text channel #{name} ({id.ToString()} id) could not be removed from the list of authorized text channels.",
            _ => throw new ArgumentOutOfRangeException(nameof(wasRemoved), wasRemoved, null)
        };
    }

    public static Embed GenerateAuthorizedMazzinClearedEmbed()
    {
        EmbedBuilder authorizedMazzinClearedEmbedBuilder = BuildBotEmbedBase("Authorized Entities Cleared");

        EmbedFieldBuilder authorizedMazzinClearedField = new()
        {
            Name = "Authorized Entities Cleared",
            IsInline = false,
            Value = "<a:coolcheckmark:744653262514421871> The list of authorized entities has been cleared."
        };
        
        authorizedMazzinClearedEmbedBuilder.Color = Color.Teal;

        authorizedMazzinClearedEmbedBuilder.AddField(authorizedMazzinClearedField);
        Embed authorizedMazzinClearedEmbed = authorizedMazzinClearedEmbedBuilder.Build();
        return authorizedMazzinClearedEmbed;
    }

    public static Embed GenerateMysteryChanceColorSetEmbed(Color color, RequestResult requestResult, string colorPosition)
    {
        EmbedBuilder mysteryChanceColorSetEmbed = BuildBotEmbedBase("Goal Formatting Request");

        string colorPositionLower = colorPosition.ToLower();

        EmbedFieldBuilder mysteryChanceColorField = new()
        {
            Name = $"{colorPosition} Goal Color Change Request",
            IsInline = false,
            Value = requestResult switch
            {
                RequestResult.SUCCESSFUL => $"The color change was successfully applied to the {colorPositionLower} color of the mystery chance.",
                RequestResult.FAILED => $"An issue occured when requesting the change of the {colorPositionLower} of the mystery chance " +
                                        "in the satellite program.",
                RequestResult.TIMED_OUT => $"The request to change the {colorPositionLower} was made but the satellite program did not respond." +
                                           "\nMake sure the program is on.",
                _ => throw new ArgumentOutOfRangeException(nameof(requestResult), requestResult, null)
            }
        };

        mysteryChanceColorSetEmbed.Color = requestResult switch
        {
            RequestResult.FAILED => Color.Red,
            RequestResult.TIMED_OUT => Color.Orange,
            RequestResult.SUCCESSFUL => Color.DarkGreen,
            _ => throw new ArgumentOutOfRangeException(nameof(requestResult), requestResult, null)
        };

        string discordColorHex = ToHtmlHexadecimal(color);
        mysteryChanceColorSetEmbed.ImageUrl = $"https://singlecolorimage.com/get/{discordColorHex}/256x256";

        mysteryChanceColorSetEmbed.AddField(mysteryChanceColorField);
        Embed goalColorForegroundEmbed = mysteryChanceColorSetEmbed.Build();

        return goalColorForegroundEmbed;
    }

    private static string ToHtmlHexadecimal(this Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    public static Embed GenerateWrongHexCodeEmbed(string colorPosition, string userEntry)
    {
        EmbedBuilder wrongHexCodeEmbedBuilder = BuildBotEmbedBase("Goal Formatting Request");
        wrongHexCodeEmbedBuilder.Color = Color.Red;

        int userEntryLength = userEntry.Length;

        EmbedFieldBuilder wrongHexCodeField = new()
        {
            Name = $"{colorPosition} Goal Color Change Request",
            IsInline = false,
            Value = userEntryLength < 1024 
                ? $"\"{userEntry}\" is not a valid hex code for a color."
                : "That is not a valid hex code for a color."
        };

        wrongHexCodeEmbedBuilder.AddField(wrongHexCodeField);
        Embed WrongHexCodeEmbed = wrongHexCodeEmbedBuilder.Build();

        return WrongHexCodeEmbed;
    }


    public static Embed GenerateMysteryChanceValueSetEmbed(int value, RequestResult requestResult)
    {
        EmbedBuilder mysteryGoalValueSetEmbedBuilder = BuildBotEmbedBase("Mystery Goal Value Request");

        EmbedFieldBuilder mysteryGoalValueSetField = new()
        {
            Name = "Mystery Chance Value Change Request",
            IsInline = false,
            Value = requestResult switch
            {
                RequestResult.SUCCESSFUL => $"The percentage value for the mystery alert has been changed to {value.ToString()}",
                RequestResult.FAILED => $"An issue occured when requesting the change of the {value} of the mystery chance " + 
                                        "in the satellite program.",
                RequestResult.TIMED_OUT => $"The request to change the {value} was made but the satellite program did not respond." +
                                           "\nMake sure the program is on."
            }
        };
        
        mysteryGoalValueSetEmbedBuilder.Color = requestResult switch
        {
            RequestResult.FAILED => Color.Red,
            RequestResult.TIMED_OUT => Color.Orange,
            RequestResult.SUCCESSFUL => Color.DarkGreen,
            _ => throw new ArgumentOutOfRangeException(nameof(requestResult), requestResult, null)
        };

        mysteryGoalValueSetEmbedBuilder.AddField(mysteryGoalValueSetField);
        Embed mysteryGoalValueSetEmbed = mysteryGoalValueSetEmbedBuilder.Build();

        return mysteryGoalValueSetEmbed;
    }

    public static Embed GenerateGoalOfflineEmbed(RequestResult requestResult)
    {
        EmbedBuilder disableMysteryChanceEmbedBuilder = BuildBotEmbedBase("Mystery Chance Deactivation");

        EmbedFieldBuilder disableMysteryChanceField = new()
        {
            Name = "Disable Mystery Chance Request",
            IsInline = false,
            Value = requestResult switch
            {
                RequestResult.SUCCESSFUL => "The mystery chance system has been successfully disabled.",
                RequestResult.FAILED => $"An issue occured when requesting to disable the mystery chance system " + 
                                        "in the satellite program.",
                RequestResult.TIMED_OUT => $"The request to disable the mystery chance system was made but the satellite program did not respond." +
                                           "\nMake sure the program is on."
            }
        };

        disableMysteryChanceEmbedBuilder.AddField(disableMysteryChanceField);
        Embed disableMysteryChanceEmbed = disableMysteryChanceEmbedBuilder.Build();

        return disableMysteryChanceEmbed;
    }
    
    public static Embed GenerateGoalOnlineEmbed(RequestResult requestResult)
    {
        EmbedBuilder disableMysteryChanceEmbedBuilder = BuildBotEmbedBase("Mystery Chance Activation");

        EmbedFieldBuilder disableMysteryChanceField = new()
        {
            Name = "Enable Mystery Chance Request",
            IsInline = false,
            Value = requestResult switch
            {
                RequestResult.SUCCESSFUL => "The mystery chance system has been successfully enabled.",
                RequestResult.FAILED => $"An issue occured when requesting to enable the mystery chance system " + 
                                        "in the satellite program.",
                RequestResult.TIMED_OUT => $"The request to enable the mystery chance system was made but the satellite program did not respond." +
                                           "\nMake sure the program is on."
            }
        };

        disableMysteryChanceEmbedBuilder.AddField(disableMysteryChanceField);
        Embed disableMysteryChanceEmbed = disableMysteryChanceEmbedBuilder.Build();

        return disableMysteryChanceEmbed;
    }

    public static Embed GenerateMysteryChancePositionSet(RequestResult requestResult, int value, string positionAxis)
    {
        EmbedBuilder xPositionMysteryChanceEmbedBuilder = BuildBotEmbedBase($"Mystery Chance {positionAxis} Position");

        EmbedFieldBuilder xPositionMysteryChanceField = new()
        {
            Name = $"Set Mystery Chance {positionAxis} Position",
            IsInline = false,
            Value = requestResult switch
            {
                RequestResult.SUCCESSFUL => $"The mystery chance system's {positionAxis} position has been successfully set to {value.ToString()}.",
                RequestResult.FAILED => $"An issue occured when requesting to change the mystery chance's {positionAxis} position to {value}" + 
                                        "in the satellite program.",
                RequestResult.TIMED_OUT => $"The request to set the mystery chance system's {positionAxis} position to {value} was made but the satellite program did not respond." +
                                           "\nMake sure the program is on."
            }
        };

        xPositionMysteryChanceEmbedBuilder.AddField(xPositionMysteryChanceField);
        Embed xPositionMysteryChanceEmbed = xPositionMysteryChanceEmbedBuilder.Build();

        return xPositionMysteryChanceEmbed;
    }
}