using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Template.Modules.Utils.Discord;

public class Server
{
    private readonly SocketGuild _socketGuild;
    private readonly SocketGuildUser _botUser;

    internal Dictionary<ulong, SocketRole> RoleLookup;
    internal Dictionary<ulong, SocketGuildUser> UserLookup;
    internal Dictionary<ulong, SocketChannel> ChannelLookup;
    internal Dictionary<ulong, SocketCategoryChannel> CategoryLookup;
    internal Dictionary<ulong, SocketTextChannel> TextChannelLookup;
    internal Dictionary<ulong, SocketVoiceChannel> VoiceChannelLookup;
    internal Dictionary<ulong, SocketNewsChannel> NewsChannelLookup;
    internal Dictionary<ulong, SocketMessage> MessageLookup;
    internal Dictionary<ulong, SocketMessage> PinnedMessageLookup;

    internal Dictionary<string, List<SocketRole>> RoleNameLookup;
    internal Dictionary<string, List<SocketGuildUser>> UserNameLookup;
    internal Dictionary<string, List<SocketChannel>> ChannelNameLookup;
    internal Dictionary<string, List<SocketCategoryChannel>> CategoryNameLookup;
    internal Dictionary<string, List<SocketTextChannel>> TextChannelNameLookup;
    internal Dictionary<string, List<SocketVoiceChannel>> VoiceChannelNameLookup;
    internal Dictionary<string, List<SocketNewsChannel>> NewsChannelNameLookup;
    internal Dictionary<string, List<SocketMessage>> MessageNameLookup;
    internal Dictionary<string, List<SocketMessage>> PinnedMessageNameLookup;

    internal HashSet<ulong> RoleIds;
    internal HashSet<ulong> UserIds;
    internal HashSet<ulong> ChannelIds;
    internal HashSet<ulong> CategoryIds;
    internal HashSet<ulong> TextChannelIds;
    internal HashSet<ulong> VoiceChannelIds;
    internal HashSet<ulong> NewsChannelIds;
    internal HashSet<ulong> MessageIds;
    internal HashSet<ulong> PinnedMessageIds;

    internal HashSet<string> RoleNames;
    internal HashSet<string> UserNames;
    internal HashSet<string> ChannelNames;
    internal HashSet<string> CategoryNames;
    internal HashSet<string> TextChannelNames;
    internal HashSet<string> VoiceChannelNames;
    internal HashSet<string> NewsChannelNames;
    internal HashSet<string> MessageContents;
    internal HashSet<string> PinnedMessageContents;

    public Server(SocketGuild socketGuild)
    {
        _socketGuild = socketGuild;
        SocketGuildUser botUser = _socketGuild.CurrentUser;
        ulong botId = botUser.Id;
        _botUser = _socketGuild.GetUser(botId);
        Initialize();
        InitializeRoles();
        InitializeUsers();
        InitializeChannels();
        InitializeMessages();
    }

    public SocketGuild SocketGuild => _socketGuild;
    
    public SocketGuildUser BotUser => _botUser;

    public string Name => _socketGuild.Name;

    public ulong Id => _socketGuild.Id;

    public string IconUrl => _socketGuild.IconUrl;

    public string Icon => _socketGuild.IconUrl;

    public string SplashUrl => _socketGuild.SplashUrl;

    public SocketGuildUser Owner => _socketGuild.Owner;

    public SocketGuildChannel[] GetSocketGuildChannelsArray()
    {
        return _socketGuild.Channels.ToArray();
    }

    public SocketRole[] GetSocketGuildRolesArray()
    {
        return _socketGuild.Roles.ToArray();
    }

    public SocketGuildUser[] GetSocketGuildUsersArray()
    {
        return _socketGuild.Users.ToArray();
    }

