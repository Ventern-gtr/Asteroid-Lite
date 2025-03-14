using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(GorillaNetworkPublicTestJoin2))]
    [HarmonyPatch("GracePeriod", MethodType.Normal)]
    public class GracePeriod1
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}
