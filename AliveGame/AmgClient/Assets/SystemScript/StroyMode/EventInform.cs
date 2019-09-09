using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 이벤트마다 갖는 cs
    /// tutorial = 1, firstBuildingFirstFloor = 2, firstBuildingSecondFloor = 3, firstBuildingThirdFloor = 4
    /// 각 챕터마다 이름-이벤트 번호; 순으로 데이터 저장하기.
    /// 이벤트 번호는 각 챕터마다 1,2,3 순으로.
    /// </summary>
    public class EventInform : MonoBehaviour
    {
        /// <summary>
        /// 각 챕터의 event마다 번호를 붙임.
        /// </summary>
        public int eventNum;

        /// <summary>
        /// 이벤트가 실행되면 true. 그 전에는 false
        /// </summary>
        private bool eventOn;

        /// <summary>
        /// 필요 없으면 삭제해도 될 듯.
        /// </summary>
        public bool m_eventOn{
            get { return eventOn; }
            set { eventOn = value; }
        }

        /// <summary>
        /// StoryInform.cs를 갖는 object
        /// </summary>
        private GameObject sceneDataObj;

        /// <summary>
        /// EventInformObj를 가지는 parentObj
        /// </summary>
        private GameObject parentObj;

        private bool sendOnce;

        public bool deleteOk;

        //Start보다 먼저.
        private void Awake()
        {
            if (transform.parent.gameObject)
            {
                parentObj = transform.parent.gameObject;
            }
            else
            {
                parentObj = null;
            }
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
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// EventInformObj를 자식으로 갖는 각각의 Object에서 호출
        /// </summary>
        private void EventPlayOn()
        {
            eventOn = true;
            if (sendOnce)
            {
                sceneDataObj.SendMessage("GoEventOrder", eventNum);
                sendOnce = false;
            }
        }

        /// <summary>
        /// Load되거나 씬이 이동해서 특정 event가 이미 true인 경우.
        /// 그 event를 true로 만들고 skip
        /// </summary>
        private void EventSkip(int loadeventnum)
        {
            if(loadeventnum == eventNum)
            {
                //Debug.Log("EventSkip()" + parentObj.name);
                eventOn = true;

                parentObj.SendMessage("EventPlaySkip");
            }
        }
    }
}
