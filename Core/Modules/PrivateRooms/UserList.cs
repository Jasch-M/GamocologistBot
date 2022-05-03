using System.Collections.Generic;
using Discord.WebSocket;

namespace Template.Modules.PrivateRooms;

public class UserList
{
    private HashSet<SocketGuildUser> _users;
    private HashSet<SocketRole> _roles;
    private int _numberOfUsers;
    private int _numberOfRoles;

    public UserList()
    {
        _users = new HashSet<SocketGuildUser>();
        _roles = new HashSet<SocketRole>();
        _numberOfUsers = 0;
        _numberOfRoles = 0;
    }

    public UserList(IEnumerable<SocketGuildUser> users)
    {
        _users = new HashSet<SocketGuildUser>();
        _numberOfUsers = 0;
        AddUsers(users);
    }

    public bool AddUsers(IEnumerable<SocketGuildUser> users)
    {
        (bool wasSuccessful, int numberOfAddedUsers) = AddElements(_users, users);
        _numberOfUsers += numberOfAddedUsers;
        return wasSuccessful;
    }

    public bool AddRoles(IEnumerable<SocketRole> roles)
    {
        (bool wasSuccessful, int numberOfAddedRoles) = AddElements(_roles, roles);
        _numberOfRoles += numberOfAddedRoles;
        return wasSuccessful;
    }

    private static (bool, int) AddElements<T>(HashSet<T> set, IEnumerable<T> elements)
    {
        int addedElements = 0;
        bool areAllOkay = true;
        HashSet<T> elementsToAdd = new();
        foreach (T element in elements)
        {
            if (set.Contains(element) || elementsToAdd.Contains(element))
            {
                areAllOkay = false;
                break;
            }

            elementsToAdd.Add(element);
        }

        if (areAllOkay)
        {
            foreach (T element in elementsToAdd)
            {
                set.Add(element);
                addedElements += 1;
            }
        }

        return (areAllOkay, addedElements);
    }

    public bool AddUser(SocketGuildUser user)
    {
        bool wasAdded = _users.Add(user);
        if (wasAdded)
            _numberOfUsers += 1;
        return wasAdded;
    }

    public bool AddRole(SocketRole role)
    {
        bool wasAdded = _roles.Add(role);
        if (wasAdded)
            _numberOfRoles += 1;
        return wasAdded;
    }

    public bool ContainsUser(SocketGuildUser user)
    {
        return _users.Contains(user);
    }
    
    public bool ContainsRole(SocketRole role)
    {
        return _roles.Contains(role);
    }

    public bool RemoveUsers(IEnumerable<SocketGuildUser> users)
    {
        (bool wasSuccessful, int numberOfRemovedUsers) = RemoveElements(_users, users);
        _numberOfUsers -= numberOfRemovedUsers;
        return wasSuccessful;
    }
    
    public bool RemoveRoles(IEnumerable<SocketRole> roles)
    {
        (bool wasSuccessful, int numberOfRemovedRoles) = RemoveElements(_roles, roles);
        _numberOfUsers -= numberOfRemovedRoles;
        return wasSuccessful;
    }
    
    private static (bool, int) RemoveElements<T>(HashSet<T> set, IEnumerable<T> elements)
    {
        int removedElements = 0;
        bool areAllOkay = true;
        HashSet<T> elementsToRemove = new();
        foreach (T element in elements)
        {
            if (!set.Contains(element) && !elementsToRemove.Contains(element))
            {
                areAllOkay = false;
                break;
            }

            elementsToRemove.Add(element);
        }

        if (areAllOkay)
        {
            foreach (T element in elementsToRemove)
            {
                set.Remove(element);
                removedElements -= 1;
            }
        }

        return (areAllOkay, removedElements);
    }
}