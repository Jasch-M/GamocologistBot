using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Template.Modules.Utils.Discord;

/// <summary>
/// Represents the bot.
/// </summary>
public class Bot
{
    /// <summary>
    /// The bot's (<see cref="DiscordSocketClient"/>) discord client.
    /// This is the main interface to the discord API.
    /// </summary>
    private readonly DiscordSocketClient _botClient;

    /// <summary>
    /// A Dictionary of all the <see cref="Server"/> servers the bot is on.
    /// The server's key is the server's ID.
    /// These servers are the bot's internal representation of the servers the bot is on.
    /// </summary>
    internal Dictionary<ulong, Server> ServerLookup;

    /// <summary>
    /// A Dictionary of all the <see cref="SocketGuild"/> servers the bot is on.
    /// The server's key is the server's ID.
    /// These servers are the discord API's representation of the servers the bot is on.
    /// </summary>
    internal Dictionary<ulong, SocketGuild> ServerDiscordLookup;

    /// <summary>
    /// A Dictionary of all the <see cref="SocketDMChannel"/> direct message channels the bot is on.
    /// The channel's key is the channel's ID.
    /// </summary>
    internal Dictionary<ulong, SocketDMChannel> DmChannelLookup;

    /// <summary>
    /// A Dictionary of all the <see cref="SocketGroupChannel"/> group channels the bot is on.
    /// The channel's key is the channel's ID.
    /// </summary>
    internal Dictionary<ulong, SocketGroupChannel> GroupChannelLookup;

    /// <summary>
    /// A Dictionary of all the <see cref="ISocketPrivateChannel"/> private channels the bot is on.
    /// The channel's key is the channel's ID.
    /// </summary>
    internal Dictionary<ulong, ISocketPrivateChannel> PrivateChannelLookup;

    /// <summary>
    /// A Dictionary of all the <see cref="RestVoiceRegion"/> voice channels the bot is on.
    /// The channel's key is the channel's ID.
    /// </summary>
    internal Dictionary<string, RestVoiceRegion> VoiceRegionLookup;

    /// <summary>
    /// The number of servers the bot is on.
    /// </summary>
    internal int ServerCount;

    /// <summary>
    /// A Hashset of all the IDs of the servers the bot is on.
    /// The ID is represented as a <see cref="long"/>.
    /// </summary>
    internal HashSet<ulong> ServersIds;

    /// <summary>
    /// A Hashset of all the IDs of the direct message channels the bot is on.
    /// The ID is represented as a <see cref="long"/>.
    /// </summary>
    internal HashSet<ulong> DmChannelsIds;
    
    /// <summary>
    /// The number of direct message channels the bot is on.
    /// </summary>
    internal int DmChannelCount;

    /// <summary>
    /// A Hashset of all the IDs of the group channels the bot is on.
    /// The ID is represented as a <see cref="long"/>.
    /// </summary>
    internal HashSet<ulong> GroupChannelsIds;
    
    /// <summary>
    /// The number of group channels the bot is on.
    /// </summary>
    internal int GroupChannelCount;

    /// <summary>
    /// A Hashset of all the IDs of the private channels the bot is on.
    /// The ID is represented as a <see cref="long"/>.
    /// </summary>
    internal HashSet<ulong> PrivateChannelsIds;
    
    /// <summary>
    /// The number of private channels the bot is on.
    /// </summary>
    internal int PrivateChannelCount;

    /// <summary>
    /// A Hashset of all the IDs of the voice channels the bot is on.
    /// The ID is represented as a <see cref="string"/>.
    /// </summary>
    internal HashSet<string> VoiceRegionsIds;
    
    /// <summary>
    /// The number of voice channels the bot is on.
    /// </summary>
    internal int VoiceRegionCount;

    /// <summary>
    /// A Hashset of all the <see cref="Server"/> servers the bot is on.
    /// This is the bot's internal representation of the servers the bot is on.
    /// </summary>
    internal HashSet<Server> Servers;

    /// <summary>
    /// A Hashset of all the <see cref="SocketGuild"/> servers the bot is on.
    /// This is the discord API's representation of the servers the bot is on.
    /// </summary>
    internal HashSet<SocketGuild> ServersDiscord;

    /// <summary>
    /// A Hashset of all the <see cref="SocketDMChannel"/> direct message channels the bot is on.
    /// </summary>
    internal HashSet<SocketDMChannel> DmChannels;

    /// <summary>
    /// A Hashset of all the <see cref="SocketGroupChannel"/> group channels the bot is on.
    /// </summary>
    internal HashSet<SocketGroupChannel> GroupChannels;

