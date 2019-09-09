using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class MouseLook : MonoBehaviour
    {
        public float xmouseSensitivity = 100.0f;
        public float zmouseSensitivity = 100.0f;

        private float rotY = 0.0f; // rotation around the up/y axis
        private float rotX = 0.0f; // rotation around the right/x axis
        public float Y_MinLimit = -40.0f;
        public float Y_MaxLimit = 80.0f;
        public float X_MinLimit = -40.0f;
        public float X_MaxLimit = 80.0f;

        void Start()
        {
            Vector3 rot = transform.localRotation.eulerAngles;
            rotY = rot.y;
            rotX = rot.x;
        }

        void LateUpdate()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            // this is where the mouseY is limited - Helper script
            mouseY = ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

            rotY += mouseX * xmouseSensitivity * Time.deltaTime;
            rotX += mouseY * zmouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, X_MinLimit, X_MaxLimit);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;
        }


        public float ClampAngle(float angle, float min, float max)
        {
            while (angle < -360 || angle > 360)
            {
                if (angle < -360)
                    angle += 360;
                if (angle > 360)
                    angle -= 360;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}