using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Template.Modules.Utils;

namespace Template.Modules.Mazzin
{
    public class AuthenticationService
    {
        private static readonly DataAssociation MazzinCommandAuthentication = new("../../../Modules/Mazzin/AuthorizedUsers.txt");
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
    }
}