    /// <summary>
    /// A Hashset of all the <see cref="ISocketPrivateChannel"/> private channels the bot is on.
    /// </summary>
    internal HashSet<ISocketPrivateChannel> PrivateChannels;

    /// <summary>
    /// A Hashset of all the <see cref="RestVoiceRegion"/> voice channels the bot is on.
    /// </summary>
    internal HashSet<RestVoiceRegion> VoiceRegions;

    /// <summary>
    /// The bot's constructor.
    /// Creates the bot's discord client and initializes the bot's internal data structures
    /// from the discord API's representation of the same data.
    /// </summary>
    /// <param name="botClient">The bot client which is the representation of the server from discord's API</param>
    internal Bot(DiscordSocketClient botClient)
    {
        _botClient = botClient;
        ServerLookup = new Dictionary<ulong, Server>();
        ServerDiscordLookup = new Dictionary<ulong, SocketGuild>();
        ServersIds = new HashSet<ulong>();
        Servers = new HashSet<Server>();
        ServersDiscord = new HashSet<SocketGuild>();
        InitializeElements();
        Load();
    }

    /// <summary>
    /// Initializes the bot's internal data structures without any data.
    /// </summary>
    private void InitializeElements()
    {
        ServerLookup = new Dictionary<ulong, Server>();
        ServerDiscordLookup = new Dictionary<ulong, SocketGuild>();
        DmChannelLookup = new Dictionary<ulong, SocketDMChannel>();
        GroupChannelLookup = new Dictionary<ulong, SocketGroupChannel>();
        PrivateChannelLookup = new Dictionary<ulong, ISocketPrivateChannel>();
        VoiceRegionLookup = new Dictionary<string, RestVoiceRegion>();

        ServersIds = new HashSet<ulong>();
        DmChannelsIds = new HashSet<ulong>();
        GroupChannelsIds = new HashSet<ulong>();
        PrivateChannelsIds = new HashSet<ulong>();
        VoiceRegionsIds = new HashSet<string>();

        Servers = new HashSet<Server>();
        ServersDiscord = new HashSet<SocketGuild>();
        DmChannels = new HashSet<SocketDMChannel>();
        GroupChannels = new HashSet<SocketGroupChannel>();
        PrivateChannels = new HashSet<ISocketPrivateChannel>();
        VoiceRegions = new HashSet<RestVoiceRegion>();

        ServerCount = 0;
        DmChannelCount = 0;
        GroupChannelCount = 0;
        PrivateChannelCount = 0;
        VoiceRegionCount = 0;
    }

    /// <summary>
    /// Loads the bot's internal data structures from the discord API's representation.
    /// Loads all the data from the discord API's representation of the bot's data.
    /// </summary>
    private void Load()
    {
        LoadServers();
        LoadDmChannels();
        LoadGroupChannels();
        LoadPrivateChannels();
        LoadVoiceRegions();
    }

    /// <summary>
    /// Loads the bot's internal data structures from the discord API's representation.
    /// Loads the server data from the discord API's representation of the bot's data.
    /// </summary>
    private void LoadServers()
    {
        IReadOnlyCollection<SocketGuild> servers = _botClient.Guilds;

        foreach (SocketGuild serverDiscord in servers)
        {
            ulong serverId = serverDiscord.Id;

            ServersIds.Add(serverId);
            ServersDiscord.Add(serverDiscord);
            ServerDiscordLookup.Add(serverId, serverDiscord);

            Server server = new Server(serverDiscord);
            ServerLookup.Add(serverId, server);
            Servers.Add(server);

            ServerCount++;
        }
    }

    /// <summary>
    /// Generic method to load the bot's internal data structures from the discord API's representation.
    /// </summary>
    /// <param name="channelAccessor">
    /// A function which takes as input the client and outputs an <see cref="IReadOnlyCollection{T}"/>
    /// of the channels the bot is on.
    /// </param>
    /// <param name="channelIdAccessor">
    /// A function which takes a channel as input and outputs its ID.
    /// </param>
    /// <param name="channelsSet">
    /// The HashSet of channels to fill.
    /// </param>
    /// <param name="channelsIds">
    /// The HashSet of channel IDs to fill.
    /// </param>
    /// <param name="channelsLookup">
    /// The Dictionary corresponding channel IDS to the channels to fill.
    /// </param>
    /// <typeparam name="TChannel">
    /// The channel's type
    /// </typeparam>
    /// <returns>
    /// The number of channels found.
    /// </returns>
    private int LoadChannels<TChannel>(Func<DiscordSocketClient, IReadOnlyCollection<TChannel>> channelAccessor,
        Func<TChannel, ulong> channelIdAccessor, HashSet<TChannel> channelsSet, HashSet<ulong> channelsIds,
        Dictionary<ulong, TChannel> channelsLookup)
    {
        IReadOnlyCollection<TChannel> channels = channelAccessor.Invoke(_botClient);
        int channelCount = 0;
        
        foreach (TChannel channel in channels)
        {
            ulong channelId = channelIdAccessor.Invoke(channel);
            channelsSet.Add(channel);
            channelsIds.Add(channelId);
            channelsLookup.Add(channelId, channel);
            channelCount++;
        }

        return channelCount;
    }

