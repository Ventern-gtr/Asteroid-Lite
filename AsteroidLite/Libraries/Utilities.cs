using GorillaNetworking;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
namespace AsteroidLite.Libraries
{
    public class Utilities
    {
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
            text.text = $"Thanks for using <color={AsteroidLite.Libraries.Utilities.Color32ToHTML(Plugin.AsteroidOrange)}>Asteroid Lite</color>, although this may be undetected you still may get <color={AsteroidLite.Libraries.Utilities.Color32ToHTML(Color.red)}>banned</color>.\nPlease consider buying asteroid full if u want to league cheat!";
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
