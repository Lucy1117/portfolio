using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class InitInterface : MonoBehaviour
    {
        private GameObject sDataObj;
        public SceneName mySceneData;
        public GameObject errorExceptionObj;
        private bool sDataIs;

        public string playTimeStr;
        public string chapterNameStr;
        public string currentSpotStr;

        public string itemDetailStr = string.Empty;

        private GameObject soundObj;
        /// <summary>
        /// bgm 번호. BGMOperation.cs 참고.
        /// </summary>
        public int bgmSoundNum;


        // Use this for initialization
        private void Awake()
        {
            Cursor.visible = false;
            if (GameObject.Find("SceneData"))
            {
                sDataObj = GameObject.Find("SceneData");
                if (GameObject.Find("SceneData(Clone)"))
                {
                    GameObject.Destroy(GameObject.Find("SceneData(Clone)"));
                }
            }
            else
            {
                if (GameObject.Find("SceneData(Clone)"))
                {
                    sDataObj = GameObject.Find("SceneData(Clone)");
                }
                else
                {
                    sDataObj = GameObject.Instantiate(errorExceptionObj);
                }
            }
        }

        void Start()
        {
            soundObj = GameObject.Find("BackGroundSound");
            sDataObj.GetComponent<SceneDataLoad>().myScene = mySceneData;
            soundObj.SendMessage("SoundBGMPlay", bgmSoundNum);

            if ((mySceneData == SceneName.Tutorial) && (sDataObj.GetComponent<StoryInform>().tuStoryNum != 0))
            {
                ReadLoad();
            }
            if ((mySceneData == SceneName.ChapterOneFirstFloor) && (sDataObj.GetComponent<StoryInform>().chOneFirstNum != 0))
            {
                ReadLoad();
            }
            if ((mySceneData == SceneName.ChapterOneSecondFloor) && (sDataObj.GetComponent<StoryInform>().chOneSecondNum != 0))
            {
                ReadLoad();
            }
            if ((mySceneData == SceneName.ChapterOneThirdFloor) && (sDataObj.GetComponent<StoryInform>().chOneThirdNum != 0))
            {
                ReadLoad();
            }

            if (GameObject.FindGameObjectWithTag("StoryMode"))
            {
                sDataObj.SendMessage("LoadStoryOrder", mySceneData);
            }
        }

        /// <summary>
        /// SaveButton의 SaveLoadMenu에서 호출해서 SaveLoadInformation.cs내의 함수를 호출.
        /// </summary>
        /// <param name="myNum"> 몇번째 버튼에서 저장을 눌렀는지</param>
        public void CallSave(string myNum)
        {
            sDataObj.SendMessage("ChangeSceneInform", mySceneData);
            sDataObj.SendMessage("SaveGameInformation", myNum);
        }

        /// <summary>
        /// LoadButton의 SaveLoadMenu에서 호출해서 SaveLoadInformation.cs내의 함수를 호출.
        /// </summary>
        /// <param name="myNum"> 몇번째 버튼의 불러오기를 눌렀는지</param>
        public void CallLoad(string myNum)
        {
            sDataObj.SendMessage("LoadFun", myNum);
        }

        /// <summary>
        /// 자기자신에서 호출
        /// </summary>
        public void ReadLoad()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(
                sDataObj.GetComponent<SaveLoadInformation>().playerPositionX,
                sDataObj.GetComponent<SaveLoadInformation>().playerPositionY,
                sDataObj.GetComponent<SaveLoadInformation>().playerPositionZ
            );
            player.transform.rotation = Quaternion.Euler(0, sDataObj.GetComponent<SaveLoadInformation>().playerAngle, 0);

            //Debug.Log("캐릭터 위치 로드");
        }
        // Update is called once per frame

        /// <summary>
        /// saveloadmenu.cs에서 실행. 현재 저장된 파일의 정보를 가시적으로 보여줌.
        /// </summary>
        public void VisibleInformRead()
        {
            for(int i = 1; i < 6; i++)
            {
                sDataObj.SendMessage("FileInformationRead", i.ToString());
            }
        }
        
        /// <summary>
        /// SaveLoadInformation.cs에서 실행. 파일 정보를 보냄.
        /// </summary>
        /// <param name="fileNum"></param>
        public void VisibleInformWrite(string fileNum)
        {
            playTimeStr = sDataObj.GetComponent<SaveLoadInformation>().playTimeString;
            chapterNameStr = sDataObj.GetComponent<SaveLoadInformation>().chapterName;
            currentSpotStr = sDataObj.GetComponent<SaveLoadInformation>().currentScene;
            if (GameObject.Find("SaveSlot1"))
            {
                GameObject.Find("SaveSlot" + fileNum).SendMessage("InformationVisible");
            }
            if (GameObject.Find("LoadSlot1"))
            {
                GameObject.Find("LoadSlot" + fileNum).SendMessage("InformationVisible");
            }
        }
    }
}
