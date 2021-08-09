using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace ssh.gateway.lib.models
{
    public class Tunnel : IDisposable
    {


          public static IPAddress Parse(string ip)
        {
            IPAddress retval = null;
            if (ip.StartsWith("0.0.0.0"))
            {
                retval = IPAddress.Any;
            }
            else if (ip.StartsWith("127.0.0.1"))
            {
                retval = IPAddress.Loopback;
            }
            else
            {
                retval = IPAddress.Parse(ip);
            }

            return retval;
        }


        public Tunnel(TunnelDefinition config)
        {
            this._config = config;
        }
        private SshClient _client;
        private List<ForwardedPort> _ports = new List<ForwardedPort>();
        private TunnelDefinition _config;
        public TunnelDefinition Config { get { return this._config; } }

        public void Start()
        {   
            var connectionInfo = new ConnectionInfo(this.Config.Host,this.Config.UserId,this.getAuthenticationMethod());
            this._client = new Renci.SshNet.SshClient(connectionInfo);
            this._client.Connect();
            this.configurePorts();
        }
        private AuthenticationMethod getAuthenticationMethod()
        {
                if(!string.IsNullOrWhiteSpace(this.Config.KeyFile))
                {
                   PrivateKeyFile pkf =new PrivateKeyFile(Config.KeyFile,Config.Password);
                  
                   return  new PrivateKeyAuthenticationMethod(this.Config.UserId, pkf);
                }
                if(!string.IsNullOrWhiteSpace(this.Config.Password))
                {
                    return new PasswordAuthenticationMethod(this.Config.UserId,this.Config.Password);
                }
                return null;
        }

        private void configurePorts()
        {
            foreach(var portDef in this.Config.Ports)
            {
                var port = getSSHPort(portDef);
                this._client.AddForwardedPort(port);
                port.Start();
                this._ports.Add(port);
            }
        }

        private ForwardedPort getSSHPort(Port def)
        {
            ForwardedPort port = null;
            if(def.Direction ==enums.PortDirectionEnum.INBOUND)
            {
                port = new ForwardedPortRemote(Tunnel.Parse(def.RemoteInterface),(uint)def.RemotePort,Tunnel.Parse(def.LocalInterface),(uint) def.LocalPort);
            }
            if(def.Direction == enums.PortDirectionEnum.OUTBOUND)
            { 
                 port = new ForwardedPortLocal(def.LocalInterface,(uint)def.LocalPort,def.RemoteInterface,(uint)def.RemotePort);
            }
            return port;
        }

        public void Stop()
        {
                foreach(var port in _ports.ToList())
                {
                    port.Stop();
                    this._ports.Remove(port);
                }
                this._client.Disconnect();
        }
        public void Dispose()
        {
            this.Stop();
        }
    }
}