    private void Initialize()
    {
        RoleLookup = new Dictionary<ulong, SocketRole>();
        UserLookup = new Dictionary<ulong, SocketGuildUser>();
        ChannelLookup = new Dictionary<ulong, SocketChannel>();
        CategoryLookup = new Dictionary<ulong, SocketCategoryChannel>();
        TextChannelLookup = new Dictionary<ulong, SocketTextChannel>();
        VoiceChannelLookup = new Dictionary<ulong, SocketVoiceChannel>();
        NewsChannelLookup = new Dictionary<ulong, SocketNewsChannel>();
        MessageLookup = new Dictionary<ulong, SocketMessage>();
        PinnedMessageLookup = new Dictionary<ulong, SocketMessage>();

        RoleNameLookup = new Dictionary<string, List<SocketRole>>();
        UserNameLookup = new Dictionary<string, List<SocketGuildUser>>();
        ChannelNameLookup = new Dictionary<string, List<SocketChannel>>();
        CategoryNameLookup = new Dictionary<string, List<SocketCategoryChannel>>();
        TextChannelNameLookup = new Dictionary<string, List<SocketTextChannel>>();
        VoiceChannelNameLookup = new Dictionary<string, List<SocketVoiceChannel>>();
        NewsChannelNameLookup = new Dictionary<string, List<SocketNewsChannel>>();
        MessageNameLookup = new Dictionary<string, List<SocketMessage>>();

        RoleIds = new HashSet<ulong>();
        UserIds = new HashSet<ulong>();
        ChannelIds = new HashSet<ulong>();
        CategoryIds = new HashSet<ulong>();
        TextChannelIds = new HashSet<ulong>();
        VoiceChannelIds = new HashSet<ulong>();
        NewsChannelIds = new HashSet<ulong>();
        MessageIds = new HashSet<ulong>();
        PinnedMessageIds = new HashSet<ulong>();

        RoleNames = new HashSet<string>();
        UserNames = new HashSet<string>();
        ChannelNames = new HashSet<string>();
        CategoryNames = new HashSet<string>();
        TextChannelNames = new HashSet<string>();
        VoiceChannelNames = new HashSet<string>();
        NewsChannelNames = new HashSet<string>();
        MessageContents = new HashSet<string>();
        PinnedMessageContents = new HashSet<string>();
    }

    private void InitializeRoles()
    {
        //Assumes that the dictionaries and sets are already initialized
        IReadOnlyCollection<SocketRole> roles = _socketGuild.Roles;
        foreach (SocketRole role in roles)
        {
            ulong roleId = role.Id;
            string roleName = role.Name;

            RoleLookup.Add(roleId, role);
            RoleIds.Add(roleId);
            if (RoleNames.Contains(roleName))
            {
                List<SocketRole> rolesWithName = RoleNameLookup[roleName];
                rolesWithName.Add(role);
            }
            else
            {
                List<SocketRole> rolesWithName = new();
                rolesWithName.Add(role);
                RoleNameLookup.Add(roleName, rolesWithName);
                RoleNames.Add(roleName);
            }
        }
    }

    private void InitializeUsers()
    {
        //Assumes that the dictionaries and sets are already initialized
        IReadOnlyCollection<SocketGuildUser> serverUsers = _socketGuild.Users;
        foreach (SocketGuildUser user in serverUsers)
        {
            ulong userId = user.Id;
            UserLookup.Add(userId, user);
            UserIds.Add(userId);
        }
    }

    private void InitializeChannels()
    {
        //Assumes that the dictionaries and sets are already initialized
        IReadOnlyCollection<SocketChannel> channels = _socketGuild.Channels;
        foreach (SocketChannel channel in channels)
        {
            ulong channelId = channel.Id;
            ChannelLookup.Add(channelId, channel);
            ChannelIds.Add(channelId);

            if (channel is SocketCategoryChannel categoryChannel)
            {
                CategoryLookup.Add(channelId, categoryChannel);
                CategoryIds.Add(channelId);

                InitializeChannels(categoryChannel);
            }
            else if (channel is SocketTextChannel textChannel)
            {
                TextChannelLookup.Add(channelId, textChannel);
                TextChannelIds.Add(channelId);
            }
            else if (channel is SocketVoiceChannel voiceChannel)
            {
                VoiceChannelLookup.Add(channelId, voiceChannel);
                VoiceChannelIds.Add(channelId);
            }
        }
    }

    private void InitializeChannels(SocketCategoryChannel categoryChannel)
    {
        IReadOnlyCollection<SocketChannel> channels = categoryChannel.Channels;
        foreach (SocketChannel channel in channels)
        {
            ulong channelId = channel.Id;
            ChannelLookup.Add(channelId, channel);
            ChannelIds.Add(channelId);

            if (channel is SocketTextChannel textChannel)
            {
                TextChannelLookup.Add(channelId, textChannel);
                TextChannelIds.Add(channelId);
            }
            else if (channel is SocketVoiceChannel voicechannel)
            {
                VoiceChannelLookup.Add(channelId, voicechannel);
                VoiceChannelIds.Add(channelId);
            }
        }
    }

