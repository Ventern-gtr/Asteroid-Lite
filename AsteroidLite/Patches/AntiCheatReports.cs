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
            bool flag = susId != null && susNick != null && susReason != null;
            if (flag)
            {
                bool flag2 = susId == PhotonNetwork.LocalPlayer.UserId;
                if (flag2)
                {
                    susReason = null;
                    susNick = null;
                    susId = null;
                }
            }
        }
    }
}
