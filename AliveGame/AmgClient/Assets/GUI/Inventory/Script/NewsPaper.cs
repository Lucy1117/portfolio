using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 신문을 누르면 이미지가 뜰 수 있게
    /// MapPamphlet.cs와 비슷하게 코딩.
    /// </summary>
    public class NewsPaper : MonoBehaviour
    {

        public GUIState guiStat = GUIState.off;

        private bool gamePlay;

        private bool errorCheck;

        private GameObject initObj;

        // Use this for initialization
        void Start()
        {
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
                    ObjectNotActive();
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
            if (guiStat == GUIState.on)
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
