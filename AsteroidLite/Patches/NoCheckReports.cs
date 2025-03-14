using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "CheckReports", MethodType.Enumerator)]
    public class NoCheckReports : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}
