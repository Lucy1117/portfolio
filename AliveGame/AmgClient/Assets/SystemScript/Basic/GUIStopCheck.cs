using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// GUI Interface가 현재 활성화 되어있는지의 여부를 나타낸다.
    /// </summary>
    public enum GUIState
    {
        on,
        off
    };
    public class GUIStopCheck : MonoBehaviour
    {
        public GameObject inGameMenuobj;

        public GameObject phoneImageObj;

        public GameObject inventoryObj;

        public GameObject messagePopObj;

        public GameObject useSlotObj;

        public GameObject mapImageObj;

        public GameObject newsImageObj;

        public int whichGUIOn;

        private GameObject[] GUIList;

        private GameObject player;
        

        // Use this for initialization
        void Start()
        {
            GUIList = GameObject.FindGameObjectsWithTag("GUI");
            player = GameObject.FindGameObjectWithTag("Player");
            whichGUIOn = 8;
            GUIOnCheck();
            if (this.GetComponent<InitInterface>().mySceneData != SceneName.Tutorial)
            {
                messagePopObj.SendMessage("MessageCallDown");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GUIOnCheck()
        {
            if (GameObject.Find("Chapter"))
            {
                if (GameObject.Find("Chapter").GetComponent<ChapterIntro>().guiStat == GUIState.on)
                {//챕터 GUI활성화되어있을 경우
                    whichGUIOn = 1;
                }
                else
                {
                    whichGUIOn = 2;
                    if (inGameMenuobj.GetComponent<InGameMenu>().guiStat == GUIState.on)
                    {//bInGameMenu가 활성화 되어있을 경우
                        whichGUIOn = 3;
                    }
                    else
                    {
                        whichGUIOn = 4;
                        if (phoneImageObj.GetComponent<PhoneMessage>().guiStat == GUIState.on || mapImageObj.GetComponent<MapPamphlet>().guiStat == GUIState.on
                            || newsImageObj.GetComponent<NewsPaper>().guiStat == GUIState.on)
                        {//inventory내의 phoneImage가 활성화 되어있을 경우
                            whichGUIOn = 5;
                        }
                        else
                        {
                            whichGUIOn = 6;
                            if (inventoryObj.GetComponent<Inventory>().guiStat == GUIState.on)
                            {//Inventory가 활성화 되어있을 경우
                                whichGUIOn = 7;
                            }
                            else
                            {//아무것도 활성화 안되어 있을 경우
                                whichGUIOn = 8;
                            }
                        }
                    }
                }
            }
            else
            {
                if (inGameMenuobj.GetComponent<InGameMenu>().guiStat == GUIState.on)
                {//bInGameMenu가 활성화 되어있을 경우
                    whichGUIOn = 3;
                }
                else
                {
                    whichGUIOn = 4;
                    if (phoneImageObj.GetComponent<PhoneMessage>().guiStat == GUIState.on || mapImageObj.GetComponent<MapPamphlet>().guiStat == GUIState.on
                        || newsImageObj.GetComponent<NewsPaper>().guiStat == GUIState.on)
                    {//inventory내의 phoneImage가 활성화 되어있을 경우
                        whichGUIOn = 5;
                    }
                    else
                    {
                        whichGUIOn = 6;
                        if (inventoryObj.GetComponent<Inventory>().guiStat == GUIState.on)
                        {//Inventory가 활성화 되어있을 경우
                            whichGUIOn = 7;
                        }
                        else
                        {//아무것도 활성화 안되어 있을 경우
                            whichGUIOn = 8;
                        }
                    }
                }
            }


            foreach (GameObject guiLI in GUIList)
            {
                guiLI.BroadcastMessage("ImActivate", whichGUIOn);
            }
            player.SendMessage("ImActivate", whichGUIOn);
        }
    }
}
