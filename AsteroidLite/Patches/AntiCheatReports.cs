using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;

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
                    susReason = null;
                    susNick = null;
                    susId = null;
                }
            }
        }
    }
}
