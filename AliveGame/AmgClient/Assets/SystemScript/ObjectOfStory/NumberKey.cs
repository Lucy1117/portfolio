using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class NumberKey : MonoBehaviour
    {
        public GameObject lockdoorObjA;

        public GameObject lockdoorObjB;

        private Animation _animation;

        private bool animationOnce;

        private bool unlockOnce;

        private GameObject myObject;

        /// <summary>
        /// object 앞에 활성화되는 tip
        /// </summary>
        public GameObject activeTip;

        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        private float defaultdoorVolume;
        
        public AudioClip soundEffect;
        public float soundVolume;
        private bool soundPlay;

        public float changetimer;
        private float timer;

        private int count = 0;

        // Use this for initialization
        void Start()
        {
            myObject = this.gameObject;
            _animation = GetComponent<Animation>();
            lockdoorObjA.GetComponent<DoorAnim>().doorLockCheck = true;
            lockdoorObjB.GetComponent<DoorAnim>().doorLockCheck = true;
            animationOnce = true;
            unlockOnce = true;
            timer = changetimer;
        }

        // Update is called once per frame
        void Update()
        {
            if (!animationOnce && unlockOnce)
            {
                if (!_animation.IsPlaying(_animation.name))
                {
                    Debug.Log("도어락 애니메이션");
                    lockdoorObjA.GetComponent<DoorAnim>().doorLockCheck = false;
                    lockdoorObjB.GetComponent<DoorAnim>().doorLockCheck = false;
                    unlockOnce = false;
                }
            }
            if (soundPlay)
            {
                timer += Time.deltaTime;
                if (changetimer < timer)
                {
                    Debug.Log("실행됩니당." + count);
                    this.gameObject.GetComponent<AudioSource>().PlayOneShot(soundEffect, soundVolume);
                    timer = 0.0f;
                    count++;
                }
                if(count == 4)
                {
                    soundPlay = false;
                }
            }
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                defaultdoorVolume = lockdoorObjA.GetComponent<DoorAnim>().soundVolume;
                activeTip.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (lockdoorObjA.GetComponent<DoorAnim>().doorLockCheck)
                {
                    lockdoorObjA.GetComponent<DoorAnim>().soundVolume = 0.0f;
                    lockdoorObjB.GetComponent<DoorAnim>().soundVolume = 0.0f;
                }
                else
                {
                    lockdoorObjA.GetComponent<DoorAnim>().soundVolume = defaultdoorVolume;
                    lockdoorObjB.GetComponent<DoorAnim>().soundVolume = defaultdoorVolume;
                }
                if (InputManager.XButton())
                {
                    if (animationOnce)
                    {
                        _animation.Play();
                        soundPlay = true;
                        animationOnce = false;
                        activeTip.GetComponent<CanvasGroup>().alpha = 0;
                        StoryOff();
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                activeTip.GetComponent<CanvasGroup>().alpha = 0;

                lockdoorObjA.GetComponent<DoorAnim>().soundVolume = defaultdoorVolume;
                lockdoorObjB.GetComponent<DoorAnim>().soundVolume = defaultdoorVolume;
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
            Debug.Log("StoryOff호출");
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            myObject.BroadcastMessage("StorySkipNum");
            myObject.BroadcastMessage("StoryEnd");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            animationOnce = false;
            lockdoorObjA.GetComponent<DoorAnim>().doorLockCheck = false;
            lockdoorObjB.GetComponent<DoorAnim>().doorLockCheck = false;
            unlockOnce = false;
            activeTip.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
