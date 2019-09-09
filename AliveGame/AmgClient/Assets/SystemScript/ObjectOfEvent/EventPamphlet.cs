using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// hapter1. FirstBuilding의 팜플렛에 들어갈 이벤트. 팜플렛을 주우면 발생. 
    /// brochure의 Trigger에 들어가는 cs
    /// </summary>
    public class EventPamphlet : MonoBehaviour
    {
        // 팜플렛을 주우면 번개가 내리치는 효과음이 들리는 것도 나쁘지 않을 듯.
        
        /// <summary>
        /// 자식으로 갖는 eventObject
        /// </summary>
        public GameObject eventObj;

        /// <summary>
        /// 효과음 소리 크기
        /// </summary>
        public float soundVolume;

        /// <summary>
        /// 팜플렛 주울 때 나는 효과음
        /// </summary>
        public AudioClip pamphletSound;


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
            if (other.gameObject.tag == "Player")
            {

            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (InputManager.XButton())
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                    {
                        EventPlayOn();
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
            }
        }

        /// <summary>
        /// 이벤트가 플레이 되면
        /// </summary>
        private void EventPlayOn()
        {
            GetComponent<AudioSource>().PlayOneShot(pamphletSound, soundVolume);
            eventObj.SendMessage("EventPlayOn");
        }

        /// <summary>
        /// 이벤트가 스킵되는 경우
        /// EventInform.cs에서 호출
        /// </summary>
        private void EventPlaySkip()
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