    private void InitializeMessages()
    {
        //Assumes that the dictionaries and sets are already initialized
        foreach (SocketTextChannel textChannel in TextChannelLookup.Values)
        {
            var messagesRaw = textChannel.GetMessagesAsync(limit: 100000);
            Task<List<IReadOnlyCollection<IMessage>>> messagesEnumerator = messagesRaw.ToList();
            messagesEnumerator.Wait();
            List<IReadOnlyCollection<IMessage>> messages = messagesEnumerator.Result;
            foreach (IReadOnlyCollection<IMessage> messageCollection in messages)
            {
                foreach (IMessage message in messageCollection)
                {
                    ulong messageId = message.Id;
                    SocketMessage socketMessage = message as SocketMessage;
                    MessageLookup.Add(messageId, socketMessage);
                    MessageIds.Add(messageId);

                    if (socketMessage.IsPinned)
                    {
                        PinnedMessageLookup.Add(messageId, socketMessage);
                        PinnedMessageIds.Add(messageId);
                    }
                }
            }
        }
    }

    public SocketRole GetRole(ulong roleId)
    {
        return RoleLookup.ContainsKey(roleId) ? RoleLookup[roleId] : null;
    }

    public bool TryGetRole(ulong roleId, out SocketRole role)
    {
        return RoleLookup.TryGetValue(roleId, out role);
    }

    public SocketGuildUser GetUser(ulong userId)
    {
        return UserLookup.ContainsKey(userId) ? UserLookup[userId] : null;
    }

    public bool TryGetUser(ulong userId, out SocketGuildUser user)
    {
        return UserLookup.TryGetValue(userId, out user);
    }

    public SocketChannel GetChannel(ulong channelId)
    {
        return ChannelLookup.ContainsKey(channelId) ? ChannelLookup[channelId] : null;
    }

    public bool TryGetChannel(ulong channelId, out SocketChannel channel)
    {
        return ChannelLookup.TryGetValue(channelId, out channel);
    }

    public SocketCategoryChannel GetCategory(ulong categoryId)
    {
        return CategoryLookup.ContainsKey(categoryId) ? CategoryLookup[categoryId] : null;
    }

    public bool TryGetCategory(ulong categoryId, out SocketCategoryChannel category)
    {
        return CategoryLookup.TryGetValue(categoryId, out category);
    }

    public SocketTextChannel GetTextChannel(ulong textChannelId)
    {
        return TextChannelLookup.ContainsKey(textChannelId) ? TextChannelLookup[textChannelId] : null;
    }

    public bool TryGetTextChannel(ulong textChannelId, out SocketTextChannel textChannel)
    {
        return TextChannelLookup.TryGetValue(textChannelId, out textChannel);
    }

    public SocketVoiceChannel GetVoiceChannel(ulong voiceChannelId)
    {
        return VoiceChannelLookup.ContainsKey(voiceChannelId) ? VoiceChannelLookup[voiceChannelId] : null;
    }

    public bool TryGetVoiceChannel(ulong voiceChannelId, out SocketVoiceChannel voiceChannel)
    {
        return VoiceChannelLookup.TryGetValue(voiceChannelId, out voiceChannel);
    }

    public SocketNewsChannel GetNewsChannel(ulong newsChannelId)
    {
        return NewsChannelLookup.ContainsKey(newsChannelId) ? NewsChannelLookup[newsChannelId] : null;
    }

    public bool TryGetNewsChannel(ulong newsChannelId, out SocketNewsChannel newsChannel)
    {
        return NewsChannelLookup.TryGetValue(newsChannelId, out newsChannel);
    }

    public SocketMessage GetMessage(ulong messageId)
    {
        return MessageLookup.ContainsKey(messageId) ? MessageLookup[messageId] : null;
    }

    public bool TryGetMessage(ulong messageId, out SocketMessage message)
    {
        return MessageLookup.TryGetValue(messageId, out message);
    }

    public SocketMessage GetPinnedMessage(ulong pinnedMessageId)
    {
        return PinnedMessageLookup.ContainsKey(pinnedMessageId) ? PinnedMessageLookup[pinnedMessageId] : null;
    }

