using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 플레이어가 SceneMove Object에 충돌하면 실행되는 코드. 다음 씬으로 넘어가는 함수를 호출한다.
    /// </summary>
    public class LoadSceneChange : MonoBehaviour
    {
        #region SceneLoad에 필요한 변수
        private GameObject sceneDataObj;

        public SceneName changeSceneName;

        public int lastStoryNum;

        private int currentStoryNum;

        /// <summary>
        /// x버튼을 눌렀을 때 씬이 넘어가게 하려면 false 통과만 해도 넘어가려면 true
        /// </summary>
        public bool buttonNeed;

        //public SceneName scName;
        #endregion
        // Use this for initialization
        void Start()
        {
            if (GameObject.Find("SceneData"))
            {
                sceneDataObj = GameObject.Find("SceneData");
            }
            else
            {
                sceneDataObj = GameObject.Find("SceneData(Clone)");
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (InputManager.XButton())
                {
                    buttonNeed = true;
                }
                if (buttonNeed)
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8 && IsLastStory())
                    {
                        ChangeScene(changeSceneName);
                    }
                }
            }
        }

        /// <summary>
        /// 현재의 StoryNum이 마지막인지 판단하는 함수
        /// </summary>
        /// <returns></returns>
        public bool IsLastStory()
        {
            currentStoryNum = sceneDataObj.GetComponent<StoryInform>().storyNum;

            if(currentStoryNum == lastStoryNum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Scene의 이동을 도와주는 함수.
        /// sceneData로 SendMessage를 보내는 역할을 한다.
        /// </summary>
        /// <param name="sName"></param>
        public void ChangeScene(SceneName sName)
        {
            sceneDataObj.SendMessage("LoadSceneData", sName);
        }
    }
}