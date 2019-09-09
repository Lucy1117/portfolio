using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 메뉴화면 첫번째에서 버튼 간의 이동에 쓰임.
    /// Canvas에 있는 Button_New_Game부터 Button_Exit까지.
    /// 이 cs 안에서는 Awake()함수를 쓰면 에러남. Start함수에서 에러.
    /// </summary>
    public class InputStartButton : InputJoystick
    {
        #region SceneLoad에 필요한 변수
        private GameObject sceneDataObj;
        #endregion

        private GameObject initObj;


        //private buttonState bs = buttonState.original;
        //public buttonNameTag bNameTag = buttonNameTag.zero;

        // Use this for initialization
        void Start()
        {
            if (myButton.GetComponent<InputJoystick>().bNameTag == buttonNameTag.first)
            {
                bs = buttonState.highlight;
                moveButton = true;
            }
            else
            {
                bs = buttonState.original;
            }
            initObj = GameObject.Find("Initiate");

            if (GameObject.Find("SceneData"))
            {
                sceneDataObj = GameObject.Find("SceneData");
            }
            else
            {
                if (GameObject.Find("SceneData(Clone)"))
                {
                    sceneDataObj = GameObject.Find("SceneData(Clone)");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (InputManager.MainVertical() == 0.0f)
            {
                moveButton = true;
            }
            if (moveButton)
            {
                if (bs == buttonState.highlight)
                {
                    GetComponent<Image>().sprite = HighlightTexture;
                    PointerOver();
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
                }
            }
        }

        /// <summary>
        /// "Button_New_Game"에서 활성화되는 함수
        /// </summary>
        protected override void FirstButtonFun()
        {
            /// 0-Default, 1-Loading, 2-StartMenu, 3-Garden
            Debug.Log("SendMessage(LoadSceneData, SceneName.Garden)");
            sceneDataObj.SendMessage("LoadSceneData", SceneName.Prologue);
            this.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        /// <summary>
        /// "Button_Load_Game"에서 활성화되는 함수
        /// </summary>
        protected override void SecondButtonFun()
        {
            initObj.SendMessage("VisibleInformRead");
            LoadMenuOpen();
            this.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            //loadgame작업
        }
        /// <summary>
        /// "Button_Ending_Collect"에서 활성화되는 함수
        /// </summary>
        protected override void ThirdButtonFun()
        {
            myButton.SendMessage("Collect");
            this.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        /// <summary>
        /// "Button_Option""에서 활성화되는 함수
        /// </summary>
        protected override void ForthButtonFun()
        {
            ToMenu();
            //this.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            //Game내 옵션을 바꾸는 작업
            //보통 감마보정, 음량, 밝기보정, 지원언어 등이 들어감.
        }
        /// <summary>
        /// "Button_Exit"에서 활성화되는 함수
        /// </summary>
        protected override void FifthButtonFun()
        {
            //this.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            Application.Quit();
        }

        /// <summary>
        /// Load메뉴가 열린 경우
        /// </summary>
        public void LoadMenuOpen()
        {
            GameObject loadObj = GameObject.Find("LoadBackGround");
            loadObj.GetComponent<CanvasGroup>().alpha = 1;
            loadObj.BroadcastMessage("InGameMenuOn", this.gameObject.name);
            moveButton = false;
        }

        /// <summary>
        /// SaveMenu 닫힌 경우
        /// </summary>
        public void MenuClose()
        {
            this.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            //moveButton = true;
            ToMenu();
        }
    }
}