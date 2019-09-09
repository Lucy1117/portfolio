using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class DeadGUI : MonoBehaviour
    {
        public GameObject DeadGUICanvas;
        private GameObject player;
        
        /// <summary>
        /// 캐릭터의 dead Animation이 끝나는 시간.
        /// </summary>
        public float timer;
        private bool timerCheck = false;
        
        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (player.GetComponent<FirstPersonController>().deadCheck)
            {
                if (timerCheck)
                {
                    timer = timer - Time.deltaTime;
                    if (timer < 0.0f)
                    {
                        timerCheck = false;
                        Instantiate(DeadGUICanvas);
                    }
                }
            }
        }

        /// <summary>
        /// DeadThunderbolt에서 호출.
        /// </summary>
        public void CreateDeadGUI()
        {
            timerCheck = true;
        }
    }
}
