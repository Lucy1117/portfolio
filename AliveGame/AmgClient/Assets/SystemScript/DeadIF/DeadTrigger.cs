using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Trigger에 들어가면 3초 뒤 사망
    /// </summary>
    public class DeadTrigger : MonoBehaviour
    {
        /// <summary>
        /// 초기화obj
        /// </summary>
        private GameObject initObj;


        /// <summary>
        /// 죽기까지의 시간초
        /// </summary>
        public float deadTimer;

        private float timer;

        /// <summary>
        /// 트리거 안에 들어오면 true
        /// </summary>
        private bool deadTimerStart;

        /// <summary>
        /// 효과음 소리 크기
        /// </summary>
        public float soundVolume;

        /// <summary>
        /// 죽을 때 뒤통수 맞아서 나는 효과음
        /// </summary>
        public AudioClip headSound;

        // Use this for initialization
        void Start()
        {
            initObj = GameObject.FindGameObjectWithTag("Initiate");
            timer = 0.0f;
            deadTimerStart = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (deadTimerStart)
            {
                timer = timer + Time.deltaTime;
                if(timer > deadTimer)
                {
                    GetComponent<AudioSource>().PlayOneShot(headSound, soundVolume);

                    GameObject.FindWithTag("Player").SendMessage("PlayerDead");
                    initObj.SendMessage("CreateDeadGUI");
                    deadTimerStart = false;
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                deadTimerStart = true;
            }
        }
    }
}
