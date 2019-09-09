using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Ending Collect에 생성될 각각 Slot에 적용
    /// </summary>
    public class CollectSlotMove : InputJoystick2
    {
 
        /// <summary>
        /// Ending_Collect
        /// </summary>
        private GameObject canvasObject;

        /// <summary>
        /// Button_Ending_Collect
        /// </summary>
        private GameObject backObject;
        

        /// <summary>
        /// endingScene이 확대되어 켜져있을 때 a,b버튼이 활성화되지 않도록.
        /// </summary>
        private bool sceneOn = false;

        private bool sceneCreate = false;

        //씬슬롯 안에 적을 텍스트
        public Text numberTxt;

        // Use this for initialization
        void Start()
        {
            myObject = this.gameObject;
            myNum = int.Parse(myObject.name);
            numberTxt.text = myNum.ToString();

            //스크립트의 순서가 sendMessage를 받고 start()함수를 실행하는 듯. 그러므로 InsertNum()함수는 여기서 실행.
            InsertNum();
            canvasObject = GameObject.Find("Ending_Collect");
            parentObject = GameObject.Find("BackGround");
            backObject = GameObject.Find("Button_Ending_Collect");

            Initiate();
        }

        /// <summary>
        /// 메뉴화면으로 돌아갔다가 다시 EndingCollect를 누를 경우 모든 slot을 초기화.
        /// </summary>
        public void Initiate()
        {
            buttonOn = false;
            sceneOn = false;

            if (myNum == 1)
            {
                obs = slotState.highlight;
                buttonOn = true;
            }
            else
            {
                obs = slotState.empty;
                buttonOn = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if ((InputManager.MainVertical() == 0.0f) && (InputManager.MainHorizontal() == 0.0f))
            {
                buttonOn = true;
            }

            if (buttonOn)
            {
                switch (obs)
                {
                    case slotState.empty:
                        PointerExit(fillIn);
                        break;
                    case slotState.highlight:
                        PointerOver(fillIn);
                        break;
                    case slotState.check:
                        PointerCheck(fillIn);
                        break;
                    case slotState.fillItem:
                        PointerFill(fillIn);
                        PointerExit(fillIn);
                        break;
                    case slotState.fillHighlight:
                        PointerOver(fillIn);
                        break;
                    case slotState.fillCheck:
                        PointerCheck(fillIn);
                        break;
                }
            }
        }

        /// <summary>
        /// object가 highlight활성화 상태일때
        /// </summary>
        protected override void PointerOver(bool fIn)
        {
            //slot에 Item이 들어와있으면 itemHighlight텍스쳐를, 없으면 slotHighlight텍스쳐
            if (fIn)
            {
                GetComponent<Image>().sprite = HighlightTexture;
            }
            else
            {
                GetComponent<Image>().sprite = slotHighlight;
            }
            //up & down
            if (InputManager.MainVertical() == 1.0f)
            {
                upObject.SendMessage("CallButtonOn");
                obs = slotState.empty;
                buttonOn = false;
            }
            else if (InputManager.MainVertical() == -1.0f)
            {
                downObject.SendMessage("CallButtonOn");
                obs = slotState.empty;
                buttonOn = false;
            }
            
            //left & right
            if (InputManager.MainHorizontal() == 1.0f)
            {
                rightObject.SendMessage("CallButtonOn");
                obs = slotState.empty;
                buttonOn = false;
            }
            else if (InputManager.MainHorizontal() == -1.0f)
            {
                leftObject.SendMessage("CallButtonOn");
                obs = slotState.empty;
                buttonOn = false;

            }

            if (!sceneOn)
            {
                if (InputManager.AButton())
                {
                    if (fIn)
                    {
                        obs = slotState.fillCheck;
                    }
                    else
                    {
                        obs = slotState.check;
                    }
                    sceneOn = true;
                    sceneCreate = true;
                }
                if (InputManager.BButton())
                {
                    //Debug.Log("뒤로가기" + sceneOn + myNum);
                    //button_ending_collect로 sendmessage
                    backObject.SendMessage("MenuClose");
                    canvasObject.SetActive(false);

                }
            }
        }

        /// <summary>
        /// object를 선택했을 때
        /// </summary>
        protected override void  PointerCheck(bool fIn)
        {
            if (sceneCreate)
            {
                parentObject.SendMessage("CreateScene", myNum);
                sceneCreate = false;
            }

            if (fIn)
            {
                GetComponent<Image>().sprite = CheckTexture;
            }
            else
            {
                GetComponent<Image>().sprite = slotCheck;
            }
        }

        /// <summary>
        /// scene이 켜져있다가 꺼질 경우 다시 그 sceneSlot이 활성화되도록.
        /// </summary>
        public void FromScene()
        {
            obs = slotState.highlight;
            sceneOn = false;
        }
    }
}