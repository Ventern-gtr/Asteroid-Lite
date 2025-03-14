using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(VRRigJobManager), "DeregisterVRRig")]
    internal class RigPatch2
    {
        public static bool Prefix(VRRigJobManager __instance, VRRig rig)
        {
            return !(__instance == GorillaTagger.Instance.offlineVRRig);
        }
    }
}
