using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 게임 내에서 B버튼을 누르면 활성화되는 게임 내 메뉴 버튼
    /// 이 cs 안에서는 Awake()함수를 쓰면 에러남. Start함수에서 에러.
    /// </summary>
    public class InGameMenuButton : InputJoystick
    {
        /// <summary>
        /// 게임메뉴가 열리면 game이 멈추므로, 이를 나타내주는 변수.
        /// </summary>
        private bool gameStop;

        /// <summary>
        /// InGameMenu의 Background
        /// </summary>
        private GameObject parentObject;


        /// <summary>
        /// SceneDataObject
        /// </summary>
        private GameObject sceneDataObj;


        // Use this for initialization
        void Start()
        {
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

            parentObject = GameObject.Find("BMenuBackGround");

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
        }

        /// <summary>
        /// InGameMenu가 활성화되면 호출되어야 하는 함수
        /// </summary>
        public void Initiate()
        {
            gameStop = true;
            moveButton = false;
            menuIn = true;

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
                    {
                        parentObject.SendMessage("BDown");
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
                }
            }
        }

        /// <summary>
        /// Regame
        /// </summary>
        protected override void FirstButtonFun()
        {
            parentObject.SendMessage("BDown");
            InGameMenuOff();
            Initiate();
            Debug.Log("FirstButtonFun");
        }

        /// <summary>
        /// SaveGame
        /// </summary>
        protected override void SecondButtonFun()
        {
            GameObject.Find("Inventory").SendMessage("ADown");
            SaveMenuOpen();
            Debug.Log("SecondButtonFun");
        }

        /// <summary>
        /// LoadGame
        /// </summary>
        protected override void ThirdButtonFun()
        {
            GameObject.Find("Inventory").SendMessage("ADown");
            LoadMenuOpen();
            Debug.Log("ThirdButtonFun");
        }

        /// <summary>
        /// GoMainMenu
        /// </summary>
        protected override void ForthButtonFun()
        {
            sceneDataObj.SendMessage("LoadSceneData", SceneName.StartMenu);
            Debug.Log("ForthButtonFun");
        }

        /// <summary>
        /// Option
        /// </summary>
        protected override void FifthButtonFun()
        {
            Debug.Log("FifthButtonFun");
        }

        /// <summary>
        /// Exit Game
        /// </summary>
        protected override void SixthButtonFun()
        {
            Debug.Log("FifthButtonFun");
            Application.Quit();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InGameMenuOff()
        {
            gameStop = false;
        }

        /// <summary>
        /// Save메뉴가 열린 경우
        /// </summary>
        public void SaveMenuOpen()
        {
            GameObject saveObj = GameObject.Find("SaveBackGround");
            saveObj.GetComponent<CanvasGroup>().alpha = 1;
            saveObj.BroadcastMessage("InGameMenuOn", this.gameObject.name);
            moveButton = false;
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
            moveButton = true;
        }
    }
}