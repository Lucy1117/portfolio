using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 ThirdFloor에서 시계에 들어갈 코드.
    /// </summary>
    public class ThirdFloorClock : MonoBehaviour
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
        /// 시계 떨어질 때 나는 소리
        /// </summary>
        public AudioClip dropClockSound;

        /// <summary>
        /// 배경음 소리 크기
        /// </summary>
        public float soundVolume;

        /// <summary>
        /// 초침의 트리거 활성화
        /// </summary>
        public GameObject needleTriggerObj;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && onceCheck)
            {
                GetComponent<AudioSource>().PlayOneShot(dropClockSound, soundVolume);
                StoryOff();
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
            onceCheck = false;

            needleTriggerObj.SetActive(true);
            this.transform.parent.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameObject.Find("ClockNeedle").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
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
            needleTriggerObj.SetActive(true);
            this.transform.parent.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            if (GameObject.Find("ClockNeedle").GetComponent<Rigidbody>())
            {
                GameObject.Find("ClockNeedle").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            onceCheck = false;
        }

    }
}