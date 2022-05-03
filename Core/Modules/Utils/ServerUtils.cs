using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace Template.Modules.Utils;

public static class ServerUtils
{
    internal static bool ContainsTextChannelInCategory(SocketGuild server, SocketCategoryChannel category, string textChannelLookup)
    {
        List<SocketTextChannel> textChannels = GetTextChannelsInCategory(server, category);
        return textChannels.Any(textChannel => textChannel.Name == textChannelLookup);
    }
    
    internal static bool ContainsTextChannel(SocketGuild server, string textChannelLookup)
    {
        List<string> textChannels = GetServerTextChannelNames(server);
        return textChannels.Contains(textChannelLookup);
    }
    
    internal static bool ContainsVoiceChannel(SocketGuild server, string voiceChannelLookup)
    {
        List<string> voiceChannels = GetServerTextChannelNames(server);
        return voiceChannels.Contains(voiceChannelLookup);
    }
    
    internal static bool ContainsCategoryChannel(SocketGuild server, string categoryChannelLookup)
    {
        List<string> categoryChannels = GetServerCategoryChannelNames(server);
        return categoryChannels.Contains(categoryChannelLookup);
    }
    
    internal static List<string> GetServerTextChannelNamesInCategory(SocketGuild server, SocketCategoryChannel categoryChannel)
    {
        List<SocketTextChannel> textChannels = GetTextChannelsInCategory(server, categoryChannel);
        return textChannels.Select(textChannel => textChannel.Name).ToList();
    }
    
    internal static List<string> GetServerVoiceChannelNamesInCategory(SocketGuild server, SocketCategoryChannel categoryChannel)
    {
        List<SocketVoiceChannel> voiceChannels = GetVoiceChannelsInCategory(server, categoryChannel);
        return voiceChannels.Select(textChannel => textChannel.Name).ToList();
    }
    
    internal static List<string> GetServerTextChannelNames(SocketGuild server)
    {
        List<SocketTextChannel> textChannelsRaw = GetTextChannels(server);
        IEnumerable<string> textChannelsEnumerable = textChannelsRaw.Select(textChannel => textChannel.Name);
        List<string> textChannels = textChannelsEnumerable.ToList();
        return textChannels;
    }
    
    internal static List<string> GetServerVoiceChannelNames(SocketGuild server)
    {
        List<SocketVoiceChannel> voiceChannelsRaw = GetVoiceChannels(server);
        IEnumerable<string> voiceChannelsEnumerable = voiceChannelsRaw.Select(textChannel => textChannel.Name);
        List<string> voiceChannels = voiceChannelsEnumerable.ToList();
        return voiceChannels;
    }
    
    internal static List<string> GetServerCategoryChannelNames(SocketGuild server)
    {
        List<SocketCategoryChannel> categoryChannelsRaw = GetCategoryChannels(server);
        IEnumerable<string> categoryChannelsEnumerable = categoryChannelsRaw.Select(textChannel => textChannel.Name);
        List<string> categoryChannels = categoryChannelsEnumerable.ToList();
        return categoryChannels;
    }

    internal static List<SocketTextChannel> GetTextChannels(SocketGuild server)
    {
        IReadOnlyCollection<SocketTextChannel> channelsReadOnlyCollection = server.TextChannels;
        return channelsReadOnlyCollection.ToList();
    }
    
    internal static List<SocketTextChannel> GetTextChannelsInCategory(SocketGuild server, SocketCategoryChannel categoryChannel)
    {
        List<SocketTextChannel> channelsReadOnlyCollection = GetTextChannels(server);
        List<SocketTextChannel> channelsInCategory = channelsReadOnlyCollection.Where(channel => channel.Category == categoryChannel).ToList();
        return channelsInCategory;
    }
    
    internal static List<SocketVoiceChannel> GetVoiceChannels(SocketGuild server)
    {
        IReadOnlyCollection<SocketVoiceChannel> channelsReadOnlyCollection = server.VoiceChannels;
        return channelsReadOnlyCollection.ToList();
    }
    
    internal static List<SocketVoiceChannel> GetVoiceChannelsInCategory(SocketGuild server, SocketCategoryChannel categoryChannel)
    {
        List<SocketVoiceChannel> channelsReadOnlyCollection = GetVoiceChannels(server);
        List<SocketVoiceChannel> channelsInCategory = channelsReadOnlyCollection.Where(channel => channel.Category == categoryChannel).ToList();
        return channelsInCategory;
    }
    
    internal static List<SocketCategoryChannel> GetCategoryChannels(SocketGuild server)
    {
        IReadOnlyCollection<SocketCategoryChannel> channelsReadOnlyCollection = server.CategoryChannels;
        return channelsReadOnlyCollection.ToList();
    }

    internal static SocketTextChannel GetDefaultChannel(SocketGuild server)
    {
        return server.DefaultChannel;
    }
    
    internal static SocketGuildChannel GetEmbedChannel(SocketGuild server)
    {
        return server.EmbedChannel;
    }

    internal static int NumberOfUsersInServer(SocketGuild server)
    {
        return server.MemberCount;
    }

    internal static int AfkSecondsInServer(SocketGuild server)
    {
        return server.AFKTimeout;
    }

    internal static ulong GetServerId(SocketGuild server)
    {
        return server.Id;
    }
}