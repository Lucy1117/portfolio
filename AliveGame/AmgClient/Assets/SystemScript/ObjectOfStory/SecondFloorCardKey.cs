using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 SecondFloor에서 카드키에 들어갈 코드.
    /// </summary>
    public class SecondFloorCardKey : MonoBehaviour
    {
        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        /// <summary>
        /// 피 칠갑이 될 벽 모음 Object
        /// </summary>
        public GameObject changeWallObj;

        /// <summary>
        /// 관계자외 출입금지 문(오른쪽 끝에 있는)
        /// </summary>
        public GameObject openDoorObj;

       

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
            if (openDoorObj.GetComponent<DoorAnim>().opening)
            {
                openDoorObj.SendMessage("RemoteDoorControl");
            }
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            childstoryObj.BroadcastMessage("StorySkipNum");
            childstoryObj.BroadcastMessage("StoryEnd");

            changeWallObj.SetActive(true);

            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }

            GameObject.Find("UpPrevent").SetActive(false);
            GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 10;
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 10);
            Debug.Log("StoryOff호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            if (openDoorObj.GetComponent<DoorAnim>().opening)
            {
                openDoorObj.SendMessage("RemoteDoorControl");
            }

            changeWallObj.SetActive(true);

            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }

            GameObject.Find("UpPrevent").SetActive(false);
            GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 10;
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 10);
        }
    }
}
