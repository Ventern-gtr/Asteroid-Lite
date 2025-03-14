using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "QuitDelay")]
    public class NoQuitDelay : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}
