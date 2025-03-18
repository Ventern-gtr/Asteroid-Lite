using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace AsteroidLite.Libraries
{
    public class InputLibrary : MonoBehaviour
    {
        public void Update()
        {
            if (InputLibrary.IsSteam && !GameObject.Find("[SteamVR]"))
            {
                Debug.Log("[Input Lib] Oculus Mode enabled");
                InputLibrary.IsSteam = false;
            }
        }

        #region LeftController

        public static float LeftJoystickMove()
        {
            float result;
            if (IsSteam)
            {
                result = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis.x;
            }
            else
            {
                result = ControllerInputPoller.instance.leftControllerPrimary2DAxis.x;
            }
            return result;
        }

        public static bool LeftJoystick()
        {
            bool result;
            if (InputLibrary.IsSteam)
            {
                result = SteamVR_Actions.gorillaTag_LeftJoystickClick.GetState(SteamVR_Input_Sources.LeftHand);
            }
            else
            {
                bool flag;
                ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out flag);
                result = flag;
            }
            return result;
        }

        public static bool LeftSecondary()
        {
            return ControllerInputPoller.instance.leftControllerSecondaryButton;
        }

        public static bool LeftPrimary()
        {
            return ControllerInputPoller.instance.leftControllerPrimaryButton;
        }

        public static bool LeftTrigger()
        {
            return ControllerInputPoller.instance.leftControllerIndexFloat >= 0.7f;
        }

        public static bool LeftGrip()
        {
            return ControllerInputPoller.instance.leftGrab;
        }

        #endregion Left Controller


        #region Right Controller

        public static float RightJoystickMove()
        {
            float result;
            if (IsSteam)
            {
                result = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis.x;
            }
            else
            {
                result = ControllerInputPoller.instance.rightControllerPrimary2DAxis.x;
            }
            return result;
        }
        public static bool RightJoystick()
        {
            bool result;
            if (InputLibrary.IsSteam)
            {
                result = SteamVR_Actions.gorillaTag_RightJoystickClick.GetState(SteamVR_Input_Sources.RightHand);
            }
            else
            {
                bool flag;
                ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out flag);
                result = flag;
            }
            return result;
        }

        public static bool RightSecondary()
        {
            return ControllerInputPoller.instance.rightControllerSecondaryButton;
        }

        public static bool RightPrimary()
        {
            return ControllerInputPoller.instance.rightControllerPrimaryButton;
        }

        public static bool RightTrigger()
        {
            return ControllerInputPoller.instance.rightControllerIndexFloat >= 0.7f;
        }

        public static bool RightGrip()
        {
            return ControllerInputPoller.instance.rightGrab;
        }

        #endregion Right Controller

        public static bool IsSteam = true;
        public static bool IsInit = false;
    }
}
