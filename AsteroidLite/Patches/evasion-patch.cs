using HarmonyLib;
using PlayFab.Internal;
using PlayFab;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AsteroidLite.Libraries;

namespace AsteroidLite.Patches
{
    [HarmonyPatch(typeof(SystemInfo), "deviceUniqueIdentifier", MethodType.Getter)]
    public class SpoofHWID
    {
        private static bool Prefix(ref string __result)
        {
            string text = BitConverter.ToString(GenerateRandomBytes(6)).Replace("-", "");
            string text2 = Convert.ToBase64String(GenerateRandomBytes(3))[..4].Replace("+", "G").Replace("/", "H");
            string text3 = BitConverter.ToString(GenerateRandomBytes(4)).Replace("-", "");
            string text4 = BitConverter.ToString(GenerateRandomBytes(4)).Replace("-", "");
            AsteroidUtils.LogMessage("Spoofed HWID!");
            __result = $"{text}-{text2}-{text3}-{text4}";
            return false;
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] array = new byte[length];
            RandomNumberGenerator.Fill(array);
            return array;
        }

    }

    [HarmonyPatch(typeof(PlayFabDeviceUtil), "SendDeviceInfoToPlayFab")]
    internal class BlockDeviceInfoSend : MonoBehaviour
    {
        private static bool Prefix()
        {
            AsteroidUtils.LogMessage("Blocked 'SendDeviceInfoToPlayFab'");
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayFabClientInstanceAPI), "ReportDeviceInfo")]
    internal class BlockDeviceInfoSend2
    {
        private static bool Prefix()
        {
            AsteroidUtils.LogMessage("Blocked 'ReportDeviceInfo'");
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayFabClientAPI), "ReportDeviceInfo")]
    public class BlockDeviceInfoSend3
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayFabDeviceUtil), "GetAdvertIdFromUnity")]
    public class BlockAdvertIDSend
    {
        private static bool Prefix()
        {
            AsteroidUtils.LogMessage("Blocked 'GetAdvertIdFromUnity'");
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayFabClientAPI), "AttributeInstall")]
    public class NoAttributeInstall
    {
        private static bool Prefix()
        {
            AsteroidUtils.LogMessage("Blocked 'AttributeInstall'");
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayFabHttp), "InitializeScreenTimeTracker")]
    public class ScreenTimeTracker
    {
        private static bool Prefix()
        {
            AsteroidUtils.LogMessage("Blocked 'InitializeScreenTimeTracker'");
            return false;
        }
    }
}
