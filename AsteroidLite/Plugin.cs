using AsteroidLite.Libraries;
using BepInEx;
using GorillaLocomotion;
using Photon.Pun;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AsteroidLite
{
    [BepInPlugin("Ventern.AsteroidLite", "Ventern - AsteroidLite", "0.7")]
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
                string str2 = "Asteroid Lite | " + AsteroidLite.Libraries.Utilities.GetFPS() + " | " + str;
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
        private static GorillaSurfaceOverride[] surfaceOverrides;
        #endregion BackEnd Fields

        #region Bool/Floats/Ints fields
        private static bool SpeedBoost = false;
        private static bool TriggerBoost = false;
        private static bool FlippedTrigger = false;
        private static float MaxJump = 6.5f;
        private static float JumpMulti = 1.1f;
        private static bool WallWalk = false;
        private static float WallWalkPower = 1f;
        private static float WallWalkPowerNeg = -1f;
        private static bool TagAura = false;
        private static float TagAuraRange = 1f;
        private static bool Longjump = false;
        private static float LongjumpMulti = 1f;
        private static bool Longarms = false;
        private static float LongarmsMulti = 1f;
        private static bool PSA = false;
        private static float PSASpeed = 1f;
        private static bool GripToLag = false;
        private static float LagAmmount = 1f;
        private static bool Chams = false;
        private static bool TargetChams = false;
        private static bool AntiReport = false;
        private static bool AntiReportVis = false;
        private static float AntiReportRange = 0.35f;
        private static bool AntiModerator = false;
        private static bool LogModIDS = false;
        private static bool ExtraVel = false;
        private static float ExtraVelMax = 1f;
        private static float ExtraVelMin = 1f;
        private static bool RecRoom = false;
        private static float RecRoomPower = 1f;
        private static bool DisableWind = false;
        private static bool NoTagFreeze = false;
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
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.AntiModerator ? 114 : 115, false, 0.2f);
                }
                if (GUILayout.Button($"NoTagFreeze: {Plugin.NoTagFreeze}", GUILayout.MaxWidth(420f)))
                {
                    Plugin.NoTagFreeze = !Plugin.NoTagFreeze;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.AntiModerator ? 114 : 115, false, 0.2f);
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
            Notify.Run();
            if (!this.IsInit && Player.Instance && PhotonNetwork.LocalPlayer != null && Plugin.GUIStyleInit)
            {
                if (this.Holder != null)
                {
                    if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("AsteroidLite"))
                    {
                        PhotonNetwork.LocalPlayer.CustomProperties.Add("AsteroidLite", "AsteroidLite");
                    }
                    AsteroidLite.Libraries.Utilities.CustomBoards();
                    Debug.Log("[Asteroid] Menu initialization Complete!");
                    this.IsInit = true;
                }
                else
                {
                    Debug.Log("[Asteroid] Awaiting InputLibrary");
                    this.Holder = new GameObject("Holder");
                    this.Holder.AddComponent<InputLibrary>();
                    Debug.Log("[Asteroid] Input Library Loaded");
                }
            }
            if (this.IsInit)
            {
                if (UnityInput.Current.GetKeyDown(KeyCode.Insert))
                {
                    GUIOpen = !GUIOpen;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Plugin.GUIOpen ? 114 : 115, false, 0.2f);
                    Debug.Log(Plugin.GUIOpen ? "[Asteroid] Menu Opened" : "[Asteroid] Menu Closed");
                }
                if (Plugin.SpeedBoost)
                {
                    if (Plugin.TriggerBoost)
                    {
                        if (!Plugin.FlippedTrigger)
                        {
                            if (InputLibrary.LeftTrigger())
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
                            if (InputLibrary.LeftTrigger())
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
                if (Plugin.WallWalk)
                {
                    if (Player.Instance.wasLeftHandColliding || Player.Instance.wasRightHandColliding)
                    {
                        RaycastHit raycastHit = (RaycastHit)typeof(Player).GetField("lastHitInfoHand", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Player.Instance);
                        Plugin.walkPos = raycastHit.point;
                        Plugin.walkNormal = raycastHit.normal;
                    }
                    if (InputLibrary.RightGrip())
                    {
                        if (Plugin.walkPos != Vector3.zero)
                        {
                            Plugin.WallWalkPowerNeg = -Plugin.WallWalkPower;
                            Player.Instance.bodyCollider.attachedRigidbody.AddForce(Plugin.walkNormal * Plugin.WallWalkPowerNeg, ForceMode.Acceleration);
                            Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (Plugin.WallWalkPower / Time.deltaTime)), ForceMode.Acceleration);
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
                                if (!RigManager.PlayerIsTagged(vrrig) && !Player.Instance.disableMovement && num < Plugin.TagAuraRange)
                                {
                                    Player.Instance.rightControllerTransform.position = position;
                                }
                            }
                        }
                    }
                }
                if (Plugin.Longjump)
                {
                    if (InputLibrary.RightTrigger())
                    {
                        Player.Instance.maxJumpSpeed = 10f;
                        Player.Instance.jumpMultiplier = Plugin.JumpMulti * Plugin.LongjumpMulti;
                        return;
                    }
                    Player.Instance.maxJumpSpeed = Plugin.MaxJump;
                    Player.Instance.jumpMultiplier = Plugin.JumpMulti;
                }
                if (Plugin.Longarms)
                {
                    Player.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position) * Plugin.LongarmsMulti;
                    Player.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position) * Plugin.LongarmsMulti;
                }
                if (Plugin.PSA)
                {
                    if (InputLibrary.LeftJoystickMoveY() > 0.2f)
                        Player.Instance.transform.position += Player.Instance.bodyCollider.transform.forward * PSASpeed * Time.deltaTime;
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
                                        AsteroidLite.Libraries.Utilities.FixRigMaterialESPColors(vrrig);
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
                                        AsteroidLite.Libraries.Utilities.FixRigMaterialESPColors(vrrig);
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
                                    AsteroidLite.Libraries.Utilities.FixRigMaterialESPColors(vrrig);
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
                                    AsteroidLite.Libraries.Utilities.FixRigMaterialESPColors(vrrig);
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
                        if (!vrrig.isOfflineVRRig )
                        vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                        vrrig.mainSkin.material.color = vrrig.playerColor;
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
                                AsteroidLite.Libraries.Utilities.VisualizeAura(report.position + new Vector3(-0.1f, 0f, -0.1f), AntiReportRange, Color.red);
                            }
                            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                            {
                                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                                {
                                    float D1 = Vector3.Distance(vrrig.rightHandTransform.position, report.position + new Vector3(-0.1f, 0f, -0.1f));
                                    float D2 = Vector3.Distance(vrrig.leftHandTransform.position, report.position + new Vector3(-0.1f, 0f, -0.1f));

                                    if (D1 < AntiReportRange || D2 < AntiReportRange)
                                    {
                                        PhotonNetwork.Disconnect();
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
                        Player.Instance.transform.position += Player.Instance.bodyCollider.transform.forward * RecRoomPower *  Time.deltaTime;
                        Player.Instance.transform.position += Player.Instance.bodyCollider.transform.right * RecRoomPower *  Time.deltaTime;
                    }
                    if (InputLibrary.LeftJoystickMoveX() > -0.2f)
                    {
                        Player.Instance.transform.position += Player.Instance.bodyCollider.transform.forward * -RecRoomPower * Time.deltaTime;
                        Player.Instance.transform.position += Player.Instance.bodyCollider.transform.right * -RecRoomPower * Time.deltaTime;
                    }
                }
                if (DisableWind)
                {
                    GameObject force = GameObject.Find("Forest_ForceVolumes").gameObject;
                    force.SetActive(false);
                }
                else
                {
                    GameObject force = GameObject.Find("Forest_ForceVolumes").gameObject;
                    force.SetActive(true);
                }
                if (NoTagFreeze)
                {
                    Player.Instance.disableMovement = false;
                }
            }
        }
    }
}
