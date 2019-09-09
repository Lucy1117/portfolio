using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 인벤토리에서 맵을 누르면 맵 이미지가 뜰 수 있게.
    /// phonemessage.cs와 비슷하게 코딩.
    /// </summary>
    public class MapPamphlet : MonoBehaviour
    {

        public GUIState guiStat = GUIState.off;

        private bool buttonOn = false;

        public Sprite mapTexture1;
        public Sprite mapTexture2;

        private Image myImage;
        private int myNum;

        private bool gamePlay;

        private bool errorCheck;

        private GameObject initObj;

        // Use this for initialization
        void Start()
        {
            myImage = this.gameObject.GetComponent<Image>();
            myNum = 1;
            gamePlay = false;
            initObj = GameObject.Find("Initiate");
            errorCheck = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (gamePlay)
            {
                if (InputManager.XButtonUp())
                {
                    //phoneMessage가 활성화된 상태에서 XButton을 누르면
                    ObjectNotActive();
                }

                if (buttonOn)
                {
                    if (InputManager.MainHorizontal() < 0)
                    {
                        ChangeLeftSprite();
                        buttonOn = false;
                    }
                    if (InputManager.MainHorizontal() > 0)
                    {
                        ChangeRightSprite();
                        buttonOn = false;
                    }
                }
                if (InputManager.MainVertical() == 0)
                {
                    buttonOn = true;
                }
            }

            if (errorCheck)
            {
                if (InputManager.XButtonUp())
                {
                    gamePlay = true;
                    errorCheck = false;
                }
            }
        }

        private void ChangeLeftSprite()
        {
            switch (myNum)
            {
                case 1:
                    Debug.Log("1번. 더는 올라갈 곳이 없습니다.");
                    break;
                case 2:
                    myImage.sprite = mapTexture1;
                    myNum = myNum - 1;
                    break;
                case 3:
                    myImage.sprite = mapTexture2;
                    myNum = myNum - 1;
                    break;
            }
        }
        private void ChangeRightSprite()
        {
            switch (myNum)
            {
                case 1:
                    myImage.sprite = mapTexture2;
                    myNum = myNum + 1;
                    break;
                case 2:
                    Debug.Log("5번. 더는 내려갈 곳이 없습니다.");
                    break;
            }
        }

        /// <summary>
        /// 활성화해도 좋은지
        /// </summary>
        /// <param name="guiStateNum"></param>
        public void ImActivate(int guiStateNum)
        {
            if (guiStateNum <= 3)
            {
                gamePlay = false;
            }
            else if (guiStateNum == 7 || guiStateNum == 8)
            {
                gamePlay = false;
            }
            else if (guiStateNum == 5 && guiStat == GUIState.on)
            {
                errorCheck = true;
            }
        }

        private void ObjectActive()
        {
            guiStat = GUIState.on;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            initObj.SendMessage("GUIOnCheck");
        }

        private void ObjectNotActive()
        {
            guiStat = GUIState.off;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            initObj.SendMessage("GUIOnCheck");
        }

        private void ItemUse()
        {
            if(guiStat == GUIState.on)
            {
                ObjectNotActive();
            }
            else
            {
                ObjectActive();
            }
        }
    }
}
