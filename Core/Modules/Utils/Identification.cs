using System.Collections.Generic;
using Discord.WebSocket;
using Template.Modules.Utils.Discord;
using Template.Services;
using TranslatorBot;
using TranslatorBot.Services;

namespace Template.Modules.Utils;

public class Identification
{
    public static Bot Bot => Startup.Bot;

    public static EmailService BotMail => Startup.Email;

    public static SocketGuildUser GetServerUserById(ulong id)
    {
        bool found = TryGetServerUserById(id, out SocketGuildUser user);
        return found ? user : null;
    }

    public static bool TryGetServerUserById(ulong id, out SocketGuildUser user)
    {
        Bot discordBot = Startup.Bot;
        
        
        foreach (Server server in discordBot.Servers)
        {
            if (server.ContainsUser(id))
            {
                user = server.GetUser(id);
                return true;
            }
        }
        
        user = null;
        return false;
    }
    
    public static SocketRole GetServerRoleById(ulong id)
    {
        bool found = TryGetServerRoleById(id, out SocketRole role);
        return found ? role : null;
    }

    private static bool TryGetServerRoleById(ulong id, out SocketRole socketRole)
    {
        Bot discordBot = Startup.Bot;
        
        foreach (Server server in discordBot.Servers)
        {
            if (server.ContainsRole(id))
            {
                socketRole = server.GetRole(id);
                return true;
            }
        }
        
        socketRole = null;
        return false;
    }


    public static SocketTextChannel GetServerTextChannelById(ulong id)
    {
        bool found = TryGetServerTextChannelById(id, out SocketTextChannel channel);
        return found ? channel : null;
    }

    private static bool TryGetServerTextChannelById(ulong id, out SocketTextChannel channel)
    {
        Bot discordBot = Startup.Bot;

        foreach (Server server in discordBot.Servers)
        {
            if (server.ContainsChannel(id))
            {
                channel = server.GetChannel(id) as SocketTextChannel;
                return true;
            }
        }

        channel = null;
        return false;
    }
}