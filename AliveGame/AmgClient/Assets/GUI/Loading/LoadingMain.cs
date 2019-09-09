using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class LoadingMain : MonoBehaviour
    {
        public Canvas canvas;
        private RectTransform canvasTransform;

        private GameObject myObject;
        private RectTransform myTransform;

        private float canvasWidth;
        private float canvasHeight;

        private void Awake()
        {
            myObject = this.gameObject;
            myTransform = myObject.GetComponent<RectTransform>();
            canvasTransform = canvas.GetComponent<RectTransform>();
            canvasWidth = canvasTransform.rect.width;
            canvasHeight = canvasTransform.rect.height;
            
        }

        // Use this for initialization
        void Start()
        {
            //myTransform.localPosition = canvasTransform.localPosition; 
            
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasWidth / canvas.scaleFactor);
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasHeight / canvas.scaleFactor);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
