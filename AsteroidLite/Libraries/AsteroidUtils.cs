using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace AsteroidLite.Libraries
{
    public class AsteroidUtils : MonoBehaviour
    {
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Asteroid-Lite");
        private static string text = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Asteroid-Lite", "fileran.txt"));
        private static string logpath = Path.Combine(path, "Log.txt");
        private static string LastLog = null;

        internal void Start()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(text))
            {
                Application.OpenURL("https://discord.gg/TcgMCaXeQ4");
                Application.OpenURL("https://guns.lol/Ventern");
                File.Create(text).Close();
            }
            if (File.Exists(logpath))
            {
                File.Delete(logpath);
            }
            Application.logMessageReceived += HandleLog;
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => LogError(e.ExceptionObject as Exception);
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string logEntry = $"[{DateTime.UtcNow}] {type}: {logString}\n{stackTrace}\n";
            if (logEntry != LastLog && type != LogType.Log && type != LogType.Assert && type != LogType.Warning)
            {
                File.AppendAllText(logpath, logEntry);
                LastLog = logEntry;
            }
        }

        public static void LogMessage(string message)
        {
            if (LastLog != message)
            {
                string FullMessage = $"[{DateTime.UtcNow}] [Asteroid] {message}\n";
                File.AppendAllText(logpath, FullMessage);
                Debug.Log($"[Asteroid] {message}");
                LastLog = message;
            }
        }

        public static void LogError(Exception ex)
        {
            string errorMessage = $"[{DateTime.UtcNow}] ERROR: {ex.Message}\n{ex.StackTrace}\n";
            if (errorMessage != LastLog)
            {
                Debug.LogError(errorMessage);
                File.AppendAllText(logpath, errorMessage);
                LastLog = errorMessage;
            }
        }

        void OnDestroy()
        {
            AppDomain.CurrentDomain.UnhandledException -= (sender, e) => LogError(e.ExceptionObject as Exception);
        }

        private static float deltaTime = 0f;
        internal static void VisualizeAura(Vector3 position, float range, Color color)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.transform.position = position;
            gameObject.transform.localScale = new Vector3(range, range, range);
            Color color2 = color;
            color2.a = 0.25f;
            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            gameObject.GetComponent<Renderer>().material.color = color2;
        }

        internal static Material UI = new Material(Shader.Find("GorillaTag/UberShader"));
        internal static Color AccentColor = new Color(0.8f, 0.3f, 0f);
        internal static void CustomBoards()
        {
            TMP_FontAsset tmpFont = TMP_FontAsset.CreateFontAsset(Resources.GetBuiltinResource<Font>("Arial.ttf"));
            GameObject motdThing = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdtext");
            TextMeshPro text = motdThing.GetComponent<TextMeshPro>();
            text.font = tmpFont;
            text.fontSize = 128;
            text.lineSpacing = 25f;
            text.fontStyle = FontStyles.Bold;
            text.text = $"Thanks for using <color={AsteroidUtils.Color32ToHTML(Plugin.AsteroidOrange)}>Asteroid Lite</color>, although this may be undetected you still may get <color={AsteroidUtils.Color32ToHTML(Color.red)}>banned</color>.\nPlease consider buying asteroid full if u want to league cheat!";
        }

        internal static string GetFPS()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float f = 1f / deltaTime;
            return "FPS: " + Mathf.RoundToInt(f).ToString();
        }

        public static Color HTMLToColor32(string hex)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(hex, out color))
                return (Color32)color;
            return new Color32(0, 0, 0, 255);
        }

        public static string Color32ToHTML(Color32 color)
        {
            return $"#{color.r:X2}{color.g:X2}{color.b:X2}";
        }

        private static Dictionary<VRRig, float> delays = new Dictionary<VRRig, float> { };
        public static void FixRigMaterialESPColors(VRRig rig)
        {
            if ((delays.ContainsKey(rig) && Time.time > delays[rig]) || !delays.ContainsKey(rig))
            {
                if (delays.ContainsKey(rig))
                    delays[rig] = Time.time + 5f;
                else
                    delays.Add(rig, Time.time + 5f);

                rig.mainSkin.sharedMesh.colors32 = Enumerable.Repeat((Color32)Color.white, rig.mainSkin.sharedMesh.colors32.Length).ToArray();
                rig.mainSkin.sharedMesh.colors = Enumerable.Repeat(Color.white, rig.mainSkin.sharedMesh.colors.Length).ToArray();
            }
        }
    }
}
