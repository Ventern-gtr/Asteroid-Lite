using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidLitePatches
{
    [HarmonyPatch(typeof(GorillaNetworkPublicTestsJoin))]
    [HarmonyPatch("GracePeriod", MethodType.Normal)]
    public class GracePeriod2
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}
