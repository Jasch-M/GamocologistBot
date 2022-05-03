using System.Collections.Generic;
using Discord.WebSocket;
using Template.Modules.Utils;

namespace Template.Modules.PrivateRooms;

public class PrivateRoom
{
    private string _name;
    private UserList _whitelist;
    private UserList _blacklist;
    private UserList _users;
    private UserList _moderators;
    private bool _hidden;
    private bool _private;
    private int _userLimit;

    internal PrivateRoom(string name)
    {
        _name = name;
        _whitelist = new UserList();
        _blacklist = new UserList();
        _users = new UserList();
        _moderators = new UserList();
        _hidden = false;
        _private = false;
        _userLimit = 0;
    }
}