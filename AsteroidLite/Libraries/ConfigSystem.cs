using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AsteroidLite.Libraries
{
    public class ConfigSystem : MonoBehaviour
    {
        private static readonly string SaveFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Asteroid-Lite", "config.txt"));


        private void Start()
        {
            LoadConfig();
        }

        private void OnApplicationQuit()
        {
            SaveConfig();
        }

        public static void SaveConfig()
        {
            if (!File.Exists(SaveFilePath))
            {
                File.Create(SaveFilePath);
            }
            using StreamWriter streamWriter = new StreamWriter(SaveFilePath);
            streamWriter.WriteLine($"SpeedBoost: {Plugin.SpeedBoost}");
            streamWriter.WriteLine($"TriggerBoost: {Plugin.TriggerBoost}");
            streamWriter.WriteLine($"FlippedTrigger: {Plugin.FlippedTrigger}");
            streamWriter.WriteLine($"MaxJump: {Plugin.MaxJump}");
            streamWriter.WriteLine($"JumpMulti: {Plugin.JumpMulti}");
            streamWriter.WriteLine($"WallWalk: {Plugin.WallWalk}");
            streamWriter.WriteLine($"WallWalkPower: {Plugin.WallWalkPower}");
            streamWriter.WriteLine($"WallWalkPowerNeg: {Plugin.WallWalkPowerNeg}");
            streamWriter.WriteLine($"TagAura: {Plugin.TagAura}");
            streamWriter.WriteLine($"TagAuraRange: {Plugin.TagAuraRange}");
            streamWriter.WriteLine($"LongJump: {Plugin.Longjump}");
            streamWriter.WriteLine($"LongJumpMulti: {Plugin.LongjumpMulti}");
            streamWriter.WriteLine($"Longarms: {Plugin.Longarms}");
            streamWriter.WriteLine($"LongarmsMulti: {Plugin.LongarmsMulti}");
            streamWriter.WriteLine($"Longarms: {Plugin.Longarms}");
            streamWriter.WriteLine($"LongarmsMulti: {Plugin.LongarmsMulti}");
            streamWriter.WriteLine($"PSA: {Plugin.PSA}");
            streamWriter.WriteLine($"PSASpeed: {Plugin.PSASpeed}");
            streamWriter.WriteLine($"GripToLag: {Plugin.GripToLag}");
            streamWriter.WriteLine($"LagAmmount: {Plugin.LagAmmount}");
            streamWriter.WriteLine($"AntiReport: {Plugin.AntiReport}");
            streamWriter.WriteLine($"AntiReportVis: {Plugin.AntiReportVis}");
            streamWriter.WriteLine($"AntiReportRange: {Plugin.AntiReportRange}");
            streamWriter.WriteLine($"AntiModerator: {Plugin.AntiModerator}");
            streamWriter.WriteLine($"LogModIDS: {Plugin.LogModIDS}");
            streamWriter.WriteLine($"ExtraVel: {Plugin.ExtraVel}");
            streamWriter.WriteLine($"ExtraVelMax: {Plugin.ExtraVelMax}");
            streamWriter.WriteLine($"ExtraVelMin: {Plugin.ExtraVelMin}");
            streamWriter.WriteLine($"RecRoom: {Plugin.RecRoom}");
            streamWriter.WriteLine($"RecRoomPower: {Plugin.RecRoomPower}");
            streamWriter.WriteLine($"DisableWind: {Plugin.DisableWind}");
            streamWriter.WriteLine($"NoTagFreeze: {Plugin.NoTagFreeze}");
        }

        public static void LoadConfig()
        {
            if (!File.Exists(SaveFilePath)) return;
            var data = File.ReadAllLines(SaveFilePath).Select(line => line.Split('=')).Where(parts => parts.Length == 2).ToDictionary(parts => parts[0], parts => parts[1]);

            bool.TryParse(data.GetValueOrDefault("SpeedBoost"), out Plugin.SpeedBoost);
            bool.TryParse(data.GetValueOrDefault("TriggerBoost"), out Plugin.TriggerBoost);
            bool.TryParse(data.GetValueOrDefault("FlippedTrigger"), out Plugin.FlippedTrigger);
            float.TryParse(data.GetValueOrDefault("MaxJump"), out Plugin.MaxJump);
            float.TryParse(data.GetValueOrDefault("JumpMulti"), out Plugin.JumpMulti);
            bool.TryParse(data.GetValueOrDefault("WallWalk"), out Plugin.WallWalk);
            float.TryParse(data.GetValueOrDefault("WallWalkPower"), out Plugin.WallWalkPower);
            float.TryParse(data.GetValueOrDefault("WallWalkPowerNeg"), out Plugin.WallWalkPowerNeg);
            bool.TryParse(data.GetValueOrDefault("TagAura"), out Plugin.TagAura);
            float.TryParse(data.GetValueOrDefault("TagAuraRange"), out Plugin.TagAuraRange);
            bool.TryParse(data.GetValueOrDefault("LongJump"), out Plugin.Longjump);
            float.TryParse(data.GetValueOrDefault("LongJumpMulti"), out Plugin.LongjumpMulti);
            bool.TryParse(data.GetValueOrDefault("Longarms"), out Plugin.Longarms);
            float.TryParse(data.GetValueOrDefault("LongarmsMulti"), out Plugin.LongarmsMulti);
            bool.TryParse(data.GetValueOrDefault("PSA"), out Plugin.PSA);
            float.TryParse(data.GetValueOrDefault("PSASpeed"), out Plugin.PSASpeed);
            bool.TryParse(data.GetValueOrDefault("GripToLag"), out Plugin.GripToLag);
            float.TryParse(data.GetValueOrDefault("LagAmmount"), out Plugin.LagAmmount);
            bool.TryParse(data.GetValueOrDefault("AntiReport"), out Plugin.AntiReport);
            bool.TryParse(data.GetValueOrDefault("AntiReportVis"), out Plugin.AntiReportVis);
            float.TryParse(data.GetValueOrDefault("AntiReportRange"), out Plugin.AntiReportRange);
            bool.TryParse(data.GetValueOrDefault("AntiModerator"), out Plugin.AntiModerator);
            bool.TryParse(data.GetValueOrDefault("LogModIDS"), out Plugin.LogModIDS);
            bool.TryParse(data.GetValueOrDefault("ExtraVel"), out Plugin.ExtraVel);
            float.TryParse(data.GetValueOrDefault("ExtraVelMax"), out Plugin.ExtraVelMax);
            float.TryParse(data.GetValueOrDefault("ExtraVelMin"), out Plugin.ExtraVelMin);
            bool.TryParse(data.GetValueOrDefault("RecRoom"), out Plugin.RecRoom);
            float.TryParse(data.GetValueOrDefault("RecRoomPower"), out Plugin.RecRoomPower);
            bool.TryParse(data.GetValueOrDefault("DisableWind"), out Plugin.DisableWind);
            bool.TryParse(data.GetValueOrDefault("NoTagFreeze"), out Plugin.NoTagFreeze);

        }

        private static void ParseVector(string value, out Vector3 result)
        {
            result = Vector3.zero;
            if (string.IsNullOrEmpty(value)) return;
            var parts = value.Split(',');
            if (parts.Length == 3 && float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y) && float.TryParse(parts[2], out float z))
                result = new Vector3(x, y, z);
        }
    }
}
