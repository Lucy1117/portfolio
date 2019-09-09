using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// SceneData에 적용
    /// Save에 필요한 요소들을 모아주는 곳.
    /// 저장&불러오기 모두 담당.
    /// </summary>
    public class SaveLoadInformation : MonoBehaviour
    {
        public static SaveLoadInformation saveloadDetail;

        public float playerPositionX;
        public float playerPositionY;
        public float playerPositionZ;
        public float playerAngle;

        /// <summary>
        /// 현재 자신의 씬
        /// </summary>
        private SceneName myScene;

        /// <summary>
        /// 플레이시간
        /// </summary>
        private float playTime;
        /// <summary>
        /// 잠시 플레이시간을 넣는 곳.
        /// </summary>
        private float playTimeBuf;

        private float playSeconds;
        private float playMinutes;
        private float playHours;

        public string playTimeString;

        private int tutorialNum;
        private int chOneFirstNum;
        private int chOneSecondNum;
        private int chOneThirdNum;

        public string itemDetail = string.Empty;

        public string eventDetailStr = string.Empty;

        /// <summary>
        /// 현재 챕터 이름
        /// </summary>
        public string chapterName;
        /// <summary>
        /// 플레이하고 있는 현재 위치.(건물, 층)
        /// </summary>
        public string currentScene;

        private string saveFileNum;
        private string loadFileNum;

        private void Awake()
        {
            if(saveloadDetail == null)
            {
                saveloadDetail = this;
            }
            else if(saveloadDetail != this)
            {
                Debug.Log("saveLoadInformation.cs에서 saveloadDetail이 없다고 에러남.");
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            playTime += Time.deltaTime;
        }

        /// <summary>
        /// 게임에 저장할 정보(눈에 보이는)
        /// InitInterface.cs에서 SendMessage함.
        /// </summary>
        public void SaveGameInformation(string saveNum)
        {
            saveFileNum = saveNum;
            myScene = this.gameObject.GetComponent<SceneDataLoad>().myScene;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject itemData = GameObject.Find("Inventory");

            tutorialNum = this.gameObject.GetComponent<StoryInform>().tuStoryNum;
            chOneFirstNum = this.gameObject.GetComponent<StoryInform>().chOneFirstNum;
            chOneSecondNum = this.gameObject.GetComponent<StoryInform>().chOneSecondNum;
            chOneThirdNum = this.gameObject.GetComponent<StoryInform>().chOneThirdNum;
            itemDetail = itemData.GetComponent<Inventory>().itemDetailContent;
            eventDetailStr = this.gameObject.GetComponent<StoryInform>().eventDetailStr;

            playerPositionX = player.transform.position.x;
            playerPositionY = player.transform.position.y;
            playerPositionZ = player.transform.position.z;
            playerAngle = player.transform.rotation.eulerAngles.y;
            
            playSeconds = Mathf.Floor(playTime % 60.0f);
            playMinutes = Mathf.Floor(playTime / 60.0f);
            playHours = Mathf.Floor(playTime / 3600.0f);

            playTimeString = playHours.ToString("00") + ":" + playMinutes.ToString("00") + ":" + playSeconds.ToString("00");
            Debug.Log(saveFileNum + "번째 파일" + playTimeString);

            switch (myScene)
            {
                case SceneName.Default:
                    chapterName = "Default";
                    currentScene = "Default";
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case SceneName.LoadScene:
                    chapterName = "Default";
                    currentScene = "Default";
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case SceneName.StartMenu:
                    chapterName = "StartMenu";
                    currentScene = "StartMenu";
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case SceneName.Prologue:
                    chapterName = "Prologue";
                    currentScene = "프롤로그";
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case SceneName.Tutorial:
                    chapterName = "Tutorial";
                    currentScene = "정원";
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case SceneName.ChapterOneFirstFloor:
                    chapterName = "Chapter1";
                    currentScene = "본관 1층";
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case SceneName.ChapterOneSecondFloor:
                    chapterName = "Chapter1";
                    currentScene = "본관 2층";
                    break;
                case SceneName.ChapterOneThirdFloor:
                    chapterName = "Chapter1";
                    currentScene = "본관 3층";
                    break;
                case SceneName.InputMain:
                    chapterName = "실험챕터";
                    currentScene = "실험용씬";
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                default:
                    break;
            }

            SaveFun();
        }

        /// <summary>
        /// 이 cs내에서 실행
        /// </summary>
        public void SaveFun()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.streamingAssetsPath + "/playerInfo" + saveFileNum + ".dat");
            
            PlayerData pdata = new PlayerData();
            pdata.playerPosX = playerPositionX;
            pdata.playerPosY = playerPositionY;
            pdata.playerPosZ = playerPositionZ;
            pdata.playerAngle = playerAngle;

            GameData gdata = new GameData();
            gdata.playTimeFloat = playTime;
            gdata.playTimeStr = playTimeString;
            gdata.chapterNameStr = chapterName;
            gdata.currentSceneStr = currentScene;
            gdata.tutorialOrder = tutorialNum;
            gdata.chOneFirstOrder = chOneFirstNum;
            gdata.chOneSecondOrder = chOneSecondNum;
            gdata.chOndThirdOrder = chOneThirdNum;
            gdata.inventoryDetail = itemDetail;
            gdata.eventDetail = eventDetailStr;

            bf.Serialize(file, pdata);
            bf.Serialize(file, gdata);
            file.Close();

            GameObject initObj = GameObject.Find("Initiate");
            initObj.SendMessage("VisibleInformWrite", saveFileNum);
        }

        /// <summary>
        /// InitInterface에서 호출
        /// </summary>
        /// <param name="loadNum"></param>
        public void LoadFun(string loadNum)
        {
            loadFileNum = loadNum;
            FileInformationRead(loadFileNum);

            if (File.Exists(Application.streamingAssetsPath + "/playerInfo" + loadFileNum + ".dat"))
            {
                //씬을 로딩하면 됨
                LoadGameInformation();
            }
        }

        /// <summary>
        /// 파일에 저장된 정보를 읽어냄
        /// LoadFun()에서 호출
        /// b메뉴 버튼이 눌릴 때 InitInterface.cs에서 호출
        /// </summary>
        public void FileInformationRead(string loadNum)
        {
            if (File.Exists(Application.streamingAssetsPath + "/playerInfo" + loadNum + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.streamingAssetsPath + "/playerInfo" + loadNum + ".dat", FileMode.Open);

                PlayerData pdata = (PlayerData)bf.Deserialize(file);
                GameData gdata = (GameData)bf.Deserialize(file);
                file.Close();

                playerPositionX = pdata.playerPosX;
                playerPositionY = pdata.playerPosY;
                playerPositionZ = pdata.playerPosZ;
                playerAngle = pdata.playerAngle;

                playTimeBuf = gdata.playTimeFloat;
                playTimeString = gdata.playTimeStr;
                chapterName = gdata.chapterNameStr;
                currentScene = gdata.currentSceneStr;
                tutorialNum = gdata.tutorialOrder;
                chOneFirstNum = gdata.chOneFirstOrder;
                chOneSecondNum = gdata.chOneSecondOrder;
                chOneThirdNum = gdata.chOndThirdOrder;
                itemDetail = gdata.inventoryDetail;
                eventDetailStr = gdata.eventDetail;
                
                GameObject initObj = GameObject.Find("Initiate");
                initObj.SendMessage("VisibleInformWrite", loadNum);
                
            }
            else
            {
                //Debug.Log(loadNum + "해당 이름으로 저장된 파일이 없습니다.");
            }
        }

        /// <summary>
        /// 같은 cs내에서 호출
        /// </summary>
        public void LoadGameInformation()
        {
            this.gameObject.GetComponent<StoryInform>().tuStoryNum = tutorialNum;
            this.gameObject.GetComponent<StoryInform>().chOneFirstNum = chOneFirstNum;
            this.gameObject.GetComponent<StoryInform>().chOneSecondNum = chOneSecondNum;
            this.gameObject.GetComponent<StoryInform>().chOneThirdNum = chOneThirdNum;
            this.gameObject.GetComponent<StoryInform>().itemDetailStr = itemDetail;
            this.gameObject.GetComponent<StoryInform>().eventDetailStr = eventDetailStr;
            playTime = playTimeBuf;
            switch (currentScene)
            {
                case "Default":
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case "StartMenu":
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case "프롤로그":
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case "정원":
                    this.gameObject.SendMessage("LoadGameData", SceneName.Tutorial);
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case "본관 1층":
                    this.gameObject.SendMessage("LoadGameData", SceneName.ChapterOneFirstFloor);
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case "본관 2층":
                    this.gameObject.SendMessage("LoadGameData", SceneName.ChapterOneSecondFloor);
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case "본관 3층":
                    this.gameObject.SendMessage("LoadGameData", SceneName.ChapterOneThirdFloor);
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                case "실험용씬":
                    this.gameObject.SendMessage("LoadGameData", SceneName.InputMain);
                    //Debug.Log(chapterName + " " + currentScene);
                    break;
                default:
                    break;
            }
        }

        public void Delete()
        {
            if (File.Exists(Application.streamingAssetsPath + "/playerInfo.dat"))
            {
                File.Delete(Application.streamingAssetsPath + "/playerInfo.dat");
            }
        }


        /// <summary>
        /// Serializable
        /// CPU가 계산에 있어서 optimizing하는 것에 대해 우선 순위를 주는 것.
        /// 주로 데이터를 파일에 쓸 때, 필요한 값을 serializable해서 쓴다.
        /// 플레이어 데이터
        /// </summary>
        [Serializable]
        class PlayerData
        {
            public float playerPosX;
            public float playerPosY;
            public float playerPosZ;
            public float playerAngle;
        }

        /// <summary>
        /// 게임 내 데이터
        /// 플레이타임, 챕터이름, 현재 건물과 층, 스토리 순서
        /// </summary>
        [Serializable]
        class GameData
        {
            public float playTimeFloat;
            public string playTimeStr;
            public string chapterNameStr;
            public string currentSceneStr;
            public int tutorialOrder;
            public int chOneFirstOrder;
            public int chOneSecondOrder;
            public int chOndThirdOrder;
            public string inventoryDetail;
            public string eventDetail;
        }

    }
}
