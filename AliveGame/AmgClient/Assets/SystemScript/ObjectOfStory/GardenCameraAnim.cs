using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class GardenCameraAnim : MonoBehaviour
    {
        private Animation animationclip;

        //public float animSpeed;

        public GameObject chapterobj;

        private bool animOnce = true;
        /// <summary>
        /// 사용 중인 애니메이션의 이름을 Inspector에서 적어야 함
        /// </summary>
        public string animName;

        public bool animStopneedObj;

        private bool animStop = false;

        private bool timeOnce = false;

        public float soundDelay;
        
        private void Awake()
        {
            animationclip = this.gameObject.GetComponent<Animation>();
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (animOnce)
            {
                if (!chapterobj.GetComponent<ChapterIntro>().chapterCanvasState)
                {
                    AnimPlay();
                    if (GetComponent<AudioSource>())
                    {
                        GetComponent<AudioSource>().PlayDelayed(soundDelay);
                        
                        //GetComponent<AudioSource>().Play();
                    }
                    animOnce = false;
                }
            }

            if (animStopneedObj)
            {
                if (timeOnce)
                {
                    AnimStopFun();
                }
            }
        }

        private void AnimPlay()
        {
            timeOnce = true;
            animationclip.Play(animName);
        }

        private void AnimStopFun()
        {
            if (animationclip.IsPlaying(animName))
            {
                animStop = false;
            }
            else
            {
                animStop = true;
            }

            if (animStop)
            {
                this.gameObject.SendMessage("ChangeScene", SceneName.Tutorial);
                timeOnce = false;

            }
        }
    }
}