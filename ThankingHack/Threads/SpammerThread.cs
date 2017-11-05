using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thanking.Attributes;
using Thanking.Options;

namespace Thanking.Threads
{
    public static class SpammerThread
    {
        //[Thread]
        public static void Start()
        {
            while(true)
            {
                if (MiscOptions.SpammerEnabled)
                {
                    ChatManager.sendChat(EChatMode.GLOBAL, MiscOptions.SpamText);
                    Thread.Sleep(MiscOptions.SpammerDelay);
                }
            }
        }
    }
}
