using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. SecondBuilding의 SecondFloor에서 무서운 소리가 나오는 이벤트1.
    /// </summary>
    public class EventSecondFloorOne : MonoBehaviour
    {
        /// <summary>
        /// 자식으로 갖는 eventObject
        /// </summary>
        public GameObject eventObj;

        /// <summary>
        /// 호러 사운드
        /// </summary>
        public AudioClip horrorSound;

        public float soundVolume;

        /// <summary>
        /// 한 번 만 실행되도록
        /// </summary>
        public bool onceCheck;

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
                EventPlayOn();
            }
        }

        /// <summary>
        /// 이벤트가 플레이 되면
        /// </summary>
        private void EventPlayOn()
        {
            GetComponent<AudioSource>().PlayOneShot(horrorSound, soundVolume);
            eventObj.SendMessage("EventPlayOn");
            onceCheck = false;
        }

        /// <summary>
        /// 이벤트가 스킵되는 경우
        /// EventInform.cs에서 호출
        /// </summary>
        private void EventPlaySkip()
        {
            onceCheck = false;
            this.gameObject.SetActive(false);
        }
    }
}
