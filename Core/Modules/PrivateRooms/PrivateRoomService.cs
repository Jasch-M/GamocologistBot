using Discord.WebSocket;
using Template.Modules.Utils;

namespace Template.Modules.PrivateRooms;

public class PrivateRoomService
{
    private DataAssociation _data;
    private SocketVoiceChannel _creationChannel;
    private string _name = "";
    
    public PrivateRoomService(SocketVoiceChannel creationChannel)
    {
        
    }
    private string DataAssociationBasePath => "../../../Modules/PrivateRooms/Data";

    private void LoadConfiguration()
    {
        string dataAssociationConfigPath = $"{DataAssociationBasePath}/config.txt";
        _data = new DataAssociation(dataAssociationConfigPath);
        bool hasName = _data.TryGetValue("default name", out string name);
        _name = hasName ? name : "PrivateRoomDefaultName";
    }
}