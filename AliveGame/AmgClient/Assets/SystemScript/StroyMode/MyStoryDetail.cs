using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 각각 스토리 세부사항을 넣을 cs
    /// StoryInformObj에 적용
    /// </summary>
    public class MyStoryDetail : MonoBehaviour
    {
        public int myStoryNum;
        private bool storyOn;

        private GameObject sceneDataObj;

        /// <summary>
        /// storyInformObj를 child로 가지는 gameObjedct
        /// </summary>
        private GameObject parentObj;

        private int m_currentStoryNum;

        /// <summary>
        /// gameController의 StoryInform에서 포괄적으로 관리하는 현재 스토리 순서.
        /// </summary>
        public int currentStoryNum {
            get { return m_currentStoryNum; }
            set { m_currentStoryNum = value; }
        }

        public bool sendOnce;
        
        //Start보다 먼저.
        private void Awake()
        {
            parentObj = transform.parent.gameObject;
            sendOnce = true;
            sceneDataObj = GameObject.Find("SceneData");
        }


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
            m_currentStoryNum = sceneDataObj.GetComponent<StoryInform>().storyNum;

            //Debug.Log("언제 시작되는지" + parentObj.name + m_currentStoryNum);
        }

        // Update is called once per frame
        void Update()
        {
            if (sendOnce)
            {
                if (m_currentStoryNum == myStoryNum)
                {
                    storyOn = true;
                    sendOnce = false;
                    parentObj.SendMessage("StoryOn");
                    Debug.Log(myStoryNum + "StoryOn 호출");
                }
                if(m_currentStoryNum > myStoryNum)
                {
                    storyOn = false;
                    sendOnce = false;
                    Debug.Log(currentStoryNum + " "+ parentObj.name);
                    parentObj.SendMessage("StorySkip");
                }
            }
            if (storyOn)
            {
                if (m_currentStoryNum > myStoryNum)
                {
                    storyOn = false;
                    sendOnce = false;
                    Debug.Log(currentStoryNum + " " + parentObj.name);
                    parentObj.SendMessage("StorySkip");
                }
            }
        }

        public void StorySkipNum()
        {
            sceneDataObj.GetComponent<StoryInform>().storyNum = myStoryNum;
        }


        public void StoryEnd()
        {
            storyOn = false;
            sceneDataObj.SendMessage("GoNextStoryOrder");
        }
    }
}
