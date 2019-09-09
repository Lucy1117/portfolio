using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 ThirdFloor에서 신문기사에 들어갈 코드.
    /// </summary>
    public class EventNewsPaper : MonoBehaviour
    {
        /// <summary>
        /// 자식으로 갖는 eventObject
        /// </summary>
        public GameObject eventObj;

        /// <summary>
        /// 효과음 소리 크기
        /// </summary>
        public float soundVolume;

        /// <summary>
        /// 신문을 주울 때 나는 효과음
        /// </summary>
        public AudioClip paperSound;

        /// <summary>
        /// 벽에 손바닥 찍힐 때 들릴 소리
        /// </summary>
        public AudioClip bloodSound;

        /// <summary>
        /// 한번만 실행되는 특정 이벤트에서.
        /// </summary>
        private bool onceCheck;

        public GameObject bloodWall;

        /// <summary>
        /// object 앞에 활성화되는 tip
        /// </summary>
        public GameObject activeTip;

        // Use this for initialization
        void Start()
        {
            onceCheck = true;
        }

        // Update is called once per frame
        void Update()
        {

        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {

            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                activeTip.GetComponent<CanvasGroup>().alpha = 0.4f;

                if (InputManager.XButton())
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                    {
                        GetComponent<AudioSource>().PlayOneShot(paperSound, soundVolume);

                        GameObject.Find("NewsPaperActivve").SendMessage("ItemUse");

                        if (onceCheck)
                        {
                            GetComponent<AudioSource>().PlayOneShot(bloodSound, soundVolume);
                            EventPlayOn();
                        }
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                activeTip.GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        /// <summary>
        /// 이벤트가 플레이 되면
        /// </summary>
        private void EventPlayOn()
        {
            onceCheck = false;
            eventObj.SendMessage("EventPlayOn");
            bloodWall.SetActive(true);
        }

        /// <summary>
        /// 이벤트가 스킵되는 경우
        /// EventInform.cs에서 호출
        /// </summary>
        private void EventPlaySkip()
        {
            onceCheck = false;
        }
    }
}