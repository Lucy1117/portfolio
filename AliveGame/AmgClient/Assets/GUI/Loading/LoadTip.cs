using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class LoadTip : MonoBehaviour
    {
        private GameObject myObject;
        private RectTransform myTransform;

        public Canvas canvas;
        private RectTransform canvasTransform;

        public GameObject textObject;
        
        private Text textSize;

        private float textTipWidth;
        //private float textTipHeight;
        //textTipWidth는 제대로 값이 계산되어 나오는데, textTimHeight는 완전 무관한 값이 전송되서 임의로 값을 지정해줌.
        //그 이유는 모르겠음.


        private float canvasWidth;
        private float canvasHeight;

        private float canvasXPos = 0.0f;
        private float canvasYPos = 0.0f;

        private int textNumber = 0;

        private List<string> tipList = new List<string>();

        private void Awake()
        {
            myObject = this.gameObject;
            myTransform = myObject.GetComponent<RectTransform>();
            canvasTransform = canvas.GetComponent<RectTransform>();
            textSize = textObject.GetComponent<Text>();
            tipList.Add("Y버튼을 누르면 인벤토리가 열립니다.");
            tipList.Add("로딩중입니다.");
            tipList.Add("B버튼을 누르면 게임 메뉴를 열 수 있습니다.");
            tipList.Add("A버튼을 누르면 선택할 수 있습니다.");
            tipList.Add("A버튼을 누르면 아이템을 사용할 수 있습니다.");
            tipList.Add("도착한 메세지는 인벤토리에서 \n X키를 눌러 확인할 수 있습니다.");
            tipList.Add("조이스틱의 상단 키를 좌우로 누르면 \n사용 중인 아이템이 바뀝니다.");
        }

        // Use this for initialization
        void Start()
        {
            float textheight = textSize.fontSize;

            textNumber = Random.Range(0, tipList.Count);

            //현재 2줄인 게 6번째 인자이므로, 5 이상이면 두줄로.
            if (textNumber > 5)
            {
                //키값쌍(key:int, value:string)
                textheight = textSize.fontSize * 3.0f;
            }
            else
            {
                textheight = textSize.fontSize * 1.5f;
            }

            textSize.text = tipList[textNumber];
            textTipWidth = textSize.preferredWidth;
            //textTipHeight = textSize.preferredHeight; 
            canvasWidth = (canvasTransform.rect.width);
            canvasHeight = (canvasTransform.rect.height);
            canvasXPos = canvasWidth - canvasWidth;
            canvasYPos = -canvasHeight/2;
            //myTransform.localPosition = new Vector3(canvasWidth, canvasHeight);
            ///생성 위치
            myTransform.localPosition = new Vector3(canvasXPos, canvasYPos * 0.75f);

            //생성 크기
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textTipWidth * 1.2f);
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textheight);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
