using System;
using ssh.gateway.lib.models;
using System.Linq;
using ssh.gateway.lib.enums;
using System.IO;
using System.Text.Json;

namespace JumpToMe
{
    class Program
    {
        static void Main(string[] args)
        {   
            var configFile = "/etc/jump.to.json";
            if(args !=null&& args.Length >= 1)
            {
                configFile = args[0];
            }
            var config = JsonSerializer.Deserialize<TunnelDefinition>(File.ReadAllText(configFile));
            var my = new Tunnel(config);
            my.Start();
            Console.Write("tunnel up and running");               
            Console.ReadLine();
        }
    }
}



