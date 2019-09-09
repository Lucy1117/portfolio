using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 게임 저장, 불러오기 때 사용할 스토리 순서.
    /// 모든 MyStoryDetial에 관여.
    /// SceneData에 적용
    /// </summary>
    public class StoryInform : MonoBehaviour
    {
        /// <summary>
        /// 현재 씬에서 쓰는 스토리 저장 변수.
        /// 씬이 로드되거나 체인지 될 때 각 씬에 맞는 변수에 저장한다.
        /// </summary>
        public int storyNum;

        /// <summary>
        /// 튜토리얼의 스토리 순서 저장 변수
        /// </summary>
        public int tuStoryNum;
        /// <summary>
        /// 챕터1, 본관 1층의 스토리 순서 저장 변수
        /// </summary>
        public int chOneFirstNum;
        /// <summary>
        /// 챕터1, 본관 2층의 스토리 순서 저장 변수
        /// </summary>
        public int chOneSecondNum;
        /// <summary>
        /// 챕터1, 본관 3층의 스토리 순서 저장 변수
        /// </summary>
        public int chOneThirdNum;

        public string itemDetailStr = string.Empty;
       
        private SceneName myScene;

        private GameObject[] StoryList;

        private GameObject[] EventList;

        /// <summary>
        /// 게임데이터를 이용한 로드 이후에는 챕터 캔버스가 뜨지 않도록
        /// </summary>
        private GameObject chapterCanvas;

        /// <summary>
        /// 이벤트에 대한 데이터를 담은 string.
        /// </summary>
        public string eventDetailStr = string.Empty;

        private bool loadingEnd;

        public bool m_loadingEnd
        {
            get { return loadingEnd; }
            set { loadingEnd = value; }
        }

        //Start보다 먼저.
        private void Awake()
        {
            if (GameObject.FindGameObjectWithTag("StoryMode"))
            {
                StoryList = GameObject.FindGameObjectsWithTag("StoryMode");
            }
            else
            {
                StoryList = null;
            }
            if (GameObject.FindGameObjectWithTag("EventMode"))
            {
                EventList = GameObject.FindGameObjectsWithTag("EventMode");
            }
            else
            {
                EventList = null;
            }
        }
        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// 다음 스토리 순서로 넘어갈 때 행해져야 할 것들.
        /// </summary>
        void GoNextStoryOrder()
        {
            StoryList = GameObject.FindGameObjectsWithTag("StoryMode");
            storyNum++;
            
            foreach (GameObject storyLi in StoryList)
            {
                storyLi.GetComponent<MyStoryDetail>().currentStoryNum = storyNum;
            }
            Debug.Log(storyNum + "번째 이야기 진행 중.");
        }

        /// <summary>
        /// 이벤트가 하나 실행될 때마다 행해져야 할 것들.
        /// 이벤트가 true되면 그때 그 이벤트 명을 저장.
        /// EventInform.cs에서 호출
        /// </summary>
        void GoEventOrder(int eventNum)
        {
            switch (myScene)
            {
                case SceneName.LoadScene:
                    break;
                case SceneName.Tutorial:
                    eventDetailStr += 1 + "-" + eventNum + ";";
                    break;
                case SceneName.ChapterOneFirstFloor:
                    eventDetailStr += 2 + "-" + eventNum + ";";
                    break;
                case SceneName.ChapterOneSecondFloor:
                    eventDetailStr += 3 + "-" + eventNum + ";";
                    break;
                case SceneName.ChapterOneThirdFloor:
                    eventDetailStr += 4 + "-" + eventNum + ";";
                    break;
                default:
                    break;
            }
            Debug.Log(eventDetailStr);
        }

        /// <summary>
        /// 게임이 로드 되었을 때, 씬이 이동되었을 때, 각 event에 true인지 false인지 전달하는 함수.
        /// </summary>
        private void EventListLoad(int sceneNum)
        {
            if (GameObject.FindGameObjectWithTag("EventMode"))
            {
                EventList = GameObject.FindGameObjectsWithTag("EventMode");
            }
            else
            {
                EventList = null;
            }

            string[] splitContent = eventDetailStr.Split(';');

            for(int x = 0; x < splitContent.Length - 1; x++)
            {
                string[] splitValues = splitContent[x].Split('-');
                int index = Int32.Parse(splitValues[0]);

                if (sceneNum == index)
                {
                    foreach (GameObject eventLi in EventList)
                    {
                        eventLi.SendMessage("EventSkip", Int32.Parse(splitValues[1]));
                    }
                }
            }
        }

        /// <summary>
        /// SceneDataLoad.cs에서 호출
        /// 씬 이동 할 때, 이전까지의 씬 정보를 저장(예를들어, 저장버튼 없이 1층에서 2층으로 이동하는 경우)
        /// </summary>
        /// <param name="currentScene"></param>
        public void ChangeSceneInform(SceneName currentScene)
        {
               loadingEnd = false;

               myScene = currentScene;

            switch (myScene)
            {
                case SceneName.Prologue:
                    itemDetailStr = "0-Phone;";
                    tuStoryNum = 0;
                    chOneFirstNum = 0;
                    chOneSecondNum = 0;
                    chOneThirdNum = 0;
                    break;
                case SceneName.InputMain:
                    break;
                case SceneName.LoadScene:
                    break;
                case SceneName.Tutorial:
                    itemDetailStr = GameObject.Find("Inventory").GetComponent<Inventory>().itemDetailContent;
                    tuStoryNum = storyNum;
                    break;
                case SceneName.ChapterOneFirstFloor:
                    itemDetailStr = GameObject.Find("Inventory").GetComponent<Inventory>().itemDetailContent;
                    chOneFirstNum = storyNum;
                    break;
                case SceneName.ChapterOneSecondFloor:
                    itemDetailStr = GameObject.Find("Inventory").GetComponent<Inventory>().itemDetailContent;
                    chOneSecondNum = storyNum;
                    break;
                case SceneName.ChapterOneThirdFloor:
                    itemDetailStr = GameObject.Find("Inventory").GetComponent<Inventory>().itemDetailContent;
                    chOneThirdNum = storyNum;
                    break;
                default:
                    break;
            }
            Debug.Log("체인지 된 정보 저장. " + tuStoryNum + " " + chOneFirstNum + " " + chOneSecondNum + " " + chOneThirdNum);
        }

        /// <summary>
        /// Load되었을 때, 행해져야 할 것들.
        /// InitInterface.cs에서 호출.
        /// 일단 초기화 필요.
        /// </summary>
        void LoadStoryOrder(SceneName currentScene)
        {
            myScene = currentScene;

            switch (myScene)
            {
                case SceneName.InputMain:
                    storyNum = 1;
                    break;
                case SceneName.LoadScene:
                    break;
                case SceneName.Tutorial:
                    if(tuStoryNum == 0)
                    {
                        tuStoryNum = 1;
                    }
                    else
                    {
                        ChapterCanvasOff();
                        //EventListLoad(1);
                    }
                    storyNum = tuStoryNum;
                    break;
                case SceneName.ChapterOneFirstFloor:
                    if (chOneFirstNum == 0)
                    {
                        chOneFirstNum = 1;
                    }
                    else
                    {
                        ChapterCanvasOff();
                        EventListLoad(2);
                    }
                    Debug.Log("여기인가");
                    storyNum = chOneFirstNum;
                    break;
                case SceneName.ChapterOneSecondFloor:
                    if (chOneSecondNum == 0)
                    {
                        chOneSecondNum = 1;
                    }
                    else
                    {
                        ChapterCanvasOff();
                        EventListLoad(3);
                    }
                    storyNum = chOneSecondNum;
                    break;
                case SceneName.ChapterOneThirdFloor:
                    if (chOneThirdNum == 0)
                    {
                        chOneThirdNum = 1;
                    }
                    else
                    {
                        ChapterCanvasOff();
                        EventListLoad(4);
                    }
                    storyNum = chOneThirdNum;
                    break;
                default:
                    break;
            }

            if (GameObject.FindGameObjectWithTag("StoryMode"))
            {
                StoryList = GameObject.FindGameObjectsWithTag("StoryMode");
            }
            else
            {
                StoryList = null;
            }
            //로드된 storyNum값을 보내줌.
            foreach (GameObject storyLi in StoryList)
            {
                storyLi.GetComponent<MyStoryDetail>().currentStoryNum = storyNum;
            }

            GameObject invenObj = GameObject.Find("Inventory");
            GameObject.Find("Initiate").GetComponent<InitInterface>().itemDetailStr = itemDetailStr;

            if (itemDetailStr == string.Empty)
            {
                Debug.Log("아이템 데이터가 비어있습니다.");
                invenObj.SendMessage("PhoneAdd");
            }
            else
            {
                Debug.Log(GameObject.Find("Initiate").GetComponent<InitInterface>().itemDetailStr);
                invenObj.SendMessage("LoadInventory");
            }

            Debug.Log(myScene + " 로드된 데이터입니다. " + storyNum + "번째 이야기 진행 중.");
            Debug.Log(tuStoryNum + " " + chOneFirstNum + " " + chOneSecondNum + " " + chOneThirdNum);
            loadingEnd = true;
        }

        private void ChapterCanvasOff()
        {

            if (GameObject.Find("Chapter"))
            {
                chapterCanvas = GameObject.Find("Chapter");
                chapterCanvas.GetComponent<ChapterIntro>().timerCheck = false;
            }
            else
            {
                chapterCanvas = null;
            }
        }
    }
}
