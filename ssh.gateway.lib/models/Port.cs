using ssh.gateway.lib.enums;
using System.Net;
namespace ssh.gateway.lib.models
{
    public class Port
    {
        public PortDirectionEnum Direction { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }

        public string LocalInterface { get; set; }
        public string RemoteInterface { get; set; }

      

    }
}