    /// <summary>
    /// Loads the bot's internal data structures from the discord API's representation.
    /// Loads the direct message channel data from the discord API's representation of the bot's data.
    /// </summary>
    private void LoadDmChannels()
    {
        Func<DiscordSocketClient, IReadOnlyCollection<SocketDMChannel>> GetDmChannels = (user) => user.DMChannels;
        Func<SocketDMChannel, ulong> GetDmChannel = (channel => channel.Id);

        DmChannelCount = LoadChannels(GetDmChannels, GetDmChannel, DmChannels, DmChannelsIds, DmChannelLookup);
    }

    /// <summary>
    /// Loads the bot's internal data structures from the discord API's representation.
    /// Loads the group channel data from the discord API's representation of the bot's data.
    /// </summary>
    private void LoadGroupChannels()
    {
        Func<DiscordSocketClient, IReadOnlyCollection<SocketGroupChannel>> GetGroupChannels =
            (user) => user.GroupChannels;
        Func<SocketGroupChannel, ulong> GetGroupChannel = (channel => channel.Id);

        GroupChannelCount = LoadChannels(GetGroupChannels, GetGroupChannel, GroupChannels, GroupChannelsIds, GroupChannelLookup);
    }

    /// <summary>
    /// Loads the bot's internal data structures from the discord API's representation.
    /// Loads the private channel data from the discord API's representation of the bot's data.
    /// </summary>
    private void LoadPrivateChannels()
    {
        Func<DiscordSocketClient, IReadOnlyCollection<ISocketPrivateChannel>> GetPrivateChannels =
            (user) => user.PrivateChannels;
        Func<ISocketPrivateChannel, ulong> GetPrivateChannel = (channel => channel.Id);

        PrivateChannelCount = LoadChannels(GetPrivateChannels, GetPrivateChannel, PrivateChannels, PrivateChannelsIds, PrivateChannelLookup);
    }

    /// <summary>
    /// Loads the bot's internal data structures from the discord API's representation.
    /// Loads the voice region data from the discord API's representation of the bot's data.
    /// </summary>
    private void LoadVoiceRegions()
    {
        IReadOnlyCollection<RestVoiceRegion> channels = _botClient.VoiceRegions;
        int channelCount = 0;

        foreach (RestVoiceRegion region in channels)
        {
            string channelId = region.Id;
            VoiceRegionsIds.Add(channelId);
            VoiceRegions.Add(region);
            VoiceRegionLookup.Add(channelId, region);
            channelCount++;
        }
        
        VoiceRegionCount = channelCount;
    }

    /// <summary>
    /// The bot's current set activity.
    /// </summary>
    internal IActivity Activity => _botClient.Activity;

    /// <summary>
    /// The latency of the bot's connection to the discord API.
    /// The latency is represented in milliseconds.
    /// </summary>
    internal int Latency => _botClient.Latency;

    /// <summary>
    /// The bot's current connection status.
    /// This is a <see cref="ConnectionState"/> enum value.
    /// It can be one of the following:
    /// Connecting, Connected, Disconnecting, Disconnected.
    /// </summary>
    internal ConnectionState ConnectionState => _botClient.ConnectionState;

    /// <summary>
    /// Checks if the bot is connected to the discord API.
    /// True if the bot is connected.
    /// False if the bot is not connected.
    /// </summary>
    internal bool IsConnected => ConnectionState == ConnectionState.Connected;

    /// <summary>
    /// Checks if the bot is connecting to the discord API.
    /// True if the bot is connecting.
    /// False if the bot is not connecting.
    /// </summary>
    internal bool IsConnecting => ConnectionState == ConnectionState.Connecting;

    /// <summary>
    /// Checks if the bot is disconnected from the discord API.
    /// True if the bot is disconnected.
    /// False if the bot is not disconnected.
    /// </summary>
    internal bool IsDisconnected => ConnectionState == ConnectionState.Disconnected;

    /// <summary>
    /// Checks if the bot is disconnecting from the discord API.
    /// True if the bot is disconnecting.
    /// False if the bot is not disconnecting.
    /// </summary>
    internal bool IsDisconnecting => ConnectionState == ConnectionState.Disconnecting;