    public bool TryGetPinnedMessage(ulong pinnedMessageId, out SocketMessage pinnedMessage)
    {
        return PinnedMessageLookup.TryGetValue(pinnedMessageId, out pinnedMessage);
    }

    public bool IsMessagePinned(ulong messageId)
    {
        return PinnedMessageIds.Contains(messageId);
    }

    public bool IsMessagePinned(SocketMessage message)
    {
        ulong messageId = message.Id;
        return PinnedMessageIds.Contains(messageId);
    }

    public bool IsMessagePinned(IMessage message)
    {
        ulong messageId = message.Id;
        return PinnedMessageIds.Contains(messageId);
    }

    public bool ContainsUser(ulong userId)
    {
        return UserIds.Contains(userId);
    }

    public bool ContainsChannel(ulong channelId)
    {
        return ChannelIds.Contains(channelId);
    }

    public bool ContainsCategory(ulong categoryId)
    {
        return CategoryIds.Contains(categoryId);
    }

    public bool ContainsTextChannel(ulong textChannelId)
    {
        return TextChannelIds.Contains(textChannelId);
    }

    public bool ContainsVoiceChannel(ulong voiceChannelId)
    {
        return VoiceChannelIds.Contains(voiceChannelId);
    }

    public bool ContainsNewsChannel(ulong newsChannelId)
    {
        return NewsChannelIds.Contains(newsChannelId);
    }

    public bool ContainsRole(ulong roleId)
    {
        return RoleIds.Contains(roleId);
    }

    public bool ContainsMessage(ulong messageId)
    {
        return MessageIds.Contains(messageId);
    }

    public bool ContainsPinnedMessage(ulong pinnedMessageId)
    {
        return PinnedMessageIds.Contains(pinnedMessageId);
    }

    public bool ContainsUser(SocketGuildUser user)
    {
        ulong userId = user.Id;
        return UserIds.Contains(userId);
    }

    public bool ContainsChannel(SocketChannel channel)
    {
        ulong channelId = channel.Id;
        return ChannelIds.Contains(channelId);
    }

    public bool ContainsCategory(SocketCategoryChannel category)
    {
        ulong categoryId = category.Id;
        return CategoryIds.Contains(categoryId);
    }

    public bool ContainsTextChannel(SocketTextChannel textChannel)
    {
        ulong textChannelId = textChannel.Id;
        return TextChannelIds.Contains(textChannelId);
    }

    public bool ContainsVoiceChannel(SocketVoiceChannel voiceChannel)
    {
        ulong voiceChannelId = voiceChannel.Id;
        return VoiceChannelIds.Contains(voiceChannelId);
    }

    public bool ContainsNewsChannel(SocketNewsChannel newsChannel)
    {
        ulong newsChannelId = newsChannel.Id;
        return NewsChannelIds.Contains(newsChannelId);
    }

    public bool ContainsRole(SocketRole role)
    {
        ulong roleId = role.Id;
        return RoleIds.Contains(roleId);
    }

    public bool ContainsMessage(SocketMessage message)
    {
        ulong messageId = message.Id;
        return MessageIds.Contains(messageId);
    }

    public bool ContainsPinnedMessage(SocketMessage pinnedMessage)
    {
        ulong pinnedMessageId = pinnedMessage.Id;
        return PinnedMessageIds.Contains(pinnedMessageId);
    }

    public bool ContainsUser(IMessage message)
    {
        IUser messageAuthor = message.Author;
        ulong userId = messageAuthor.Id;
        return UserIds.Contains(userId);
    }

    public bool ContainsChannel(IMessage message)
    {
        IChannel channel = message.Channel;
        ulong channelId = channel.Id;
        return ChannelIds.Contains(channelId);
    }

    public bool ContainsCategory(IMessage message)
    {
        IChannel channel = message.Channel;
        if (channel is SocketCategoryChannel category)
        {
            ulong categoryId = category.Id;
            return CategoryIds.Contains(categoryId);
        }

        return false;
    }

    public bool ContainsTextChannel(IMessage message)
    {
        IChannel channel = message.Channel;
        if (channel is not SocketTextChannel textChannel) return false;

        ulong textChannelId = textChannel.Id;
        return TextChannelIds.Contains(textChannelId);

    }

