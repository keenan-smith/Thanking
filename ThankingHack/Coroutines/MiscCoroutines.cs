using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Options;
using UnityEngine;

namespace Thanking.Coroutines
{
    public static class MiscCoroutines
    {
        public static IEnumerator Spammer()
        {
            while(true)
            {
                if (MiscOptions.SpammerEnabled)
                {
                    ChatManager.sendChat(EChatMode.GLOBAL, MiscOptions.SpamText);
                }
                yield return new WaitForSecondsRealtime(MiscOptions.SpammerDelay / 1000);
            }
        }
    }
}
