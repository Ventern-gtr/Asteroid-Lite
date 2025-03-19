using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using AsteroidLite.Libraries;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    internal class AntiCheatReports
    {
        private static void Prefix(string susReason, string susId, string susNick)
        {
            if (susId != null && susNick != null && susReason != null)
            {
                if (susId == PhotonNetwork.LocalPlayer.UserId)
                {
                    Notify.Send("Asteroid", $"Anticheat report - Reason:{susReason}", Plugin.AsteroidOrange);
                    susReason = null;
                    susNick = null;
                    susId = null;
                }
            }
        }
    }
}
