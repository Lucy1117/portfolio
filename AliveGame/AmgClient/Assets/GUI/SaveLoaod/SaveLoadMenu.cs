using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 게임 내 B버튼 눌렀을 때 나타나는 BMenu의 하위 메뉴인 Save, Load
    /// 각각 Save & Load 에 맞게 만들면 될 듯.
    /// </summary>
    public class SaveLoadMenu : InputJoystick
    {
        public enum SaveOrLoad
        {
            SaveBt,
            LoadBt
        };

        /// <summary>
        /// SaveLoad가 열려서 활성화되도 되는지를 알려주는 변수
        /// </summary>
        private bool gameStop;

        private GameObject initObj;

        public SaveOrLoad saveorload;

        private string beforeButton;

        /// <summary>
        /// Abutton누르고도 계속 활성화되도록
        /// </summary>
        private bool checkTrue;

        public GameObject chapterNameObj;
        public GameObject playSpotObj;
        public GameObject playTimeObj;

        private Text chapterNameTxt;
        private Text playSpotTxt;
        private Text playTimeTxt;

        // Use this for initialization
        void Start()
        {
            initObj = GameObject.Find("Initiate");
            chapterNameTxt = chapterNameObj.GetComponent<Text>();
            playSpotTxt = playSpotObj.GetComponent<Text>();
            playTimeTxt = playTimeObj.GetComponent<Text>(); 

            if (myButton.GetComponent<InputJoystick>().bNameTag == buttonNameTag.first)
            {
                bs = buttonState.highlight;
                moveButton = true;
            }
            else
            {
                bs = buttonState.original;
            }
            gameStop = false;
            checkTrue = false;
        }

        /// <summary>
        /// SaveLoad버튼이 활성화되면 호출되어야 하는 함수
        /// </summary>
        public void Initiate()
        {
            gameStop = true;
            moveButton = false;
            menuIn = true;
            checkTrue = false;

            if (myButton.GetComponent<InputJoystick>().bNameTag == buttonNameTag.first)
            {
                bs = buttonState.highlight;
                moveButton = true;
            }
            else
            {
                bs = buttonState.original;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (InputManager.MainVertical() == 0.0f)
            {
                moveButton = true;
            }
            if (moveButton && gameStop)
            {
                if (bs == buttonState.highlight)
                {
                    GetComponent<Image>().sprite = HighlightTexture;
                    PointerOver();
                    if (InputManager.BButton())
                    {///끌 경우
                        Initiate();
                        InGameMenuOff();
                    }
                }
                else if (bs == buttonState.check)
                {
                    GetComponent<Image>().sprite = CheckTexture;
                    PointerCheck();
                }
                else if (bs == buttonState.original)
                {
                    GetComponent<Image>().sprite = OriginalTexture;
                    PointerExit();
                    if (checkTrue)
                    {
                        bs = buttonState.highlight;
                        moveButton = true;
                        checkTrue = false;
                        menuIn = true;
                    }
                }
            }
        }

        /// <summary>
        /// 1
        /// </summary>
        protected override void FirstButtonFun()
        {
            Debug.Log("FirstButtonFun");
            ButtonTagFun("1");
        }

        /// <summary>
        /// 2
        /// </summary>
        protected override void SecondButtonFun()
        {
            Debug.Log("SecondButtonFun");
            ButtonTagFun("2");
        }

        /// <summary>
        /// 3
        /// </summary>
        protected override void ThirdButtonFun()
        {
            Debug.Log("ThirdButtonFun");
            ButtonTagFun("3");
        }

        /// <summary>
        /// 4
        /// </summary>
        protected override void ForthButtonFun()
        {
            Debug.Log("ForthButtonFun");
            ButtonTagFun("4");
        }

        /// <summary>
        /// 5
        /// </summary>
        protected override void FifthButtonFun()
        {
            Debug.Log("FifthButtonFun");
            ButtonTagFun("5");
        }

        /// <summary>
        /// 버튼 공통 함수
        /// </summary>
        /// <param name="fileNum"></param>
        private void ButtonTagFun(string fileNum)
        {
            if (saveorload == SaveOrLoad.SaveBt)
            {
                initObj.SendMessage("CallSave", fileNum);
            }
            else if (saveorload == SaveOrLoad.LoadBt)
            {
                InGameMenuOff();
                if (GameObject.Find("BMenuBackGround"))
                {
                    GameObject.Find("BMenuBackGround").SendMessage("BDown");
                }
                initObj.SendMessage("CallLoad", fileNum);
            }
            checkTrue = true;
        }

        public void InGameMenuOn(string sendName)
        {
            Initiate();
            beforeButton = sendName;
        }

        /// <summary>
        /// SaveLoad메뉴 나가서 BMenu로
        /// </summary>
        public void InGameMenuOff()
        {
            gameStop = false;
            this.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
            
            GameObject bMenuObj = GameObject.Find(beforeButton);
            bMenuObj.SendMessage("MenuClose");
        }

        /// <summary>
        /// 이 알고리즘의 첫번째는, InGameMenu.cs의 Update()함수 속 initObj.SendMessage("VisibleInformRead")에서 시작.
        /// 현재의 함수는 InitInterface.cs에서 호출. 버튼에 가시적으로 표현할 정보를 받아옴. 
        /// 알고리즘의 가장 마지막 순서
        /// </summary>
        public void InformationVisible()
        {
            chapterNameTxt.text = initObj.GetComponent<InitInterface>().chapterNameStr;
            playSpotTxt.text = initObj.GetComponent<InitInterface>().currentSpotStr;
            playTimeTxt.text = initObj.GetComponent<InitInterface>().playTimeStr;
        }
    }
}