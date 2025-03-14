using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    internal class RigPatch
    {
        public static bool Prefix(VRRig __instance)
        {
            return !(__instance == GorillaTagger.Instance.offlineVRRig);
        }
    }
}
