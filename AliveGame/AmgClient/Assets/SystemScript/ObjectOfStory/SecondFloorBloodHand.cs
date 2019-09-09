using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    ///  Chapter1. FirstBuilding의 SecondFloor에서 유리벽의 피손바닥에 들어갈 코드.
    /// </summary>
    public class SecondFloorBloodHand : MonoBehaviour
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
        /// 유리벽에 찍힐 손바닥 object
        /// </summary>
        public GameObject bloodHandObj;

        /// <summary>
        /// 액자 깨질 때 들리는 효과음
        /// </summary>
        public AudioClip bloodHandSound;

        /// <summary>
        /// 배경음 소리 크기
        /// </summary>
        public float soundVolume;


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
            if (onceCheck && other.gameObject.tag == "Player")
            {
                //AudioSource audiosource = GetComponent<AudioSource>();
                //audiosource.PlayOneShot(tvartBgm, soundVolume);
                StoryOff();
                onceCheck = false;
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
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            GetComponent<AudioSource>().PlayOneShot(bloodHandSound, soundVolume);
            bloodHandObj.SetActive(true);
            GameObject.Find("HorrorSoundEvent1").GetComponent<EventSecondFloorOne>().onceCheck = true;
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
            bloodHandObj.SetActive(true);
            if (GameObject.Find("HorrorSoundEvent1"))
            {
                GameObject.Find("HorrorSoundEvent1").GetComponent<EventSecondFloorOne>().onceCheck = true;
            }
        }
    }
}
