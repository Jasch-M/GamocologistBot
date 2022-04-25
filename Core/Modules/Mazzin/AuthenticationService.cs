using System.Net;

namespace Template.Modules.Mazzin
{
    public class AuthenticationService
    {
        internal int MazzinID { get; private set; }
        internal IPAddress IpAddress { get; private set; }
        internal int Port { get; private set; }
    }
}