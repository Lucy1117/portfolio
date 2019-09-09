using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 ThirdFloor에서 창고문을 열쇠로 여는 부분에 들어갈 코드.
    /// </summary>
    public class ThirdFloorCctvRoom : MonoBehaviour
    {
        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        /// <summary>
        /// 한번만 실행되도록
        /// </summary>
        private bool onceCheck;

        /// <summary>
        /// 열쇠를 사용하면 true, 아니면 false
        /// </summary>
        private bool usekeyCheck;

        /// <summary>
        /// 열릴 문 대상
        /// </summary>
        public GameObject openDoorObj;

        /// <summary>
        /// 문 잠금 풀릴 때 나는 소리
        /// </summary>
        public AudioClip unlockSound;

        /// <summary>
        /// 배경음 소리 크기
        /// </summary>
        public float soundVolume;

        // Use this for initialization
        void Start()
        {
            usekeyCheck = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 아이템을 사용하면 Item.cs에서 호출
        /// </summary>
        private void ItemUse()
        {
            usekeyCheck = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                usekeyCheck = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (usekeyCheck && onceCheck)
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                    {
                        GetComponent<AudioSource>().PlayOneShot(unlockSound, soundVolume); //잠금 푸는 소리
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
            onceCheck = true;
            Debug.Log("StoryOn호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOff()
        {
            openDoorObj.GetComponent<DoorAnim>().doorLockCheck = false;

            onceCheck = false;
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            childstoryObj.BroadcastMessage("StorySkipNum");
            childstoryObj.BroadcastMessage("StoryEnd");
            Debug.Log("StoryOff호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            onceCheck = false;

            openDoorObj.GetComponent<DoorAnim>().doorLockCheck = false;
        }

    }
}
