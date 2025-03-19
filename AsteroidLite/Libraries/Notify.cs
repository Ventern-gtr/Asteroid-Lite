using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

namespace AsteroidLite.Libraries
{
    public class Notify
    {
        // Full credits to erik1515 / finn for making this | https://discord.gg/XxM2wWp33u
        public static void Create(string name, ref GameObject parent, ref TextMeshPro text, TextAlignmentOptions alignment, float size = 0.7f)
        {
            parent = new GameObject(name);
            text = parent.AddComponent<TextMeshPro>();
            RectTransform component = parent.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(1.5f, 1.5f);
            TMP_FontAsset tmpFont = TMP_FontAsset.CreateFontAsset(Resources.GetBuiltinResource<Font>("Arial.ttf"));
            text.font = tmpFont;
            text.characterSpacing = 6f;
            text.lineSpacing = 50f;
            text.alignment = alignment;
            text.fontSize = size;
            text.fontStyle = TMPro.FontStyles.Normal;
            text.text = string.Empty;
            parent.transform.LookAt(Camera.main.transform);
            parent.transform.Rotate(0f, 180f, 0f);
        }

        private static GameObject parent = null;
        private static TextMeshPro text = null;

        private static string last_notification = string.Empty;
        private static float cooldown;

        public static void Run()
        {
            if (parent == null)
                Create("Notifications - Asteroidlite", ref parent, ref text, TextAlignmentOptions.BottomLeft);
            else
            {
                if (!parent.activeSelf)
                    parent.SetActive(true);
                else
                {
                    if (text.renderer.material.shader != Shader.Find("GUI/Text Shader"))
                        text.renderer.material.shader = Shader.Find("GUI/Text Shader");

                    parent.transform.position = GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward * 2.75f;
                    parent.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;
                    if (!string.IsNullOrEmpty(text.text))
                    {
                        if (Time.time >= cooldown)
                        {
                            int index = text.text.IndexOf('\n');
                            if (index != -1)
                                text.text = text.text.Substring(index + 1);
                            else
                                text.text = string.Empty;

                            cooldown = Time.time + 1.25f;
                        }
                    }
                }
            }
        }

        public static void Send(string title, string notification, Color color)
        {
            if (parent != null)
            {
                if (parent.activeSelf)
                {
                    string display = $"<color=grey>[</color><color={Utilities.Color32ToHTML(color)}>{title}</color><color=grey>]</color> {notification}\n";
                    if (!last_notification.Contains(display))
                    {
                        if (!text.text.Contains(display))
                        {
                            text.text += display;
                            last_notification = "1";

                            cooldown = Time.time + 1.25f;
                        }
                    }
                }
            }
        }

        public static void Cleanup()
        {
            if (parent != null)
            {
                parent.Destroy();
                text = null;
                last_notification = null;
            }
        }
    }
}
