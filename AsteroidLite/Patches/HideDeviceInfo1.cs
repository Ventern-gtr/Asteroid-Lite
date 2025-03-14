using HarmonyLib;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(PlayFabDeviceUtil), "SendDeviceInfoToPlayFab")]
    internal class HideDeviceInfo1 : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}
