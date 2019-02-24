using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OV_ChatManager
    {
        [Override(typeof(ChatManager), "sendChat", BindingFlags.Static | BindingFlags.Public)]
        public static void OV_sendChat(EChatMode mode, string text)
        {
            if (text.Contains("@tp"))
            {
                
                string[] array = text.Split(' ');

                if (array.Length > 1)
                    OptimizationVariables.MainPlayer.movement.transform.position = PlayerTool.getPlayer(array[1]).transform.position;

                return;
            }

            if (text.Contains("@day"))
            {
                LightingManager.time = 900;

                return;
            }

            if (text.Contains("@night"))
            {
                LightingManager.time = 0;

                return;
            }
            
            ChatManager.instance.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER,
                (byte) mode, text);
        }
    }
}