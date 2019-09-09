using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{

    public class CanvasTextPosition : MonoBehaviour
    {
        private GameObject myObject;
        private RectTransform myTransform;

        public Canvas canvas;
        private RectTransform canvasTransform;
        
        private float canvasWidth;
        private float canvasHeight;

        private float canvasXPos = 0.0f;
        private float canvasYPos = 0.0f;

        public float myXSize;
        public float myYSize;
        public float myPosition = 1.0f;


        private void Awake()
        {
            myObject = this.gameObject;
            myTransform = myObject.GetComponent<RectTransform>();
            canvasTransform = canvas.GetComponent<RectTransform>();
        }

        // Use this for initialization
        void Start()
        {
            
            //textTipHeight = textSize.preferredHeight; 
            canvasWidth = (canvasTransform.rect.width);
            canvasHeight = (canvasTransform.rect.height);
            canvasXPos = canvasWidth - canvasWidth;
            canvasYPos = -canvasHeight / 2;
            //myTransform.localPosition = new Vector3(canvasWidth, canvasHeight);
            ///생성 위치
            myTransform.localPosition = new Vector3(canvasXPos, canvasYPos * myPosition);

            //생성 크기
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, myXSize);
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, myYSize);
            //myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasWidth / 3.0f);
            //myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasHeight / 3.0f);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