    /// <summary>
    /// The bot's <see cref="UserStatus"/> status.
    /// It can be one of the following:
    /// Idle, DoNotDisturb, AFK, Invisible, Online or Offline.
    /// </summary>
    internal UserStatus Status => _botClient.Status;

    /// <summary>
    /// Checks if the bot is idle.
    /// True if the bot is idle.
    /// False if the bot is not idle.
    /// </summary>
    internal bool IsIdle => Status == UserStatus.Idle;

    /// <summary>
    /// Check if the bot is set to do not disturb.
    /// True if the bot is set to do not disturb.
    /// False if the bot is not set to do not disturb.
    /// </summary>
    internal bool IsDoNotDisturb => Status == UserStatus.DoNotDisturb;

    /// <summary>
    /// Checks if the bot is set to be AFK (away from keyboard).
    /// True if the bot is set to be AFK.
    /// False if the bot is not set to be AFK.
    /// </summary>
    internal bool IsAfk => Status == UserStatus.AFK;

    /// <summary>
    /// Checks if the bot is set to invisible (appears offline even if online).
    /// True if the bot is set to be invisible.
    /// False if the bot is not set to be invisible.
    /// </summary>
    internal bool IsInvisible => Status == UserStatus.Invisible;

    /// <summary>
    /// Checks if the bot's status is set to online.
    /// True if the bot's status is set to online.
    /// False if the bot's status is not set to online.
    /// </summary>
    internal bool IsOnline => Status == UserStatus.Online;

    /// <summary>
    /// Checks if the bot's status is set to offline.
    /// True if the bot's status is set to offline.
    /// False if the bot's status is not set to offline.
    /// </summary>
    internal bool IsOffline => Status == UserStatus.Offline;
    
    internal IEnumerable<SocketGuild> ServersDiscordEnumerable => _botClient.Guilds;

    internal HashSet<Server>.Enumerator ServersEnumerable => Servers.GetEnumerator();
    
