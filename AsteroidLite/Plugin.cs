﻿using AsteroidLite.Libraries;
using BepInEx;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AsteroidLite
{
    [BepInPlugin("Ventern.AsteroidLite", "Ventern - AsteroidLite", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal async void Start()
        {
            // StartCoroutine(AsteroidUtils.NoLeavesString());
            LeavesGameobject = "UnityTempFile-27660f060fbe5c449995964e7f219762 (combined by EdMeshCombiner)";

        }

        internal void OnGUI()
        {
            if (!Plugin.GUIStyleInit && IsInit)
            {
                if (this.WindowStyle == null)
                {
                    this.WindowStyle = new GUIStyle(GUI.skin.window);
                    this.WindowStyle.alignment = TextAnchor.UpperLeft;
                    this.WindowStyle.fontSize = 17;
                    this.WindowStyle.fontStyle = FontStyle.BoldAndItalic;
                    AsteroidUtils.LogMessage("WindowStyle Loaded");
                }
                if (this.ButtonStyle == null)
                {
                    this.ButtonStyle = new GUIStyle(GUI.skin.button);
                    this.ButtonStyle.alignment = TextAnchor.MiddleCenter;
                    this.ButtonStyle.fontSize = 12;
                    this.ButtonStyle.fontStyle = FontStyle.Bold;
                    AsteroidUtils.LogMessage("ButtonStyle Loaded");

                }
                if (this.LabelStyle == null)
                {
                    this.LabelStyle = new GUIStyle(GUI.skin.label);
                    this.LabelStyle.fontSize = 14;
                    this.LabelStyle.fontStyle = FontStyle.Bold;
                    AsteroidUtils.LogMessage("LabelStyle Loaded");
                }
                if (this.WindowStyle != null && this.ButtonStyle != null && this.LabelStyle != null)
                {
                    AsteroidUtils.LogMessage("All GUI Styles Loaded");
                    Plugin.GUIStyleInit = true;
                }
            }
            if (Plugin.GUIOpen && Plugin.GUIStyleInit && this.IsInit)
            {
                GUI.color = Color.black;
                GUI.contentColor = Color.red;
                GUI.backgroundColor = Color.black;
                string str = "PING: " + PhotonNetwork.GetPing().ToString();
                string str2 = "Asteroid Lite | " + AsteroidLite.Libraries.AsteroidUtils.GetFPS() + " | " + str;
                GUI.Box(this.windowRect, string.Empty);
                this.windowRect = GUI.Window(633, this.windowRect, new GUI.WindowFunction(this.Window), "<color=#FF3B00>" + str2 + "</color>", this.WindowStyle);
            }
        }

        #region BackEnd Fields
        internal static bool GUIOpen = true;
        private Rect windowRect = new Rect(10f, 80f, 450f, 550f);
        internal GameObject Holder = null;
        private static bool GUIStyleInit = false;
        private GUIStyle WindowStyle;
        private GUIStyle ButtonStyle;
        private GUIStyle LabelStyle;
        internal Color Orange = new Color(1f, 0.4f, 0f);
        internal static Color AsteroidOrange = new Color(1f, 0.4f, 0f);
        private static bool InfoTab = false;
        internal bool IsInit = false;
        private static Vector3 walkPos;
        private static Vector3 walkNormal;
        private Vector2 scrollPosition = Vector2.zero;
        private float contentHeight = 2400f;
        internal static bool DontRepeat = false;
        internal static bool DontRepeat1 = false;
        internal static bool DontRepeat2 = false;
        internal static bool DontRepeat3 = false;
        private static GorillaSurfaceOverride[] surfaceOverrides;
        private static ForceVolume[] forceVolumes;
        public static string LeavesGameobject;
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
        internal static bool Chams = false;
        internal static bool TargetChams = false;
        internal static bool AntiReport = false;
        internal static bool AntiReportVis = false;
        internal static float AntiReportRange = 0.35f;
        internal static bool AntiReportRejoin = false;
        internal static bool AntiModerator = false;
        internal static bool LogModIDS = false;
        internal static bool ExtraVel = false;
        internal static float ExtraVelMax = 1f;
        internal static float ExtraVelMin = 1f;
        internal static bool RecRoom = false;
        internal static float RecRoomPower = 1f;
        internal static bool DisableWind = false;
        internal static bool NoTagFreeze = false;
        internal static bool NoLeaves = false;
        #endregion Bool/Floats/Ints fields

        internal void RoundValues()
        {
            Plugin.MaxJump = (float)Math.Round((double)Plugin.MaxJump, 1);
            Plugin.JumpMulti = (float)Math.Round((double)Plugin.JumpMulti, 2);
            Plugin.WallWalkPower = (float)Math.Round((double)Plugin.WallWalkPower, 1);
            Plugin.TagAuraRange = (float)Math.Round((double)Plugin.TagAuraRange, 1);
            Plugin.LongjumpMulti = (float)Math.Round((double)Plugin.LongjumpMulti, 2);
            Plugin.LongarmsMulti = (float)Math.Round((double)Plugin.LongarmsMulti, 1);
            Plugin.PSASpeed = (float)Math.Round((double)Plugin.PSASpeed, 0);
            Plugin.LagAmmount = (float)Math.Round((double)Plugin.LagAmmount, 0);
            Plugin.AntiReportRange = (float)Math.Round((double)Plugin.AntiReportRange, 2);
            Plugin.ExtraVelMax = (float)Math.Round((double)Plugin.ExtraVelMax, 2);
            Plugin.ExtraVelMin = (float)Math.Round((double)Plugin.ExtraVelMin, 2);
            Plugin.RecRoomPower = (float)Math.Round((double)Plugin.RecRoomPower, 1);
        }

        internal void Window(int windowID)
        {
            GUI.color = this.Orange;
            GUI.contentColor = this.Orange;
            GUI.backgroundColor = this.Orange;
            GUI.skin.button.fontStyle = FontStyle.Bold;
            GUI.skin.button.fontSize = 14;
            if (GUI.Button(new Rect(375f, 0f, 25f, 20f), "!", this.ButtonStyle))
            {
                GUIOpen = false;
                SpeedBoost = false;
                TriggerBoost = false;
                FlippedTrigger = false;
                WallWalk = false;
                TagAura = false;
                Longjump = false;
                Longarms = false;
                PSA = false;
                GripToLag = false;
                Chams = false;
                TargetChams = false;
                AntiReport = false;
                AntiReportVis = false;
                AntiModerator = false;
                LogModIDS = false;
                ExtraVel = false;
                RecRoom = false;
                DisableWind = false;
                NoTagFreeze = false;
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.InfoTab ? 114 : 115, false, 0.2f);
            }
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
                if (GUILayout.Button($"ExtraVel: {Plugin.ExtraVel}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.ExtraVel = !Plugin.ExtraVel;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.ExtraVel ? 114 : 115, false, 0.2f);
                }
                if (Plugin.ExtraVel)
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
                    GUILayout.Label($"ExtraVelMax: {ExtraVelMax}", this.LabelStyle);
                    ExtraVelMax = GUILayout.HorizontalSlider(ExtraVelMax, 1f, 2f, GUILayout.MaxWidth(420f));
                    GUILayout.Label($"ExtraVelMin: {ExtraVelMin}", this.LabelStyle);
                    ExtraVelMin = GUILayout.HorizontalSlider(ExtraVelMin, 1f, 2f, GUILayout.MaxWidth(420f));
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
                if (GUILayout.Button($"Chams: {Chams}", GUILayout.MaxWidth(420f)))
                {
                    Chams = !Chams;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Chams ? 114 : 115, false, 0.2f);
                }
                if (Chams)
                {
                    if (GUILayout.Button($"Target Only: {TargetChams}", GUILayout.MaxWidth(420f)))
                    {
                        TargetChams = !TargetChams;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(TargetChams ? 114 : 115, false, 0.2f);
                    }
                }
                if (GUILayout.Button($"RecRoom: {RecRoom}", GUILayout.MaxWidth(420f)))
                {
                    RecRoom = !RecRoom;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(RecRoom ? 114 : 115, false, 0.2f);
                }
                if (RecRoom)
                {
                    GUILayout.Label($"RecRoom Power: {RecRoomPower}", this.LabelStyle);
                    Plugin.RecRoomPower = GUILayout.HorizontalSlider(Plugin.RecRoomPower, 0f, 10f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"AntiReport: {Plugin.AntiReport}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.AntiReport = !Plugin.AntiReport;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.AntiReport ? 114 : 115, false, 0.2f);
                }
                if (Plugin.AntiReport)
                {
                    if (GUILayout.Button($"AntiReport Visble: {AntiReportVis}", GUILayout.MaxWidth(420f)))
                    {
                        Plugin.AntiReportVis = !Plugin.AntiReportVis;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.TriggerBoost ? 114 : 115, false, 0.2f);
                    }
                    if (GUILayout.Button($"AntiReport Rejoin: {AntiReportRejoin}", GUILayout.MaxWidth(420f)))
                    {
                        Plugin.AntiReportRejoin = !Plugin.AntiReportRejoin;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.AntiReportRejoin ? 114 : 115, false, 0.2f);
                    }
                    GUILayout.Label($"AntiReport Range: {AntiReportRange}", this.LabelStyle);
                    Plugin.AntiReportRange = GUILayout.HorizontalSlider(Plugin.AntiReportRange, 0f, 1f, GUILayout.MaxWidth(420f));
                }
                if (GUILayout.Button($"AntiModerator: {Plugin.AntiModerator}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.AntiModerator = !Plugin.AntiModerator;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.AntiModerator ? 114 : 115, false, 0.2f);
                }
                if (AntiModerator)
                {
                    if (GUILayout.Button($"Log Mod IDS: {Plugin.LogModIDS}", GUILayout.MaxWidth(420f)))
                    {
                        Plugin.LogModIDS = !Plugin.LogModIDS;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.LogModIDS ? 114 : 115, false, 0.2f);
                    }
                }
                if (GUILayout.Button($"Disable Wind: {Plugin.DisableWind}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.DisableWind = !Plugin.DisableWind;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.DisableWind ? 114 : 115, false, 0.2f);
                }
                if (GUILayout.Button($"NoTagFreeze: {Plugin.NoTagFreeze}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.NoTagFreeze = !Plugin.NoTagFreeze;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.NoTagFreeze ? 114 : 115, false, 0.2f);
                }

                if (GUILayout.Button($"NoLeaves: {Plugin.NoLeaves}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.NoLeaves = !Plugin.NoLeaves;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.NoLeaves ? 114 : 115, false, 0.2f);
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
                GUILayout.Label("Left Joystick Move > RecRoom", this.LabelStyle);
            }
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }

        internal void Update()
        {
            this.RoundValues();
            if (!this.IsInit && GTPlayer.Instance && PhotonNetwork.LocalPlayer != null)
            {
                if (this.Holder != null)
                {
                    if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("AsteroidLite"))
                    {
                        PhotonNetwork.LocalPlayer.CustomProperties.Add("AsteroidLite", "AsteroidLite");
                    }
                    AsteroidLite.Libraries.AsteroidUtils.CustomBoards();
                    AsteroidUtils.LogMessage("Menu Initialization process completed!");
                    this.IsInit = true;
                }
                else
                {
                    this.Holder = new GameObject("Holder");
                    this.Holder.AddComponent<AsteroidLite.Libraries.AsteroidUtils>();
                    AsteroidUtils.LogMessage("Utils Component!");
                    this.Holder.AddComponent<InputLibrary>();
                    AsteroidUtils.LogMessage("InputLibrary Component!");
                    this.Holder.AddComponent<Notify>();
                    AsteroidUtils.LogMessage("Notify Component!");
                }
            }
            if (this.IsInit)
            {
                if (UnityInput.Current.GetKeyDown(KeyCode.Insert))
                {
                    GUIOpen = !GUIOpen;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.GUIOpen ? 114 : 115, false, 0.2f);
                    AsteroidUtils.LogMessage(Plugin.GUIOpen ? "[Asteroid] Menu Opened" : "[Asteroid] Menu Closed");
                }
                if (NoLeaves)
                {
                    if (!DontRepeat3)
                    {
                        Debug.Log("String:" + LeavesGameobject);
                        foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
                        {
                            if (gameObject.name.Contains(LeavesGameobject))
                            {
                                gameObject.SetActive(false);
                            }
                        }
                        DontRepeat3 = true;
                    }
                }
                else
                {
                    if (DontRepeat3)
                    {
                        foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
                        {
                            if (gameObject.name.Contains(LeavesGameobject))
                            {
                                gameObject.SetActive(true);
                            }
                        }
                        DontRepeat3 = false;
                    }
                }
                if (Plugin.SpeedBoost)
                {
                    if (Plugin.TriggerBoost)
                    {
                        if (!Plugin.FlippedTrigger)
                        {
                            if (InputLibrary.LeftTrigger())
                            {
                                GTPlayer.Instance.maxJumpSpeed = Plugin.MaxJump;
                                GTPlayer.Instance.jumpMultiplier = Plugin.JumpMulti;
                                return;
                            }
                            GTPlayer.Instance.maxJumpSpeed = 6.5f;
                            GTPlayer.Instance.jumpMultiplier = 1.1f;
                        }
                        else
                        {
                            if (InputLibrary.LeftTrigger())
                            {
                                GTPlayer.Instance.maxJumpSpeed = 6.5f;
                                GTPlayer.Instance.jumpMultiplier = 1.1f;
                                return;
                            }
                            GTPlayer.Instance.maxJumpSpeed = Plugin.MaxJump;
                            GTPlayer.Instance.jumpMultiplier = Plugin.JumpMulti;
                        }
                    }
                    else
                    {
                        GTPlayer.Instance.maxJumpSpeed = Plugin.MaxJump;
                        GTPlayer.Instance.jumpMultiplier = Plugin.JumpMulti;
                    }
                }
                else
                {
                    GTPlayer.Instance.maxJumpSpeed = 6.5f;
                    GTPlayer.Instance.jumpMultiplier = 1.1f;
                    Plugin.MaxJump = 6.5f;
                    Plugin.JumpMulti = 1.1f;
                }
                if (Plugin.WallWalk)
                {
                    if (GTPlayer.Instance.wasLeftHandColliding || GTPlayer.Instance.wasRightHandColliding)
                    {
                        RaycastHit raycastHit = (RaycastHit)typeof(GTPlayer).GetField("lastHitInfoHand", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(GTPlayer.Instance);
                        Plugin.walkPos = raycastHit.point;
                        Plugin.walkNormal = raycastHit.normal;
                    }
                    if (InputLibrary.RightGrip())
                    {
                        if (Plugin.walkPos != Vector3.zero)
                        {
                            Plugin.WallWalkPowerNeg = -Plugin.WallWalkPower;
                            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Plugin.walkNormal * Plugin.WallWalkPowerNeg, ForceMode.Acceleration);
                            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (Plugin.WallWalkPower / Time.deltaTime)), ForceMode.Acceleration);
                        }
                    }
                }
                if (Plugin.TagAura)
                {
                    if (InputLibrary.LeftJoystick())
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            Vector3 position = vrrig.headMesh.transform.position;
                            Vector3 position2 = GorillaTagger.Instance.offlineVRRig.head.rigTarget.position;
                            float num = Vector3.Distance(position, position2);
                            if (RigManager.PlayerIsTagged(GorillaTagger.Instance.offlineVRRig))
                            {
                                if (!RigManager.PlayerIsTagged(vrrig) && !GTPlayer.Instance.disableMovement && num < Plugin.TagAuraRange)
                                {
                                    GTPlayer.Instance.rightControllerTransform.position = position;
                                }
                            }
                        }
                    }
                }
                if (Plugin.Longjump)
                {
                    if (InputLibrary.RightTrigger())
                    {
                        GTPlayer.Instance.maxJumpSpeed = 10f;
                        GTPlayer.Instance.jumpMultiplier = Plugin.JumpMulti * Plugin.LongjumpMulti;
                        return;
                    }
                    GTPlayer.Instance.maxJumpSpeed = Plugin.MaxJump;
                    GTPlayer.Instance.jumpMultiplier = Plugin.JumpMulti;
                }
                if (Plugin.Longarms)
                {
                    GTPlayer.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position) * Plugin.LongarmsMulti;
                    GTPlayer.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position) * Plugin.LongarmsMulti;
                }
                if (Plugin.PSA)
                {
                    if (InputLibrary.LeftJoystickMoveY() > 0.2f)
                        GTPlayer.Instance.transform.position += GTPlayer.Instance.bodyCollider.transform.forward * PSASpeed * Time.deltaTime;
                }
                if (Plugin.GripToLag)
                {
                    if (InputLibrary.LeftGrip())
                    {
                        if (Plugin.LagAmmount == 1f)
                        {
                            foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                        }
                        if (Plugin.LagAmmount == 2f)
                        {
                            foreach (GameObject gameObject2 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                            foreach (GameObject gameObject3 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                            {
                            }
                        }
                        if (Plugin.LagAmmount == 3f)
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
                        if (Plugin.LagAmmount == 4f)
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
                        if (Plugin.LagAmmount == 5f)
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
                if (Chams)
                {
                    if (TargetChams)
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (RigManager.PlayerIsTagged(GorillaTagger.Instance.offlineVRRig))
                            {
                                if (!vrrig.isOfflineVRRig)
                                {
                                    if (!RigManager.PlayerIsTagged(vrrig))
                                    {
                                        AsteroidLite.Libraries.AsteroidUtils.FixRigMaterialESPColors(vrrig);
                                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                                        vrrig.mainSkin.material.color = new Color(1f, 0.3f, 0.1f, 0.5f);
                                    }
                                    else
                                    {
                                        vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                                        vrrig.mainSkin.material.color = vrrig.playerColor;
                                    }
                                }
                            }
                            else
                            {
                                if (!vrrig.isOfflineVRRig)
                                {
                                    if (RigManager.PlayerIsTagged(vrrig))
                                    {
                                        AsteroidLite.Libraries.AsteroidUtils.FixRigMaterialESPColors(vrrig);
                                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                                        vrrig.mainSkin.material.color = new Color(1f, 0.3f, 0.1f, 0.5f);
                                    }
                                    else
                                    {
                                        vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                                        vrrig.mainSkin.material.color = vrrig.playerColor;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (RigManager.PlayerIsTagged(GorillaTagger.Instance.offlineVRRig))
                            {
                                if (!vrrig.isOfflineVRRig)
                                {
                                    AsteroidLite.Libraries.AsteroidUtils.FixRigMaterialESPColors(vrrig);
                                    if (!RigManager.PlayerIsTagged(vrrig))
                                    {

                                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                                        vrrig.mainSkin.material.color = new Color(0.45f, 0f, 1f, 0.5f);
                                    }
                                    else
                                    {
                                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                                        vrrig.mainSkin.material.color = new Color(1f, 0.3f, 0.1f, 0.5f);
                                    }
                                }
                            }
                            else
                            {
                                if (!vrrig.isOfflineVRRig)
                                {
                                    AsteroidLite.Libraries.AsteroidUtils.FixRigMaterialESPColors(vrrig);
                                    if (RigManager.PlayerIsTagged(vrrig))
                                    {
                                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                                        vrrig.mainSkin.material.color = new Color(1f, 0.3f, 0.1f, 0.5f);
                                    }
                                    else
                                    {
                                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                                        vrrig.mainSkin.material.color = new Color(0.45f, 0f, 1f, 0.5f);
                                    }
                                }
                            }
                        }
                    }
                    DontRepeat1 = true;
                }
                else
                {
                    if (DontRepeat1)
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (!vrrig.isOfflineVRRig)
                            {
                                vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                                vrrig.mainSkin.material.color = vrrig.playerColor;
                            }
                        }
                    DontRepeat1 = false;
                }
                if (AntiReport && PhotonNetwork.InRoom)
                {
                    foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                    {
                        if (line.linePlayer == NetworkSystem.Instance.LocalPlayer)
                        {

                            Transform report = line.reportButton.gameObject.transform;
                            if (AntiReportVis)
                            {
                                AsteroidLite.Libraries.AsteroidUtils.VisualizeAura(report.position + new Vector3(-0.1f, 0f, -0.1f), AntiReportRange, Color.red);
                            }
                            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                            {
                                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                                {
                                    float D1 = Vector3.Distance(vrrig.rightHandTransform.position, report.position + new Vector3(-0.1f, 0f, -0.1f));
                                    float D2 = Vector3.Distance(vrrig.leftHandTransform.position, report.position + new Vector3(-0.1f, 0f, -0.1f));

                                    if (D1 < AntiReportRange || D2 < AntiReportRange)
                                    {
                                        if (AntiReportRejoin)
                                        {
                                            string code = PhotonNetwork.CurrentRoom.Name;
                                            PhotonNetwork.Disconnect();
                                            if (code != null) PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(code, JoinType.Solo);
                                            else Notify.Send("Asteroid", "Room Code came back null", AsteroidOrange);
                                        }
                                        else
                                        {
                                            PhotonNetwork.Disconnect();
                                        }
                                        Notify.Send("Asteroid", $"Attempted Report from {RigManager.GetPlayerFromVRRig(vrrig).NickName}, you were disconnected!", AsteroidOrange);
                                    }
                                }
                            }
                        }
                    }
                }
                if (AntiModerator && PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig.concatStringOfCosmeticsAllowed.Contains("LBAAD.") || vrrig.concatStringOfCosmeticsAllowed.Contains("LBAAK."))
                        {
                            if (LogModIDS)
                            {
                                string fileName = "Moderator ID: " + vrrig.OwningNetPlayer.UserId;
                                if (!Directory.Exists("asteroid"))
                                {
                                    Directory.CreateDirectory("asteroid");
                                }
                                File.WriteAllText(fileName, string.Concat(new string[]
                                {
                                    "Player Name: " + vrrig.OwningNetPlayer.NickName,
                                    "Player ID: " + vrrig.OwningNetPlayer.UserId
                                }));
                            }
                            PhotonNetwork.Disconnect();
                            Notify.Send("Asteroid", $"Moderator named: {RigManager.GetPlayerFromVRRig(vrrig).NickName}, was found you were disconnected!", AsteroidOrange);
                        }
                    }
                }
                if (ExtraVel)
                {
                    if (surfaceOverrides != null)
                    {
                        if (Plugin.TriggerBoost)
                        {
                            if (!Plugin.FlippedTrigger)
                            {
                                if (InputLibrary.LeftTrigger())
                                {
                                    foreach (GorillaSurfaceOverride gorillaSurfaceOverride in surfaceOverrides)
                                    {
                                        if (gorillaSurfaceOverride.extraVelMaxMultiplier != ExtraVelMax || gorillaSurfaceOverride.extraVelMultiplier != ExtraVelMin)
                                        {
                                            gorillaSurfaceOverride.extraVelMaxMultiplier = ExtraVelMax;
                                            gorillaSurfaceOverride.extraVelMultiplier = ExtraVelMin;
                                        }
                                    }
                                    return;
                                }
                                foreach (GorillaSurfaceOverride gorillaSurfaceOverride in surfaceOverrides)
                                {
                                    if (gorillaSurfaceOverride.extraVelMaxMultiplier != 1f || gorillaSurfaceOverride.extraVelMultiplier != 1f)
                                    {
                                        gorillaSurfaceOverride.extraVelMaxMultiplier = 1f;
                                        gorillaSurfaceOverride.extraVelMultiplier = 1f;
                                    }
                                }
                            }
                            else
                            {
                                if (InputLibrary.LeftTrigger())
                                {
                                    foreach (GorillaSurfaceOverride gorillaSurfaceOverride in surfaceOverrides)
                                    {
                                        if (gorillaSurfaceOverride.extraVelMaxMultiplier != 1f || gorillaSurfaceOverride.extraVelMultiplier != 1f)
                                        {
                                            gorillaSurfaceOverride.extraVelMaxMultiplier = 1f;
                                            gorillaSurfaceOverride.extraVelMultiplier = 1f;
                                        }
                                    }
                                    return;
                                }
                                foreach (GorillaSurfaceOverride gorillaSurfaceOverride in surfaceOverrides)
                                {
                                    if (gorillaSurfaceOverride.extraVelMaxMultiplier != ExtraVelMax || gorillaSurfaceOverride.extraVelMultiplier != ExtraVelMin)
                                    {
                                        gorillaSurfaceOverride.extraVelMaxMultiplier = ExtraVelMax;
                                        gorillaSurfaceOverride.extraVelMultiplier = ExtraVelMin;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (GorillaSurfaceOverride gorillaSurfaceOverride in surfaceOverrides)
                            {
                                if (gorillaSurfaceOverride.extraVelMaxMultiplier != ExtraVelMax || gorillaSurfaceOverride.extraVelMultiplier != ExtraVelMin)
                                {
                                    gorillaSurfaceOverride.extraVelMaxMultiplier = ExtraVelMax;
                                    gorillaSurfaceOverride.extraVelMultiplier = ExtraVelMin;
                                }
                            }
                        }
                    }
                    else
                    {
                        surfaceOverrides = UnityEngine.Object.FindObjectsOfType<GorillaSurfaceOverride>();
                    }
                }
                if (RecRoom)
                {
                    if (InputLibrary.LeftJoystickMoveX() > 0.2f)
                    {
                        GTPlayer.Instance.transform.position += GTPlayer.Instance.bodyCollider.transform.forward * RecRoomPower * Time.deltaTime;
                        GTPlayer.Instance.transform.position += GTPlayer.Instance.bodyCollider.transform.right * RecRoomPower * Time.deltaTime;
                    }
                    if (InputLibrary.LeftJoystickMoveX() > -0.2f)
                    {
                        GTPlayer.Instance.transform.position += GTPlayer.Instance.bodyCollider.transform.forward * -RecRoomPower * Time.deltaTime;
                        GTPlayer.Instance.transform.position += GTPlayer.Instance.bodyCollider.transform.right * -RecRoomPower * Time.deltaTime;
                    }
                }
                if (DisableWind)
                {
                    if (forceVolumes != null)
                    {
                        foreach (ForceVolume forceVolume in forceVolumes)
                        {
                            forceVolume.enabled = false;
                        }
                    }
                    else
                    {
                        forceVolumes = UnityEngine.Object.FindObjectsOfType<ForceVolume>();
                    }
                }
                else
                {
                    if (forceVolumes != null)
                    {
                        foreach (ForceVolume forceVolume in forceVolumes)
                        {
                            forceVolume.enabled = true;
                        }
                    }
                    else
                    {
                        forceVolumes = UnityEngine.Object.FindObjectsOfType<ForceVolume>();
                    }
                }
                if (NoTagFreeze)
                {
                    GTPlayer.Instance.disableMovement = false;
                }
            }
        }
    }
}
