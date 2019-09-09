using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class TipButtonRot : MonoBehaviour
    {
        private GameObject myObject;
        public float angleSpeed;

        // Use this for initialization
        void Start()
        {
            myObject = this.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if(myObject.GetComponent<CanvasGroup>().alpha != 0)
            {
                myObject.transform.Rotate(0.0f, angleSpeed * Time.deltaTime, 0.0f);
            }
        }
    }
}
