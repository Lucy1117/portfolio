using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class InputManager
    {

        // --방향축(키) 설정--
        public static float MainHorizontal()
        {
            float r = 0.0f;
            r += Input.GetAxis("J_MainHorizontal");
            r += Input.GetAxis("K_MainHorizontal");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        public static float MainVertical()
        {
            float r = 0.0f;
            r += Input.GetAxis("J_MainVertical");
            r += Input.GetAxis("K_MainVertical");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        public static Vector3 MainJoystick()
        {
            return new Vector3(MainHorizontal(), 0, MainVertical());
        }

        public static float RotateAxis()
        {
            float r = 0.0f;
            r += Input.GetAxis("Left_AxisTrigger");
            r -= Input.GetAxis("Right_AxisTrigger");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        public static float VerticalRotateAxis()
        {
            float r = 0.0f;
            r += Input.GetAxis("K_UD_AxisTrigger");
            r += Input.GetAxis("J_UD_AxisTrigger");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        public static float ItemSelectTrigger()
        {
            float r = 0.0f;
            r += Input.GetAxis("Item_Select");
            r += Input.GetAxis("K_Item_Select");

            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        // --방향축(키) 설정--
        public static float ServerHorizontal()
        {
            float r = 0.0f;
            r += Input.GetAxis("Server_Horizontal");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        public static float ServerVertical()
        {
            float r = 0.0f;
            r += Input.GetAxis("Server_Vertical");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        // --버튼 설정--

        public static bool AButton()
        {
            return Input.GetButtonDown("A_Button");
        }

        public static bool BButton()
        {
            return Input.GetButtonDown("B_Button");
        }

        public static bool XButton()
        {
            return Input.GetButtonDown("X_Button");
        }

        public static bool XButtonUp()
        {
            return Input.GetButtonUp("X_Button");
        }

        public static bool YButton()
        {
            return Input.GetButtonDown("Y_Button");
        }

        public static bool InvButton()
        {
            return Input.GetButtonDown("Inv_Button");
        }


    }
}