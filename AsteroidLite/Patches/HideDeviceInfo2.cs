using HarmonyLib;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(PlayFabDeviceUtil), "ReportDeviceInfo")]
    internal class HideDeviceInfo2 : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}