    /// <summary>
    /// Update the bot's internal data structures from the discord API's representation.
    /// Updates all the data structures from the discord API's representation of the bot's data.
    /// </summary>
    internal void UpdateServers()
    {
        Load();
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="Server"/> with the given ID.
    /// This is the internal representation of the server.
    /// Returns null if the bot is not on a server with the given ID.
    /// </summary>
    /// <param name="serverId">
    /// The server's ID.
    /// </param>
    /// <returns>
    /// The <see cref="Server"/> server.
    /// Null if the bot is not on a server with the given ID.
    /// </returns>
    public Server GetServer(ulong serverId)
    {
        return ServerLookup.ContainsKey(serverId)
            ? ServerLookup[serverId]
            : null;
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="SocketGuild"/> with the given ID.
    /// This is the API representation of the server.
    /// Returns null if the bot is not on a server with the given ID.
    /// </summary>
    /// <param name="serverId">
    /// The server's ID.
    /// </param>
    /// <returns>
    /// The <see cref="SocketGuild"/> server.
    /// Null if the bot is not on a server with the given ID.
    /// </returns>
    public SocketGuild GetServerDiscord(ulong serverId)
    {
        return ServerDiscordLookup.ContainsKey(serverId)
            ? ServerDiscordLookup[serverId]
            : null;
    }

    /// <summary>
    /// Gets the <see cref="Server"/> servers with the given name.
    /// This is the internal representation of the server.
    /// This is case sensitive.
    /// </summary>
    /// <param name="name">
    /// The name of the server.
    /// </param>
    /// <returns>
    /// A list of <see cref="Server"/> servers with the given name.
    /// </returns>
    public List<Server> GetServers(string name)
    {
        List<Server> list = new();
        foreach (Server server in Servers)
        {
            string serverName = server.Name;
            if (serverName == name)
                list.Add(server);
        }

        return list;
    }

    /// <summary>
    /// Gets the <see cref="SocketGuild"/> servers with the given name.
    /// This is the API representation of the server.
    /// This is case sensitive.
    /// </summary>
    /// <param name="name">
    /// The name of the servers.
    /// </param>
    /// <returns>
    /// A list of <see cref="SocketGuild"/> servers with the given name.
    /// </returns>
    public List<SocketGuild> GetDiscordServers(string name)
    {
        List<SocketGuild> list = new();
        foreach (SocketGuild server in ServersDiscord)
        {
            string serverName = server.Name;
            if (serverName == name)
                list.Add(server);
        }

        return list;
    }

    public (bool, Server) CreateServer(string name, string iconUrl = null)
    {
        Random rng = new();
        List<RestVoiceRegion> voiceRegionsList = VoiceRegions.ToList();
        int randomIndex = rng.Next(VoiceRegionCount);
        RestVoiceRegion voiceRegion = voiceRegionsList[randomIndex];
        Task<RestGuild> createdServer = _botClient.CreateGuildAsync(name, voiceRegion, null, null);
        createdServer.Wait();
        RestGuild restServer = createdServer.Result;
        if (restServer == null)
            return (false, null);
        
        ulong serverId = restServer.Id;
        LoadServers();
        Server server = ServerLookup[serverId];
        return (true, server);
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="Server"/> with the given ID.
    /// This is the internal representation of the server.
    /// Sets the value passed in by reference to the retrieved server.
    /// Returns if the bot is not on a server with the given ID.
    /// </summary>
    /// <param name="serverId">
    /// The server's ID.
    /// </param>
    /// <param name="server">
    /// The server passed in by reference.
    /// </param>
    /// <returns>
    /// True if the bot is on a server with the given ID.
    /// False if the bot is not on a server with the given ID.
    /// </returns>
    public bool TryGetServerDiscord(ulong serverId, out Server server)
    {
        return ServerLookup.TryGetValue(serverId, out server);
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="SocketGuild"/> with the given ID.
    /// This is the API representation of the server.
    /// Sets the value passed in by reference to the retrieved server.
    /// Returns if the bot is not on a server with the given ID.
    /// </summary>
    /// <param name="serverId">
    /// The server's ID.
    /// </param>
    /// <param name="server">
    /// The server passed in by reference.
    /// </param>
    /// <returns>
    /// True if the bot is on a server with the given ID.
    /// False if the bot is not on a server with the given ID.
    /// </returns>
    public bool TryGetServerDiscord(ulong serverId, out SocketGuild server)
    {
        return ServerDiscordLookup.TryGetValue(serverId, out server);
    }

    /// <summary>
    /// Checks if the bot is on a server with the given ID.
    /// </summary>
    /// <param name="serverId">
    /// The server's ID.
    /// </param>
    /// <returns>
    /// True if the bot is on a server with the given ID.
    /// False if the bot is not on a server with the given ID.
    /// </returns>
    public bool IsOnServer(ulong serverId)
    {
        return ServersIds.Contains(serverId);
    }

    /// <summary>
    /// Checks if the bot is on the given server.
    /// </summary>
    /// <param name="serverDiscord">
    /// The <see cref="SocketGuild"/>
    /// server to check
    /// (API representation of the server).
    /// </param>
    /// <returns>
    /// True if the bot is on the given server.
    /// False if the bot is not the given server.
    /// </returns>
    public bool IsOnServer(SocketGuild serverDiscord)
    {
        return ServersDiscord.Contains(serverDiscord);
    }

    /// <summary>
    /// Checks if the bot is on the given server.
    /// </summary>
    /// <param name="server">
    /// The <see cref="Server"/> server to check
    /// (internal representation of the server).
    /// </param>
    /// <returns>
    /// True if the bot is on the given server.
    /// False if the bot is not on the given server.
    /// </returns>
    public bool IsOnServer(Server server)
    {
        return Servers.Contains(server);
    }

    /// <summary>
    /// Checks if the bot is on the server with the given name.
    /// </summary>
    /// <param name="serverName">
    /// The name to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the server with the given name.
    /// False if the bot is not on the server with the given name.
    /// </returns>
    public bool IsOnServer(string serverName)
    {
        return Servers.Any(server => server.Name == serverName);
    }

    /// <summary>
    /// Checks if the bot is on the direct message conversation channel with the given ID.
    /// </summary>
    /// <param name="channelId">
    /// The ID of the direct message channel.
    /// </param>
    /// <returns>
    /// True if the bot is on the direct message conversation channel with the given ID.
    /// False if the bot is not on the direct message conversation channel with the given ID.
    /// </returns>
    public bool IsOnDMChannel(ulong channelId)
    {
        return DmChannelsIds.Contains(channelId);
    }

    /// <summary>
    /// Checks if the bot is on the given direct message conversation channel.
    /// </summary>
    /// <param name="channel">
    /// The <see cref="SocketDMChannel"/> direct message channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the given direct message conversation channel.
    /// False if the bot is not on the given direct message conversation channel.
    /// </returns>
    public bool IsOnDMChannel(SocketDMChannel channel)
    {
        return DmChannels.Contains(channel);
    }

    /// <summary>
    /// Checks if the bot is on the given direct message conversation channel.
    /// Checks against using a private channel (<see cref="ISocketPrivateChannel"/>).
    /// </summary>
    /// <param name="channel">
    /// The <see cref="ISocketPrivateChannel"/> private channel
    /// to check against.
    /// </param>
    /// <returns>
    /// True if the bot is on the given direct message conversation channel.
    /// False if the bot is not on the given direct message conversation channel.
    /// </returns>
    public bool IsOnDMChannel(ISocketPrivateChannel channel)
    {
        return PrivateChannels.Contains(channel);
    }

    /// <summary>
    /// Checks if the bot is on the direct message conversation channel with the given name.
    /// </summary>
    /// <param name="interlocutor">
    /// The name of the direct message channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the direct message conversation channel with the given name.
    /// False if the bot is not on the direct message conversation channel with the given name.
    /// </returns>
    public bool IsOnDMChannel(string interlocutor)
    {
        return PrivateChannels.Any(channel => channel.Name == interlocutor);
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="SocketDMChannel"/> direct message channel with the given ID.
    /// Returns null if the bot is not on a direct message conversation channel with the given ID.
    /// </summary>
    /// <param name="dmChannelId">
    /// The direct message channel's ID.
    /// </param>
    /// <returns>
    /// The <see cref="SocketDMChannel"/> direct message channel with the given ID.
    /// Null if the bot is not on a direct message conversation channel with the given ID.
    /// </returns>
    public SocketDMChannel GetDmChannel(ulong dmChannelId)
    {
        bool wasFound = TryGetDmChannel(dmChannelId, out SocketDMChannel channel);
        if (!wasFound)
            channel = null;

        return channel;
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="SocketDMChannel"/> direct message channel with the given ID.
    /// Assigns the value to the <see cref="channel"/> parameter passed in by reference.
    /// </summary>
    /// <param name="dmChannelId">
    /// The direct message channel's ID.
    /// </param>
    /// <param name="channel">
    /// The <see cref="SocketDMChannel"/> direct message channel with the given ID passed in by reference
    /// to be assigned the value of the direct message channel with the given ID.
    /// </param>
    /// <returns>
    /// True if the bot is on a direct message conversation channel with the given ID.
    /// False if the bot is not on a direct message conversation channel with the given ID.
    /// </returns>
    public bool TryGetDmChannel(ulong dmChannelId, out SocketDMChannel channel)
    {
        return DmChannelLookup.TryGetValue(dmChannelId, out channel);
    }

    /// <summary>
    /// Gets the <see cref="SocketDMChannel"/> direct message channels with the given name.
    /// </summary>
    /// <param name="recipient">
    /// The name of the recipient in the direct message conversation channels.
    /// </param>
    /// <returns>
    /// The <see cref="SocketDMChannel"/> direct message channels with the given name.
    /// </returns>
    public List<SocketDMChannel> GetDmChannels(string recipient)
    {
        List<SocketDMChannel> channels = new();

        foreach (SocketDMChannel channel in DmChannels)
        {
            SocketUser channelRecipient = channel.Recipient;
            string channelRecipientName = channelRecipient.Username;
            if (channelRecipientName == recipient)
                channels.Add(channel);
        }

        return channels;
    }

    /// <summary>
    /// Creates a new <see cref="SocketDMChannel"/> direct message channel with the given name.
    /// </summary>
    /// <param name="user">
    /// The user to create the direct message channel with.
    /// </param>
    /// <returns>
    /// True if the channel was created successfully.
    /// False if the channel was not created successfully or already exists.
    /// </returns>
    public bool CreateDmChannel(SocketUser user)
    {
        if (user == null)
            return false;

        string username = user.Username;
        if (IsOnDMChannel(username))
            return false;

        SocketSelfUser bot = _botClient.CurrentUser;
        Task<IDMChannel> creation = bot.GetOrCreateDMChannelAsync();
        creation.Wait();
        IDMChannel channel = creation.Result;

        if (channel is not SocketDMChannel socketDmChannel) return false;

        ulong channelId = socketDmChannel.Id;
        DmChannelsIds.Add(channelId);
        DmChannels.Add(socketDmChannel);
        DmChannelLookup.Add(channelId, socketDmChannel);
        return true;
    }

    /// <summary>
    /// Checks if the bot is on the group conversation channel with the given ID.
    /// </summary>
    /// <param name="channelId">
    /// The ID of the group conversation to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the group conversation channel with the given ID.
    /// False if the bot is not on the group conversation channel with the given ID.
    /// </returns>
    public bool IsOnGroupChannel(ulong channelId)
    {
        return GroupChannelsIds.Contains(channelId);
    }

    /// <summary>
    /// Checks if the bot is on the given group conversation channel.
    /// </summary>
    /// <param name="channel">
    /// The <see cref="SocketGroupChannel"/> group conversation channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the given group conversation channel.
    /// False if the bot is not on the given group conversation channel.
    /// </returns>
    public bool IsOnGroupChannel(SocketGroupChannel channel)
    {
        return GroupChannels.Contains(channel);
    }

    /// <summary>
    /// Checks if the bot is on the given group conversation channel.
    /// Checks against using a private channel (<see cref="ISocketPrivateChannel"/>).
    /// </summary>
    /// <param name="channel">
    /// The <see cref="ISocketPrivateChannel"/> private channel to check against.
    /// </param>
    /// <returns>
    /// True if the bot is on the given group conversation channel.
    /// False if the bot is not on the given group conversation channel.
    /// </returns>
    public bool IsOnGroupChannel(ISocketPrivateChannel channel)
    {
        return PrivateChannels.Contains(channel);
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="SocketGroupChannel"/> group conversation channel with the given ID.
    /// Returns null if the bot is not on a group conversation channel with the given ID.
    /// </summary>
    /// <param name="channelId">
    /// The group conversation channel's ID.
    /// </param>
    /// <returns>
    /// The <see cref="SocketGroupChannel"/> group conversation channel with the given ID.
    /// Null if the bot is not on a group conversation channel with the given ID.
    /// </returns>
    public SocketGroupChannel GetGroupChannel(ulong channelId)
    {
        bool wasFound = TryGetGroupChannel(channelId, out SocketGroupChannel channel);
        if (!wasFound)
            channel = null;

        return channel;
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="SocketGroupChannel"/> group conversation channel with the given ID.
    /// Assigns the value to the <see cref="channel"/> parameter passed in by reference.
    /// </summary>
    /// <param name="channelId">
    /// The group conversation channel's ID.
    /// </param>
    /// <param name="channel">
    /// The <see cref="SocketGroupChannel"/> group conversation channel with the given ID passed in by reference.
    /// </param>
    /// <returns>
    /// True if the bot is on a group conversation channel with the given ID.
    /// False if the bot is not on a group conversation channel with the given ID.
    /// </returns>
    public bool TryGetGroupChannel(ulong channelId, out SocketGroupChannel channel)
    {
        return GroupChannelLookup.TryGetValue(channelId, out channel);
    }

    /// <summary>
    /// Gets the <see cref="SocketGroupChannel"/> group conversation channels with the given name.
    /// </summary>
    /// <param name="name">
    /// The name of the group conversation channels.
    /// </param>
    /// <returns>
    /// The <see cref="SocketGroupChannel"/> group conversation channels with the given name.
    /// </returns>
    public List<SocketGroupChannel> GetGroupChannels(string name)
    {
        List<SocketGroupChannel> channels = new();

        foreach (SocketGroupChannel channel in GroupChannels)
        {
            string channelName = channel.Name;
            if (channelName == name)
                channels.Add(channel);
        }

        return channels;
    }

    /// <summary>
    /// Checks if the bot is on the private conversation channel with the given ID.
    /// </summary>
    /// <param name="channelId">
    /// The ID of the private conversation channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the private conversation channel with the given ID.
    /// False if the bot is not on the private conversation channel with the given ID.
    /// </returns>
    public bool IsOnPrivateChannel(ulong channelId)
    {
        return PrivateChannelsIds.Contains(channelId);
    }

    /// <summary>
    /// Checks if the bot is on the given private conversation channel.
    /// </summary>
    /// <param name="channel">
    /// The <see cref="ISocketPrivateChannel"/> private conversation channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the given private conversation channel.
    /// False if the bot is not on the given private conversation channel.
    /// </returns>
    public bool IsOnPrivateChannel(ISocketPrivateChannel channel)
    {
        return PrivateChannels.Contains(channel);
    }

    /// <summary>
    /// Checks if the bot is on the private conversation channel with the given name.
    /// </summary>
    /// <param name="name">
    /// The name of the private conversation channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the private conversation channel with the given name.
    /// False if the bot is not on the private conversation channel with the given name.
    /// </returns>
    public bool IsOnPrivateChannel(string name)
    {
        return PrivateChannels.Any(channel => channel.Name == name);
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="ISocketPrivateChannel"/> private conversation channel with the given ID.
    /// Returns null if the bot is not on a private conversation channel with the given ID.
    /// </summary>
    /// <param name="channelId">
    /// The private conversation channel's ID.
    /// </param>
    /// <returns>
    /// The <see cref="ISocketPrivateChannel"/> private conversation channel with the given ID.
    /// Null if the bot is not on a private conversation channel with the given ID.
    /// </returns>
    public ISocketPrivateChannel GetPrivateChannel(ulong channelId)
    {
        bool wasFound = TryGetPrivateChannel(channelId, out ISocketPrivateChannel channel);
        if (!wasFound)
            channel = null;

        return channel;
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="ISocketPrivateChannel"/> private conversation channel with the given ID.
    /// Assigns the value to the <see cref="channel"/> parameter passed in by reference.
    /// </summary>
    /// <param name="channelId">
    /// The private conversation channel's ID.
    /// </param>
    /// <param name="channel">
    /// The <see cref="ISocketPrivateChannel"/> private conversation channel with the given ID passed in by reference.
    /// </param>
    /// <returns>
    /// True if the bot is on a private conversation channel with the given ID.
    /// False if the bot is not on a private conversation channel with the given ID.
    /// </returns>
    public bool TryGetPrivateChannel(ulong channelId, out ISocketPrivateChannel channel)
    {
        return PrivateChannelLookup.TryGetValue(channelId, out channel);
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="ISocketPrivateChannel"/> private conversation channels with the given name.
    /// </summary>
    /// <param name="name">
    /// The name of the private conversation channels.
    /// </param>
    /// <returns>
    /// The <see cref="ISocketPrivateChannel"/> private conversation channels with the given name.
    /// </returns>
    public List<ISocketPrivateChannel> GetPrivateChannels(string name)
    {
        List<ISocketPrivateChannel> channels = new();

        foreach (ISocketPrivateChannel channel in PrivateChannels)
        {
            string channelName = channel.Name;
            if (channelName == name)
                channels.Add(channel);
        }

        return channels;
    }

    /// <summary>
    /// Checks if the bot is on the given voice channel with the given ID.
    /// </summary>
    /// <param name="regionId">
    /// The ID of the voice channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the given voice channel with the given ID.
    /// False if the bot is not on the given voice channel with the given ID.
    /// </returns>
    public bool IsOnVoiceRegionFromId(string regionId)
    {
        return VoiceRegionsIds.Contains(regionId);
    }

    /// <summary>
    /// Checks if the bot is on the given voice channel.
    /// </summary>
    /// <param name="region">
    /// The <see cref="RestVoiceRegion"/> voice channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the given voice channel.
    /// False if the bot is not on the given voice channel.
    /// </returns>
    public bool IsOnVoiceRegion(RestVoiceRegion region)
    {
        return VoiceRegions.Contains(region);
    }

    /// <summary>
    /// Checks if the bot is on the voice channel with the given name.
    /// </summary>
    /// <param name="regionName">
    /// The name of the voice channel to check.
    /// </param>
    /// <returns>
    /// True if the bot is on the voice channel with the given name.
    /// False if the bot is not on the voice channel with the given name.
    /// </returns>
    public bool IsOnVoiceRegionFromName(string regionName)
    {
        return VoiceRegions.Any(region => region.Name == regionName);
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="RestVoiceRegion"/> voice channel with the given ID.
    /// Returns null if the bot is not on a voice channel with the given ID.
    /// </summary>
    /// <param name="regionId">
    /// The voice channel's ID.
    /// </param>
    /// <returns>
    /// The <see cref="RestVoiceRegion"/> voice channel with the given ID.
    /// Null if the bot is not on a voice channel with the given ID.
    /// </returns>
    public RestVoiceRegion GetVoiceRegion(string regionId)
    {
        bool wasFound = TryGetVoiceRegion(regionId, out RestVoiceRegion region);
        if (!wasFound)
            region = null;

        return region;
    }

    /// <summary>
    /// Attempts to retrieve the <see cref="RestVoiceRegion"/> voice channel with the given ID.
    /// Assigns the value to the <see cref="region"/> parameter passed in by reference.
    /// </summary>
    /// <param name="regionId">
    /// The voice channel's ID.
    /// </param>
    /// <param name="region">
    /// The <see cref="RestVoiceRegion"/> voice channel with the given ID passed in by reference.
    /// </param>
    /// <returns>
    /// True if the bot is on a voice channel with the given ID.
    /// False if the bot is not on a voice channel with the given ID.
    /// </returns>
    public bool TryGetVoiceRegion(string regionId, out RestVoiceRegion region)
    {
        return VoiceRegionLookup.TryGetValue(regionId, out region);
    }

    /// <summary>
    /// Gets the <see cref="RestVoiceRegion"/> voice channels with the given name.
    /// </summary>
    /// <param name="regionName">
    /// The name of the voice channels.
    /// </param>
    /// <returns>
    /// The <see cref="RestVoiceRegion"/> voice channels with the given name.
    /// </returns>
    public List<RestVoiceRegion> GetVoiceRegions(string regionName)
    {
        List<RestVoiceRegion> regions = new();

        foreach (RestVoiceRegion region in VoiceRegions)
        {
            string currentRegionName = region.Name;
            if (currentRegionName == regionName)
                regions.Add(region);
        }

        return regions;
    }
}
        
        