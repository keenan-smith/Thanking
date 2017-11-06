using System;
using System.Net;
using SDG.Framework.Debug;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.TerminalCommands
{
    public class Connect
    {
        [Initializer]
        public static void RegisterCommand()
        {
            TerminalCommandMethodInfo MInfo = new TerminalCommandMethodInfo("connect", "connect a server",
                typeof(Connect).GetMethod("DoConnection", ReflectionVariables.PublicStatic));

            TerminalCommandParameterInfo IPInfo =
                new TerminalCommandParameterInfo("IP", "IP of server to connect to", typeof(String), "127.0.0.0");

            TerminalCommandParameterInfo PortInfo =
                new TerminalCommandParameterInfo("Port", "Port of server to connect to", typeof(ushort), "27045");

            TerminalCommandParameterInfo PasswordInfo =
                new TerminalCommandParameterInfo("Password", "Password of server to connect to", typeof(String), "");

            TerminalCommand ConnectCommand = new TerminalCommand(MInfo, new[] {IPInfo, PortInfo, PasswordInfo});
            
            Terminal.registerCommand(ConnectCommand);
        }
        
        public static void DoConnection(String IP, ushort Port, String Password)
        {
            if (Provider.isConnected)
                Provider.disconnect();

            if (String.IsNullOrEmpty(IP)) return;
            
            IPAddress[] hostAddresses = Dns.GetHostAddresses(IP);

            if (hostAddresses.Length != 1 || hostAddresses[0] == null) return;
            
            String ValidIP = hostAddresses[0].ToString();

            if (!Parser.checkIP(ValidIP)) return;
            
            SteamConnectionInfo info = new SteamConnectionInfo(ValidIP, Port, Password);
            
            Provider.provider.matchmakingService.connect(info);
        }
    }
}