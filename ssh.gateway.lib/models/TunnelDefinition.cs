using System;
using System.Collections.Generic;
using ssh.gateway.lib.enums;

namespace ssh.gateway.lib.models
{
    public class TunnelDefinition
    {


        public TunnelDefinition()
        {
            this.Ports = new List<Port>();
        }
        public string ID{get;set;}
        public string UserId{get;set;}
        public string Host{get;set;}
        public string Password{get;set;}

        public string KeyFile{get;set;}
        
       
        public string Name{get;set;}
        public IList<Port> Ports{get;set;}



    }
}