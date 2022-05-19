using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using Template.Modules.Utils;

namespace Template.Modules.Mazzin
{
    public static class AuthenticationService
    {
        private static readonly DataAssociation MazzinCommandAuthentication = DataAssociation.FromFile("../../../Modules/Mazzin/AuthorizedUsers.txt");
        public static bool IsAuthorizedToUseMazzinCommands(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            HashSet<ulong> authorizedUsers = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Users");
            HashSet<ulong> authorizedRoles = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Roles");
            HashSet<ulong> authorizedChannels = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Channels");
            
            SocketUser user = context.User as SocketUser;
            ulong userId = user.Id;
            SocketChannel channel = context.Channel as SocketChannel;
            ulong channelId = channel.Id;
            if (authorizedUsers.Contains(userId))
            {
                return true;
            }
            
            if (authorizedChannels.Contains(channelId))
            {
                return true;
            }

            if (context.User is not SocketGuildUser serverMember) return false;
            
            IReadOnlyCollection<SocketRole> memberRoles = serverMember.Roles;
            return memberRoles.Any(role => authorizedRoles.Contains(role.Id));

        }

        public static (HashSet<ulong> authorizedUsers, HashSet<ulong> authorizedRoles, HashSet<ulong> authorizedChannels
            ) GetAuthorizedToUseMazzinCommands()
        {
            HashSet<ulong> authorizedUsers = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Users");
            HashSet<ulong> authorizedRoles = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Roles");
            HashSet<ulong> authorizedChannels = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Channels");

            (HashSet<ulong> authorizedUsers, HashSet<ulong> authorizedRoles, HashSet<ulong> authorizedChannels)
                authorized = (authorizedUsers, authorizedRoles, authorizedChannels);
            return authorized;
        }

        private static HashSet<ulong> FetchAuthorizationData(DataAssociation data, string propertyName)
        {
            bool wasFound = data.TryGetValue(propertyName, out string fetchedDataStr);
            HashSet<ulong> fetchedData = new HashSet<ulong>();
            if (!wasFound) return fetchedData;
        
            string[] fetchedDataElements = fetchedDataStr.Split(',');
            foreach (string idStr in fetchedDataElements)
            {
                ulong id = ulong.Parse(idStr);
                fetchedData.Add(id);
            }

            return fetchedData;
        }

        public static AuthorizationResult AddAuthorizedUser(ulong userId)
        {
            HashSet<ulong> authorizedUsers = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Users");
            authorizedUsers.Add(userId);
            string newListOfUsers = string.Join(",", authorizedUsers);
            bool wasAdded = MazzinCommandAuthentication.SetValue("Authorized Users", newListOfUsers);

            MazzinCommandAuthentication.Save();
            
            if (wasAdded)
            { 
                return AuthorizationResult.SUCCESS;
            }

            if (authorizedUsers.Contains(userId))
            {
                return AuthorizationResult.ALREADY_PRESENT;
            }
            
            return AuthorizationResult.FAILED;
        }

        public static AuthorizationResult RemoveAuthorizedUser(ulong userId)
        {
            HashSet<ulong> authorizedUsers = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Users");
            authorizedUsers.Remove(userId);
            string newListOfUsers = string.Join(",", authorizedUsers);
            bool wasRemoved = MazzinCommandAuthentication.SetValue("Authorized Users", newListOfUsers);

            if (!authorizedUsers.Contains(userId))
            {
                return AuthorizationResult.ALREADY_PRESENT;
            }

            MazzinCommandAuthentication.Save();
            
            if (wasRemoved)
            { 
                return AuthorizationResult.SUCCESS;
            }
            
            return AuthorizationResult.FAILED;
        }
        
        public static AuthorizationResult AddAuthorizedRole(ulong roleId)
        {
            HashSet<ulong> authorizedRoles = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Roles");
            authorizedRoles.Add(roleId);
            string newListOfRoles = string.Join(",", authorizedRoles);
            bool wasAdded = MazzinCommandAuthentication.SetValue("Authorized Roles", newListOfRoles);
        
            MazzinCommandAuthentication.Save();
        
            if (wasAdded)
            { 
                return AuthorizationResult.SUCCESS;
            }
        
            if (authorizedRoles.Contains(roleId))
            {
                return AuthorizationResult.ALREADY_PRESENT;
            }
        
            return AuthorizationResult.FAILED;
        }

        public static AuthorizationResult RemoveAuthorizedRole(ulong roleId)
        {
            HashSet<ulong> authorizedRoles = FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Roles");
            authorizedRoles.Remove(roleId);
            string newListOfRoles = string.Join(",", authorizedRoles);
            bool wasRemoved = MazzinCommandAuthentication.SetValue("Authorized Roles", newListOfRoles);

            if (!authorizedRoles.Contains(roleId))
            {
                return AuthorizationResult.ALREADY_PRESENT;
            }

            MazzinCommandAuthentication.Save();

            if (wasRemoved)
            {
                return AuthorizationResult.SUCCESS;
            }

            return AuthorizationResult.FAILED;
        }

        public static AuthorizationResult AddAuthorizedChannel(ulong channelId)
        {
            HashSet<ulong> authorizedChannels =
                FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Channels");
            authorizedChannels.Add(channelId);
            string newListOfChannels = string.Join(",", authorizedChannels);
            bool wasAdded = MazzinCommandAuthentication.SetValue("Authorized Channels", newListOfChannels);

            MazzinCommandAuthentication.Save();

            if (wasAdded)
            {
                return AuthorizationResult.SUCCESS;
            }

            if (authorizedChannels.Contains(channelId))
            {
                return AuthorizationResult.ALREADY_PRESENT;
            }

            return AuthorizationResult.FAILED;
        }

        public static AuthorizationResult RemoveAuthorizedChannel(ulong channelId)
        {
            HashSet<ulong> authorizedChannels =
                FetchAuthorizationData(MazzinCommandAuthentication, "Authorized Channels");
            authorizedChannels.Remove(channelId);
            string newListOfChannels = string.Join(",", authorizedChannels);
            bool wasRemoved = MazzinCommandAuthentication.SetValue("Authorized Channels", newListOfChannels);

            if (!authorizedChannels.Contains(channelId))
            {
                return AuthorizationResult.ALREADY_PRESENT;
            }

            MazzinCommandAuthentication.Save();

            if (wasRemoved)
            {
                return AuthorizationResult.SUCCESS;
            }

            return AuthorizationResult.FAILED;
        }

        public static void ClearAuthorizedUsers()
        {
            MazzinCommandAuthentication.SetValue("Authorized Users", "");
            MazzinCommandAuthentication.Save();
        }
        
        public static void ClearAuthorizedRoles()
        {
            MazzinCommandAuthentication.SetValue("Authorized Roles", "");
            MazzinCommandAuthentication.Save();
        }
        
        public static void ClearAuthorizedChannels()
        {
            MazzinCommandAuthentication.SetValue("Authorized Channels", "");
            MazzinCommandAuthentication.Save();
        }
        
        public static void ClearAllAuthorized()
        {
            MazzinCommandAuthentication.SetValue("Authorized Users", "");
            MazzinCommandAuthentication.SetValue("Authorized Roles", "");
            MazzinCommandAuthentication.SetValue("Authorized Channels", "");
            MazzinCommandAuthentication.Save();
        }
    }
}