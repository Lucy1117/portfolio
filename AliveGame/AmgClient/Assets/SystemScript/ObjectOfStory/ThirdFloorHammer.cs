using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 ThirdFloor에서 망치에 들어갈 코드.
    /// </summary>
    public class ThirdFloorHammer : MonoBehaviour
    {

        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        /// <summary>
        /// 망치 주우면 잠길 문
        /// </summary>
        public GameObject lockDoorObj;

        /// <summary>
        /// 문 앞에 생길 석고상
        /// </summary>
        public GameObject plasterFigureObj;


        // Use this for initialization
        void Start()
        {

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
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                    {
                        StoryOff();
                    }
                }
            }
        }

        /// <summary>
        /// 이 object가 가진 스토리가 시작할 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOn()
        {
            Debug.Log("StoryOn호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOff()
        {
            plasterFigureObj.SetActive(true);  //석고상 생김

            if (lockDoorObj.GetComponent<DoorAnim>().opening)
            {
                lockDoorObj.SendMessage("RemoteDoorControl");
            }

            lockDoorObj.GetComponent<DoorAnim>().doorLockCheck = true;
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            childstoryObj.BroadcastMessage("StorySkipNum");
            childstoryObj.BroadcastMessage("StoryEnd");

            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }

            Debug.Log("StoryOff호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            if (lockDoorObj.GetComponent<DoorAnim>().opening)
            {
                lockDoorObj.SendMessage("RemoteDoorControl");
            }
            lockDoorObj.GetComponent<DoorAnim>().doorLockCheck = true;

            plasterFigureObj.SetActive(true);  //석고상 생김

            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}