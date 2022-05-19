using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Template.Modules.Mazzin
{
    public class MazzinModule : ModuleBase<SocketCommandContext>
    {
        [Command("mazzin-show-list")]
        [Summary("Show the current list of potential goals")]
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
            Goal goalToAdd = new(name, value, isActive, isCompleted);
            bool wasAdded = goalList.Add(goalToAdd);
            Embed userAddedEmbed = EmbedGenerator.GenerateGoalListAddEmbed(wasAdded);
            if (wasAdded)
                goalList.Save("../../../Modules/Mazzin/goalListData.txt");
            
            await ReplyAsync(embed: userAddedEmbed);
        }
        
        [Command("mazzin-remove-list")]
        [Summary("Remove a goal from the list")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveList(string goal)
        {
            GoalList goalList = GoalListService.GoalList;
            bool wasRemoved = goalList.Remove(goal);
            Embed userRemovedEmbed = EmbedGenerator.GenerateGoalListRemoveEmbed(wasRemoved);
            if (wasRemoved)
                goalList.Save("../../../Modules/Mazzin/goalListData.txt");
            
            await ReplyAsync(embed: userRemovedEmbed);
        }

        [Command("mazzin-remove-list")]
        [Summary("Remove a goal from the list")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveList(int value, string name, bool isActive = false, bool isCompleted = false)
        {
            GoalList goalList = GoalListService.GoalList;
            Goal goalToRemove = new(name, value, isActive, isCompleted);
            bool wasRemoved = goalList.Remove(goalToRemove);
            Embed userRemovedEmbed = EmbedGenerator.GenerateGoalListRemoveEmbed(wasRemoved);
            if (wasRemoved)
                goalList.Save("../../../Modules/Mazzin/goalListData.txt");

            await ReplyAsync(embed: userRemovedEmbed);
        }
        
        [Command("mazzin-clear-list")]
        [Summary("Clear the list of goals")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinClearList()
        {
            GoalList goalList = GoalListService.GoalList;
            goalList.Clear();
            goalList.Save("../../../Modules/Mazzin/goalListData.txt");
            Embed userClearedEmbed = EmbedGenerator.GenerateGoalListClearEmbed();
            await ReplyAsync(embed: userClearedEmbed);
        }


        [Command("mazzin-features-show-authorized")]
        [Summary("Show the people authorized to use restricted Mazzin features")]
        public async Task MazzinShowAuthorized()
        {
            (HashSet<ulong> authorizedUsers, HashSet<ulong> authorizedRoles, HashSet<ulong> authorizedChannels)
                authorized = AuthenticationService.GetAuthorizedToUseMazzinCommands();
            List<Embed> authorizedEmbed = EmbedGenerator.GenerateAuthorizedMazzinUsersEmbed(authorized);
            
            int currentEmbed = 0;
            
            foreach (Embed embed in authorizedEmbed)
            {
                await ReplyAsync(embed: embed);
                if (currentEmbed % 5 == 0)
                    Thread.Sleep(100);
                currentEmbed++;
            }
        }

        [Command("mazzin-features-add-authorized")]
        [Summary("Add a user to the list of authorized users")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddAuthorized(IGuildUser user)
        {
            ulong userId = user.Id;
            AuthorizationResult wasAdded = AuthenticationService.AddAuthorizedUser(userId);
            string userName = user.Username;
            Embed userAddedEmbed = EmbedGenerator.GenerateAuthorizedMazzinUsersAddedEmbed(wasAdded, userId, userName);
            await ReplyAsync(embed: userAddedEmbed);
        }
        
        [Command("mazzin-features-add-authorized")]
        [Summary("Add a role to the list of authorized roles")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddAuthorized(IRole role)
        {
            AuthorizationResult wasAdded = AuthenticationService.AddAuthorizedRole(role.Id);
            string roleName = role.Name;
            Embed roleAddedEmbed = EmbedGenerator.GenerateAuthorizedMazzinRolesAddedEmbed(wasAdded, role.Id, roleName);
            await ReplyAsync(embed: roleAddedEmbed);
        }
        
        [Command("mazzin-features-add-authorized")]
        [Summary("Add a channel to the list of authorized channels")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddAuthorized(ITextChannel channel)
        {
            AuthorizationResult wasAdded = AuthenticationService.AddAuthorizedChannel(channel.Id);
            string channelName = channel.Name;
            Embed channelAddedEmbed = EmbedGenerator.GenerateAuthorizedMazzinChannelsAddedEmbed(wasAdded, channel.Id, channelName);
            await ReplyAsync(embed: channelAddedEmbed);
        }

        [Command("mazzin-features-add-authorized-user")]
        [Summary("Add a user to the list of authorized users")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddAuthorizedUser(ulong userId)
        {
            AuthorizationResult wasAdded = AuthenticationService.AddAuthorizedUser(userId);
            Embed userAddedEmbed = EmbedGenerator.GenerateAuthorizedMazzinUsersAddedEmbed(wasAdded, userId);
            await ReplyAsync(embed: userAddedEmbed);
        }

        [Command("mazzin-features-add-authorized-role")]
        [Summary("Add a role to the list of authorized roles")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddAuthorizedRole(ulong roleId)
        {
            AuthorizationResult wasAdded = AuthenticationService.AddAuthorizedRole(roleId);
            Embed roleAddedEmbed = EmbedGenerator.GenerateAuthorizedMazzinRolesAddedEmbed(wasAdded, roleId);
            await ReplyAsync(embed: roleAddedEmbed);
        }

        [Command("mazzin-features-add-authorized-channel")]
        [Summary("Add a channel to the list of authorized channels")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinAddAuthorizedChannel(ulong channelId)
        {
            AuthorizationResult wasAdded = AuthenticationService.AddAuthorizedChannel(channelId);
            Embed channelAddedEmbed = EmbedGenerator.GenerateAuthorizedMazzinChannelsAddedEmbed(wasAdded, channelId);
            await ReplyAsync(embed: channelAddedEmbed);
        }
        
        [Command("mazzin-features-remove-authorized")]
        [Summary("Remove a user from the list of authorized users")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveAuthorized(IGuildUser user)
        {
            ulong userId = user.Id;
            AuthorizationResult wasRemoved = AuthenticationService.RemoveAuthorizedUser(userId);
            string userName = user.Username;
            Embed userRemovedEmbed =
                EmbedGenerator.GenerateAuthorizedMazzinUsersRemovedEmbed(wasRemoved, userId, userName);
            await ReplyAsync(embed: userRemovedEmbed);
        }
        
        [Command("mazzin-features-remove-authorized")]
        [Summary("Remove a role from the list of authorized roles")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveAuthorized(IRole role)
        {
            AuthorizationResult wasRemoved = AuthenticationService.RemoveAuthorizedRole(role.Id);
            string roleName = role.Name;
            Embed userRemovedEmbed = EmbedGenerator.GenerateAuthorizedMazzinRolesRemovedEmbed(wasRemoved, role.Id, roleName);
            await ReplyAsync(embed: userRemovedEmbed);
        }
        
        [Command("mazzin-features-remove-authorized")]
        [Summary("Remove a channel from the list of authorized channels")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveAuthorized(ITextChannel channel)
        {
            AuthorizationResult wasRemoved =
                AuthenticationService.RemoveAuthorizedChannel(channel.Id);
            string channelName = channel.Name;
            Embed userRemovedEmbed = EmbedGenerator.GenerateAuthorizedMazzinChannelsRemovedEmbed(wasRemoved, channel.Id, channelName);
            await ReplyAsync(embed: userRemovedEmbed);
        }

        [Command("mazzin-features-remove-authorized")]
        [Summary("Remove a user from the list of authorized users")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveAuthorizedUser(ulong userId)
        {
            AuthorizationResult wasRemoved = AuthenticationService.RemoveAuthorizedUser(userId);
            Embed userRemovedEmbed = EmbedGenerator.GenerateAuthorizedMazzinUsersRemovedEmbed(wasRemoved, userId);
            await ReplyAsync(embed: userRemovedEmbed);
        }
        
        [Command("mazzin-features-remove-authorized-role")]
        [Summary("Remove a role from the list of authorized roles")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveAuthorizedRole(ulong roleId)
        {
            AuthorizationResult wasRemoved = AuthenticationService.RemoveAuthorizedRole(roleId);
            Embed userRemovedEmbed = EmbedGenerator.GenerateAuthorizedMazzinRolesRemovedEmbed(wasRemoved, roleId);
            await ReplyAsync(embed: userRemovedEmbed);
        }

        [Command("mazzin-features-remove-authorized-channel")]
        [Summary("Remove a channel from the list of authorized channels")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinRemoveAuthorizedChannel(ulong channelId)
        {
            AuthorizationResult wasRemoved =
                AuthenticationService.RemoveAuthorizedChannel(channelId);
            Embed userRemovedEmbed = EmbedGenerator.GenerateAuthorizedMazzinChannelsRemovedEmbed(wasRemoved, channelId);
            await ReplyAsync(embed: userRemovedEmbed);
        }

        [Command("mazzin-features-clear-authorized")]
        [Summary("Clear the list of authorized users")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinClearAuthorized()
        {
            AuthenticationService.ClearAuthorizedUsers();
            Embed userClearedEmbed = EmbedGenerator.GenerateAuthorizedMazzinClearedEmbed();
            await ReplyAsync(embed: userClearedEmbed);
        }

        [Command("mazzin-obs-set-mystery-alert-color")]
        [Alias("mazzin-obs-set-mystery-alert-color-foreground")]
        [Summary("Set the color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetForegroundColor(int r, int g, int b)
        {
            Color discordColor = new(r, g, b);
            RequestResult requestResult = await SatelliteCommunicationService.SetColorForeground(r, g, b);
            Embed colorSetEmbed = EmbedGenerator.GenerateMysteryChanceColorSetEmbed(discordColor, requestResult, "Foreground Color");
            await ReplyAsync(embed: colorSetEmbed);
            
            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
                obsData.MysteryChanceForegroundColor = color;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-color")]
        [Alias("mazzin-obs-set-mystery-alert-color-foreground")]
        [Summary("Set the color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetForegroundColor(string hexCode)
        {
            await SetupNewColor(hexCode, SatelliteCommunicationService.SetColorForeground, "Foreground Color");
        }

        [Command("mazzin-obs-set-mystery-alert-color-foreground-contour")]
        [Summary("Set the contour color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetForegroundContourColor(int r, int g, int b)
        {
            Color discordColor = new(r, g, b);
            RequestResult requestResult = await SatelliteCommunicationService.SetColorForeground(r, g, b);
            Embed colorSetEmbed = EmbedGenerator.GenerateMysteryChanceColorSetEmbed(discordColor, requestResult, "Foreground Contour Color");
            await ReplyAsync(embed: colorSetEmbed);
            
            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
                obsData.MysteryChanceForegroundColorContour = color;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-color-foreground-contour")]
        [Summary("Set the contour color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetForegroundContourColor(string hexCode)
        {
            (RequestResult? requestResult, int r, int g, int b, int a) = await SetupNewColor(hexCode, 
                SatelliteCommunicationService.SetColorForegroundContour, "Foreground Contour Color");
            
            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(a, r, g, b);
                obsData.MysteryChanceForegroundColorContour = color;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-color-background")]
        [Summary("Set the background color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetBackgroundColor(int r, int g, int b)
        {
            Color discordColor = new(r, g, b);
            RequestResult requestResult = await SatelliteCommunicationService.SetColorForeground(r, g, b);
            Embed colorSetEmbed = EmbedGenerator.GenerateMysteryChanceColorSetEmbed(discordColor, requestResult, "Background Color");
            await ReplyAsync(embed: colorSetEmbed);
            
            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
                obsData.MysteryChanceBackgroundColor = color;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-color-background")]
        [Summary("Set the background color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetBackgroundColor(string hexCode)
        {
            (RequestResult? requestResult, int r, int g, int b, int a) = await SetupNewColor(hexCode, 
                SatelliteCommunicationService.SetColorBackground, "Background Color");
            
            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(a, r, g, b);
                obsData.MysteryChanceBackgroundColor = color;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-color-background")]
        [Summary("Set the background contour color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetBackgroundContourColor(int r, int g, int b)
        {
            Color discordColor = new(r, g, b);
            RequestResult requestResult = await SatelliteCommunicationService.SetColorForeground(r, g, b);
            Embed colorSetEmbed = EmbedGenerator.GenerateMysteryChanceColorSetEmbed(discordColor, requestResult, "Background Contour Color");
            await ReplyAsync(embed: colorSetEmbed);
            
            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
                obsData.MysteryChanceBackgroundColorContour = color;
            }
        }
        
        [Command("mazzin-obs-set-goal-color-background-contour")]
        [Summary("Set the background contour color of the mystery alert's percentage value")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetBackgroundContourColor(string hexCode)
        {
            (RequestResult? requestResult, int r, int g, int b, int a) = await SetupNewColor(hexCode, 
                SatelliteCommunicationService.SetColorForeground, "Background Contour Color");
            
            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(a, r, g, b);
                obsData.MysteryChanceBackgroundColorContour = color;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-value")]
        [Summary("Set the percentage chance for a mystery goal")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetGoalValue(int value)
        {
            RequestResult requestResult = await SatelliteCommunicationService.SetValue(value);
            Embed colorSetEmbed = EmbedGenerator.GenerateMysteryChanceValueSetEmbed(value, requestResult);
            await ReplyAsync(embed: colorSetEmbed);

            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                obsData.MysteryChancePercentage = value;
            }
        }

        [Command("disable-mazzin-obs-set-mystery-alert")]
        [Summary("Turn the mystery goal off")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetGoalOffline()
        {
            RequestResult requestResult = await SatelliteCommunicationService.SetOffline();
            Embed colorSetEmbed = EmbedGenerator.GenerateGoalOfflineEmbed(requestResult);
            await ReplyAsync(embed: colorSetEmbed);

            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                obsData.MysteryChanceOnline = false;
            }
        }
        
        [Command("enable-mazzin-obs-set-mystery-alert")]
        [Summary("Turn the mystery goal on")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetGoalOn()
        {
            RequestResult requestResult = await SatelliteCommunicationService.SetOnline();
            Embed colorSetEmbed = EmbedGenerator.GenerateGoalOnlineEmbed(requestResult);
            await ReplyAsync(embed: colorSetEmbed);

            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                obsData.MysteryChanceOnline = true;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-x-position")]
        [Summary("Turn the mystery goal on")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetGoalXPosition(uint value)
        {
            RequestResult requestResult = await SatelliteCommunicationService.SetXPosition(value);
            Embed colorSetEmbed = EmbedGenerator.GenerateMysteryChancePositionSet(requestResult, (int) value, "X");
            await ReplyAsync(embed: colorSetEmbed);

            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                obsData.MysteryXPosition = (int) value;
            }
        }
        
        [Command("mazzin-obs-set-mystery-alert-y-position")]
        [Summary("Turn the mystery goal on")]
        [IsAuthorizedToUseMazzinFeature]
        public async Task MazzinSetGoalYPosition(uint value)
        {
            RequestResult requestResult = await SatelliteCommunicationService.SetYPosition(value);
            Embed colorSetEmbed = EmbedGenerator.GenerateMysteryChancePositionSet(requestResult, (int) value, "Y");
            await ReplyAsync(embed: colorSetEmbed);

            if (requestResult == RequestResult.SUCCESSFUL)
            {
                ObsData obsData = ObsDataService.ObsData;
                obsData.MysteryXPosition = (int) value;
            }
        }
        
        private async Task<(RequestResult?, int, int, int, int)> SetupNewColor(string hexCode, Func<int, int, int, int, Task<RequestResult>> colorSetter, string colorPosition)
        {
            System.Drawing.Color? color = Utils.FromHtmlHexadecimal(hexCode);
            if (color == null)
            {
                Embed wrongHexCodeEmbed = EmbedGenerator.GenerateWrongHexCodeEmbed(colorPosition, hexCode);
                await ReplyAsync(embed: wrongHexCodeEmbed);
                return (null, 0, 0, 0, 0);
            }

            System.Drawing.Color value = color.Value;
            int red = value.R;
            int green = value.G;
            int blue = value.B;
            int alpha = value.A;
            Color discordColor = new(red, green, blue);

            RequestResult requestResult = await colorSetter.Invoke(red, green, blue, alpha);
            Embed colorSetEmbed =
                EmbedGenerator.GenerateMysteryChanceColorSetEmbed(discordColor, requestResult, colorPosition);
            await ReplyAsync(embed: colorSetEmbed);
            return (requestResult, red, green, blue, alpha);
        }
    }
}