    public bool ContainsVoiceChannel(IMessage message)
    {
        IChannel channel = message.Channel;
        if (channel is not SocketVoiceChannel voiceChannel) return false;

        ulong voiceChannelId = voiceChannel.Id;
        return VoiceChannelIds.Contains(voiceChannelId);

    }

    public bool ContainsNewsChannel(IMessage message)
    {
        IChannel channel = message.Channel;
        if (channel is not SocketNewsChannel newsChannel) return false;

        ulong newsChannelId = newsChannel.Id;
        return NewsChannelIds.Contains(newsChannelId);
    }

    public bool ContainsMessage(IMessage message)
    {
        ulong messageId = message.Id;
        return MessageIds.Contains(messageId);
    }

    public bool ContainsPinnedMessage(IMessage message)
    {
        ulong pinnedMessageId = message.Id;
        return PinnedMessageIds.Contains(pinnedMessageId);
    }

    public bool ContainsRole(string roleName)
    {
        return RoleNames.Contains(roleName);
    }

    public bool ContainsMessage(string messageContent)
    {
        return MessageContents.Contains(messageContent);
    }

    public bool ContainsPinnedMessage(string pinnedMessageContent)
    {
        return PinnedMessageContents.Contains(pinnedMessageContent);
    }

    public bool ContainsUser(string userName)
    {
        return UserNames.Contains(userName);
    }

    public bool ContainsChannel(string channelName)
    {
        return ChannelNames.Contains(channelName);
    }

    public bool ContainsCategory(string categoryName)
    {
        return CategoryNames.Contains(categoryName);
    }

    public bool ContainsTextChannel(string textChannelName)
    {
        return TextChannelNames.Contains(textChannelName);
    }

    public bool ContainsVoiceChannel(string voiceChannelName)
    {
        return VoiceChannelNames.Contains(voiceChannelName);
    }

    public bool ContainsNewsChannel(string newsChannelName)
    {
        return NewsChannelNames.Contains(newsChannelName);
    }

    public void CreateCategory(string categoryName)
    {
        Task<RestCategoryChannel> channelCreation = _socketGuild.CreateCategoryChannelAsync(categoryName);
        channelCreation.Wait();
        RestCategoryChannel categoryChannel = channelCreation.Result;
        ulong categoryId = categoryChannel.Id;
        CategoryIds.Add(categoryId);

        IReadOnlyCollection<SocketCategoryChannel> categories = _socketGuild.CategoryChannels;
        SocketCategoryChannel addedCategory = categories.First(channel => channel.Id == categoryId);

        CategoryLookup.Add(categoryId, addedCategory);
        if (!CategoryNames.Contains(categoryName))
        {
            CategoryNames.Add(categoryName);
            List<SocketCategoryChannel> categoryChannels = new();
            categoryChannels.Add(addedCategory);
            CategoryNameLookup.Add(categoryName, categoryChannels);
        }
        else
        {
            List<SocketCategoryChannel> categoryChannelsWithName = CategoryNameLookup[categoryName];
            categoryChannelsWithName.Add(addedCategory);
        }
    }

    public void CreateTextChannel(string textChannelName)
    {
        Task<RestTextChannel> channelCreation = _socketGuild.CreateTextChannelAsync(textChannelName);
        channelCreation.Wait();
        RestTextChannel textChannel = channelCreation.Result;
        ulong textChannelId = textChannel.Id;
        TextChannelIds.Add(textChannelId);

        IReadOnlyCollection<SocketTextChannel> textChannels = _socketGuild.TextChannels;
        SocketTextChannel addedTextChannel = textChannels.First(channel => channel.Id == textChannelId);

        TextChannelLookup.Add(textChannelId, addedTextChannel);
        if (!TextChannelNames.Contains(textChannelName))
        {
            TextChannelNames.Add(textChannelName);
            List<SocketTextChannel> textChannelsWithName = new();
            textChannelsWithName.Add(addedTextChannel);
            TextChannelNameLookup.Add(textChannelName, textChannelsWithName);
        }
        else
        {
            List<SocketTextChannel> textChannelsWithName = TextChannelNameLookup[textChannelName];
            textChannelsWithName.Add(addedTextChannel);
        }
    }

    public void CreateVoiceChannel(string voiceChannelName)
    {
        Task<RestVoiceChannel> channelCreation = _socketGuild.CreateVoiceChannelAsync(voiceChannelName);
    }
}