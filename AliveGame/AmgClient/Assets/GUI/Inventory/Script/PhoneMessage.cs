using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class PhoneMessage : MonoBehaviour
    {
        public GUIState guiStat = GUIState.off;

        private bool buttonOn = false;

        public Sprite phoneTexture1;
        public Sprite phoneTexture2;
        public Sprite phoneTexture3;
        public Sprite phoneTexture4;
        public Sprite phoneTexture5;

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
                    if (InputManager.MainVertical() > 0)
                    {
                        ChangeUpSprite();
                        buttonOn = false;
                    }
                    if (InputManager.MainVertical() < 0)
                    {
                        ChangeDownSprite();
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

        private void ChangeUpSprite()
        {
            switch (myNum)
            {
                case 1:
                    Debug.Log("1번. 더는 올라갈 곳이 없습니다.");
                    break;
                case 2:
                    myImage.sprite = phoneTexture1;
                    myNum = myNum - 1;
                    break;
                case 3:
                    myImage.sprite = phoneTexture2;
                    myNum = myNum - 1;
                    break;
                case 4:
                    myImage.sprite = phoneTexture3;
                    myNum = myNum - 1;
                    break;
                case 5:
                    myImage.sprite = phoneTexture4;
                    myNum = myNum - 1;
                    break;
            }
        }
        private void ChangeDownSprite()
        {
            switch (myNum)
            {
                case 1:
                    myImage.sprite = phoneTexture2;
                    myNum = myNum + 1;
                    break;
                case 2:
                    myImage.sprite = phoneTexture3;
                    myNum = myNum + 1;
                    break;
                case 3:
                    myImage.sprite = phoneTexture4;
                    myNum = myNum + 1;
                    break;
                case 4:
                    myImage.sprite = phoneTexture5;
                    myNum = myNum + 1;
                    break;
                case 5:
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
            else if(guiStateNum == 7 || guiStateNum == 8)
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
            GameObject messagePopObj = GameObject.Find("MessagePop");
            messagePopObj.SendMessage("MessageCallDown");
            initObj.SendMessage("GUIOnCheck");
        }

        private void ObjectNotActive()
        {
            guiStat = GUIState.off;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            initObj.SendMessage("GUIOnCheck");
        }

    }
}