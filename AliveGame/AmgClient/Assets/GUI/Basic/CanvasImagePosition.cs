using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{

    public class CanvasImagePosition : MonoBehaviour
    {

        private GameObject myObject;
        private RectTransform myTransform;

        public Canvas canvas;
        private RectTransform canvasTransform;

        private float canvasWidth;
        private float canvasHeight;

        /// <summary>
        /// 사용할 이미지 사이즈
        /// </summary>
        public float myXSize;
        public float myYSize;

        private float canvasZeroX;
        private float canvasZeroY;

        /// <summary>
        /// 캔버스 비율. 0 ~ 1.0 사이의 값을 적을 수 있다.
        /// 정 가운데를 (0,0)으로 두고 각 비율을 계산.
        /// </summary>
        public float canvasXPos;
        /// <summary>
        /// 캔버스 비율. -1.0 ~ 1.0 사이의 값을 적을 수 있다.
        /// 정 가운데를 (0,0)으로 두고 각 비율을 계산.
        /// </summary>
        public float canvasYPos;
        
        private void Awake()
        {
            myObject = this.gameObject;
            myTransform = myObject.GetComponent<RectTransform>();
            canvasTransform = canvas.GetComponent<RectTransform>();

        }

        // Use this for initialization
        void Start()
        {
            canvasWidth = (canvasTransform.rect.width);
            canvasHeight = (canvasTransform.rect.height);

            canvasZeroX = -(canvasWidth / 2);
            canvasZeroY = (canvasHeight / 2);

            myTransform.localPosition = new Vector3(canvasZeroX + canvasWidth * canvasXPos, canvasZeroY - canvasHeight * canvasYPos);
           
            //생성 크기
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, myXSize);
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, myYSize);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
