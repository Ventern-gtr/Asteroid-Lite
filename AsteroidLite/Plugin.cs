﻿using System;
using AsteroidLite.Libraries;
using System.IO;
using System.Reflection;
using BepInEx;
using Photon.Pun;
using GorillaLocomotion;
using UnityEngine;

namespace AsteroidLite
{
    [BepInPlugin("Ventern.AsteroidLite", "Ventern - AsteroidLite", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal void Start()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ventern");
            string text = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ventern", "AL-Fileran.txt"));
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
        }

        internal string GetFPS()
        {
            this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
            float f = 1f / this.deltaTime;
            return "FPS: " + Mathf.RoundToInt(f).ToString();
        }

        internal void OnGUI()
        {
            if (!Plugin.GUIStyleInit)
            {
                if (this.WindowStyle == null)
                {
                    this.WindowStyle = new GUIStyle(GUI.skin.window);
                    this.WindowStyle.alignment = TextAnchor.UpperLeft;
                    this.WindowStyle.fontSize = 17;
                    this.WindowStyle.fontStyle = FontStyle.BoldAndItalic;
                    Debug.Log("[Asteroid] WindowStyle Loaded");
                }
                if (this.ButtonStyle == null)
                {
                    this.ButtonStyle = new GUIStyle(GUI.skin.button);
                    this.ButtonStyle.alignment = TextAnchor.MiddleCenter;
                    this.ButtonStyle.fontSize = 12;
                    this.ButtonStyle.fontStyle = FontStyle.Bold;
                    Debug.Log("[Asteroid] ButtonStyle Loaded");
                }
                if (this.LabelStyle == null)
                {
                    this.LabelStyle = new GUIStyle(GUI.skin.label);
                    this.LabelStyle.fontSize = 14;
                    this.LabelStyle.fontStyle = FontStyle.Bold;
                    Debug.Log("[Asteroid] LabelStyle Loaded");
                }
                if (this.WindowStyle != null && this.ButtonStyle != null && this.LabelStyle != null)
                {
                    Debug.Log("[Asteroid] All GUI Styles Loaded");
                    Plugin.GUIStyleInit = true;
                }
            }
            if (Plugin.GUIOpen && Plugin.GUIStyleInit && this.IsInit)
            {
                GUI.color = Color.black;
                GUI.contentColor = Color.red;
                GUI.backgroundColor = Color.black;
                string str = "PING: " + PhotonNetwork.GetPing().ToString();
                string str2 = "Asteroid Lite | " + this.GetFPS() + " | " + str;
                GUI.Box(this.windowRect, string.Empty);
                this.windowRect = GUI.Window(846, this.windowRect, new GUI.WindowFunction(this.Window), "<color=#FF3B00>" + str2 + "</color>", this.WindowStyle);
            }
        }

        #region BackEnd Fields
        internal static bool GUIOpen = true;
        internal Rect windowRect = new Rect(10f, 80f, 450f, 550f);
        internal GameObject Holder = null;
        internal static bool GUIStyleInit = false;
        internal GUIStyle WindowStyle;
        internal GUIStyle ButtonStyle;
        internal GUIStyle LabelStyle;
        internal Color Orange = new Color(1f, 0.4f, 0f);
        internal static bool InfoTab = false;
        internal float deltaTime = 0f;
        internal bool IsInit = false;
        internal static Vector3 walkPos;
        internal static Vector3 walkNormal;
        internal Vector2 scrollPosition = Vector2.zero;
        internal float contentHeight = 2400f;
        internal static bool DontRepeat = false;
        #endregion BackEnd Fields

        #region Bool/Floats/Ints fields
        internal static bool SpeedBoost = false;
        internal static bool TriggerBoost = false;
        internal static bool FlippedTrigger = false;
        internal static float MaxJump = 6.5f;
        internal static float JumpMulti = 1.1f;
        internal static bool WallWalk = false;
        internal static float WallWalkPower = 1f;
        internal static float WallWalkPowerNeg = -1f;
        internal static bool TagAura = false;
        internal static float TagAuraRange = 1f;
        internal static bool Longjump = false;
        internal static float LongjumpMulti = 1f;
        internal static bool Longarms = false;
        internal static float LongarmsMulti = 1f;
        internal static bool PSA = false;
        internal static float PSASpeed = 1f;
        internal static bool GripToLag = false;
        internal static float LagAmmount = 1f;
        #endregion Bool/Floats/Ints fields

        internal void RoundValues()
        {
            Plugin.MaxJump = (float)Math.Round((double)Plugin.MaxJump, 1);
            Plugin.JumpMulti = (float)Math.Round((double)Plugin.JumpMulti, 2);
            Plugin.WallWalkPower = (float)Math.Round((double)Plugin.WallWalkPower, 1);
            Plugin.TagAuraRange = (float)Math.Round((double)Plugin.TagAuraRange, 1);
            Plugin.LongjumpMulti = (float)Math.Round((double)Plugin.LongjumpMulti, 1);
            Plugin.LongarmsMulti = (float)Math.Round((double)Plugin.LongarmsMulti, 1);
            Plugin.PSASpeed = (float)Math.Round((double)Plugin.PSASpeed, 0);
            Plugin.LagAmmount = (float)Math.Round((double)Plugin.LagAmmount, 0);
        }

        internal void Window(int windowID)
        {
            GUI.color = this.Orange;
            GUI.contentColor = this.Orange;
            GUI.backgroundColor = this.Orange;
            GUI.skin.button.fontStyle = FontStyle.Bold;
            GUI.skin.button.fontSize = 14;
            if (GUI.Button(new Rect(400f, 0f, 25f, 20f), "ⓘ", this.ButtonStyle))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.InfoTab ? 114 : 115, false, 0.2f);
                Plugin.InfoTab = !Plugin.InfoTab;
            }
            if (GUI.Button(new Rect(425f, 0f, 25f, 20f), "X", this.ButtonStyle))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(115, false, 0.3f);
                Application.Quit();
            }
            if (!Plugin.InfoTab)
            {
                float num = (Event.current.type == EventType.ScrollWheel) ? (Event.current.delta.y * 10f) : 0f;
                this.scrollPosition.y = this.scrollPosition.y + num;
                this.scrollPosition.y = Mathf.Clamp(this.scrollPosition.y, 0f, this.contentHeight - this.windowRect.height);
                this.scrollPosition = GUI.BeginScrollView(new Rect(0f, 20f, this.windowRect.width, this.windowRect.height - 20f), this.scrollPosition, new Rect(0f, 0f, this.windowRect.width - 20f, this.contentHeight));
                if (GUILayout.Button($"SpeedBoost: {Plugin.SpeedBoost}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.SpeedBoost = !Plugin.SpeedBoost;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.SpeedBoost ? 114 : 115, false, 0.2f);
                }
                if (Plugin.SpeedBoost)
                {
                    if (GUILayout.Button($"TriggerBoost: {TriggerBoost}", GUILayout.MaxWidth(420f)))
                    {
                        Plugin.TriggerBoost = !Plugin.TriggerBoost;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.TriggerBoost ? 114 : 115, false, 0.2f);
                    }
                    if (Plugin.TriggerBoost)
                    {
                        if (GUILayout.Button($"Flip Trigger: {FlippedTrigger}", GUILayout.MaxWidth(420f)))
                        {
                            Plugin.FlippedTrigger = !Plugin.FlippedTrigger;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.FlippedTrigger ? 114 : 115, false, 0.2f);
                        }
                    }
                    GUILayout.Label($"MaxJump: {MaxJump}", this.LabelStyle);
                    Plugin.MaxJump = GUILayout.HorizontalSlider(Plugin.MaxJump, 6.5f, 9f, GUILayout.MaxWidth(420f));
                    GUILayout.Label($"JumpMulti: {JumpMulti}", this.LabelStyle);
                    Plugin.JumpMulti = GUILayout.HorizontalSlider(Plugin.JumpMulti, 1.1f, 1.7f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"WallWalk: {WallWalk}", GUILayout.MaxWidth(420f)))
                {
                    WallWalk = !WallWalk;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(WallWalk ? 114 : 115, false, 0.2f);
                }
                if (WallWalk)
                {
                    GUILayout.Label($"WallWalk Pull: {WallWalkPower}", this.LabelStyle);
                    WallWalkPower = GUILayout.HorizontalSlider(WallWalkPower, 0f, 10f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"TagAura {TagAura}", GUILayout.MaxWidth(420f)))
                {
                    TagAura = !TagAura;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(TagAura ? 114 : 115, false, 0.2f);
                }
                if (TagAura)
                {
                    GUILayout.Label($"TagAura Range: {TagAuraRange}", this.LabelStyle);
                    TagAuraRange = GUILayout.HorizontalSlider(TagAuraRange, 0f, 3f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"Longjump Speed: {Longjump}", GUILayout.MaxWidth(420f)))
                {
                    Longjump = !Longjump;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Longjump ? 114 : 115, false, 0.2f);
                }
                if (Longjump)
                {
                   GUILayout.Label($"Longjump Multi: {LongjumpMulti}x", this.LabelStyle);
                   LongjumpMulti = GUILayout.HorizontalSlider(LongjumpMulti, 0f, 3f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"Longarms: {Longarms}", GUILayout.MaxWidth(420f)))
                {
                    Longarms = !Longarms;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.Longarms ? 114 : 115, false, 0.2f);
                }
                if (Longarms)
                {
                    GUILayout.Label($"LongArms Multi: {Plugin.LongarmsMulti}x", Array.Empty<GUILayoutOption>());
                    Plugin.LongarmsMulti = GUILayout.HorizontalSlider(Plugin.LongarmsMulti, 0f, 3f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"Playspace Abuse: {PSA}", GUILayout.MaxWidth(420f)))
                {
                    PSA = !PSA;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.PSA ? 114 : 115, false, 0.2f);
                }
                if (PSA)
                {
                    GUILayout.Label($"Playspace Speed: {PSASpeed}x", this.LabelStyle, Array.Empty<GUILayoutOption>());
                    Plugin.PSASpeed = GUILayout.HorizontalSlider(Plugin.PSASpeed, 0f, 5f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"GripToLag: {GripToLag}", GUILayout.MaxWidth(420f)))
                {
                    GripToLag = !GripToLag;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(GripToLag ? 114 : 115, false, 0.2f);
                }
                if (GripToLag)
                {
                    GUILayout.Label($"Lag Ammount: {LagAmmount}x", this.LabelStyle);
                    Plugin.LagAmmount = GUILayout.HorizontalSlider(Plugin.LagAmmount, 0f, 5f, GUILayout.MaxWidth(420f));
                }
                GUI.EndScrollView();
            }
            else
            {
                GUILayout.Space(10f);
                GUILayout.Label("Trigger Toggle > Left Trigger", this.LabelStyle);
                GUILayout.Label("WallWalk > Right Grip", this.LabelStyle);
                GUILayout.Label("TagAura > LeftJoystick", this.LabelStyle);
                GUILayout.Label("Longjump > Right Trigger", this.LabelStyle);
                GUILayout.Label("PSA > RightJoystick", this.LabelStyle);
                GUILayout.Label("Grip Lag > Left Grip", this.LabelStyle);
            }
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }

        internal void Update()
        {
            this.RoundValues();
            bool flag = !this.IsInit && Player.Instance && PhotonNetwork.LocalPlayer != null && Plugin.GUIStyleInit;
            if (flag)
            {
                bool flag2 = this.Holder != null;
                if (flag2)
                {
                    bool flag3 = !PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("AsteroidLite");
                    if (flag3)
                    {
                        PhotonNetwork.LocalPlayer.CustomProperties.Add("AsteroidLite", "AsteroidLite");
                    }
                    Debug.Log("[Asteroid] Menu initialization Complete!");
                    this.IsInit = true;
                }
                else
                {
                    Debug.Log("[Asteroid] Awaiting InputLibrary & NotifiLib!");
                    this.Holder = new GameObject("Holder");
                    this.Holder.AddComponent<InputLibrary>();
                    Debug.Log("[Asteroid] Input Library Loaded");
                }
            }
            bool isInit = this.IsInit;
            if (isInit)
            {
                bool keyDown = UnityInput.Current.GetKeyDown(KeyCode.Insert);
                if (keyDown)
                {
                    Plugin.GUIOpen = !Plugin.GUIOpen;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.GUIOpen ? 114 : 115, false, 0.2f);
                    Debug.Log(Plugin.GUIOpen ? "[Asteroid] Menu Opened" : "[Asteroid] Menu Closed");
                }
                bool speedBoost = Plugin.SpeedBoost;
                if (speedBoost)
                {
                    bool triggerBoost = Plugin.TriggerBoost;
                    if (triggerBoost)
                    {
                        bool flag4 = !Plugin.FlippedTrigger;
                        if (flag4)
                        {
                            bool flag5 = InputLibrary.LeftTrigger();
                            if (flag5)
                            {
                                Player.Instance.maxJumpSpeed = Plugin.MaxJump;
                                Player.Instance.jumpMultiplier = Plugin.JumpMulti;
                                return;
                            }
                            Player.Instance.maxJumpSpeed = 6.5f;
                            Player.Instance.jumpMultiplier = 1.1f;
                        }
                        else
                        {
                            bool flag6 = InputLibrary.LeftTrigger();
                            if (flag6)
                            {
                                Player.Instance.maxJumpSpeed = 6.5f;
                                Player.Instance.jumpMultiplier = 1.1f;
                                return;
                            }
                            Player.Instance.maxJumpSpeed = Plugin.MaxJump;
                            Player.Instance.jumpMultiplier = Plugin.JumpMulti;
                        }
                    }
                    else
                    {
                        Player.Instance.maxJumpSpeed = Plugin.MaxJump;
                        Player.Instance.jumpMultiplier = Plugin.JumpMulti;
                    }
                }
                else
                {
                    Player.Instance.maxJumpSpeed = 6.5f;
                    Player.Instance.jumpMultiplier = 1.1f;
                    Plugin.MaxJump = 6.5f;
                    Plugin.JumpMulti = 1.1f;
                }
                bool wallWalk = Plugin.WallWalk;
                if (wallWalk)
                {
                    bool flag7 = Player.Instance.wasLeftHandTouching || Player.Instance.wasRightHandTouching;
                    if (flag7)
                    {
                        RaycastHit raycastHit = (RaycastHit)typeof(Player).GetField("lastHitInfoHand", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Player.Instance);
                        Plugin.walkPos = raycastHit.point;
                        Plugin.walkNormal = raycastHit.normal;
                    }
                    bool flag8 = InputLibrary.RightGrip();
                    if (flag8)
                    {
                        bool flag9 = Plugin.walkPos != Vector3.zero;
                        if (flag9)
                        {
                            Plugin.WallWalkPowerNeg = -Plugin.WallWalkPower;
                            Player.Instance.bodyCollider.attachedRigidbody.AddForce(Plugin.walkNormal * Plugin.WallWalkPowerNeg, ForceMode.Acceleration);
                            Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (Plugin.WallWalkPower / Time.deltaTime)), ForceMode.Acceleration);
                        }
                    }
                }
                bool tagAura = Plugin.TagAura;
                if (tagAura)
                {
                    bool flag10 = InputLibrary.LeftJoystick();
                    if (flag10)
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            Vector3 position = vrrig.headMesh.transform.position;
                            Vector3 position2 = GorillaTagger.Instance.offlineVRRig.head.rigTarget.position;
                            float num = Vector3.Distance(position, position2);
                            bool flag11 = GorillaTagger.Instance.offlineVRRig.setMatIndex == 1 || GorillaTagger.Instance.offlineVRRig.setMatIndex == 2 || GorillaTagger.Instance.offlineVRRig.setMatIndex == 13;
                            if (flag11)
                            {
                                bool flag12 = !vrrig.mainSkin.material.name.Contains("fected") && !Player.Instance.disableMovement && num < Plugin.TagAuraRange;
                                if (flag12)
                                {
                                    Player.Instance.rightControllerTransform.position = position;
                                }
                            }
                        }
                    }
                }
                bool longjump = Plugin.Longjump;
                if (longjump)
                {
                    bool flag13 = InputLibrary.RightTrigger();
                    if (flag13)
                    {
                        Player.Instance.maxJumpSpeed = 10f;
                        Player.Instance.jumpMultiplier = Plugin.JumpMulti * Plugin.LongjumpMulti;
                        return;
                    }
                    Player.Instance.maxJumpSpeed = Plugin.MaxJump;
                    Player.Instance.jumpMultiplier = Plugin.JumpMulti;
                }
                bool longarms = Plugin.Longarms;
                if (longarms)
                {
                    Player.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position) * Plugin.LongarmsMulti;
                    Player.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position) * Plugin.LongarmsMulti;
                }
                bool psa = Plugin.PSA;
                if (psa)
                {
                    bool flag14 = InputLibrary.RightJoystick();
                    if (flag14)
                    {
                        Player.Instance.transform.position += Player.Instance.bodyCollider.transform.forward * Plugin.PSASpeed * Time.deltaTime;
                    }
                }
                bool gripToLag = Plugin.GripToLag;
                if (gripToLag)
                {
                    bool flag15 = InputLibrary.LeftGrip();
                    if (flag15)
                    {
                        bool flag16 = Plugin.LagAmmount == 1f;
                        if (flag16)
                        {
                            foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                        }
                        bool flag17 = Plugin.LagAmmount == 2f;
                        if (flag17)
                        {
                            foreach (GameObject gameObject2 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject3 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                        }
                        bool flag18 = Plugin.LagAmmount == 3f;
                        if (flag18)
                        {
                            foreach (GameObject gameObject4 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject5 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject6 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                        }
                        bool flag19 = Plugin.LagAmmount == 4f;
                        if (flag19)
                        {
                            foreach (GameObject gameObject7 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject8 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject9 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject10 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject11 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                        }
                        bool flag20 = Plugin.LagAmmount == 5f;
                        if (flag20)
                        {
                            foreach (GameObject gameObject12 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject13 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject14 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject15 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject16 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject17 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                        }
                    }
                }
            }
        }
    }
}
