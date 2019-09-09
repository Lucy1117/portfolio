using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 FirstFloor에서 액자 깨지는 곳에서 들어갈 코드.
    /// </summary>
    public class FirstFloorPictureBroken : MonoBehaviour
    {
        /// <summary>
        /// 깨진 액자 Object
        /// </summary>
        public GameObject brokenPicture;

        /// <summary>
        /// 액자 깨질 때 들리는 효과음
        /// </summary>
        public AudioClip pictureBrokenSound;

        /// <summary>
        /// 배경음 소리 크기
        /// </summary>
        public float soundVolume;

        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        /// <summary>
        /// 한번만 실행되도록
        /// </summary>
        private bool onceCheck;

        /// <summary>
        /// 액자가 떨어지는 동안의 시간
        /// </summary>
        public float fallingTime;
        private float timer;

        private bool thirdCheck;

        // Use this for initialization
        void Start()
        {
            onceCheck = true;
        }

        // Update is called once per frame
        void Update()
        {
           
            //그 다음 사운드 플레이. - 액자 깨지는 소리
            if(GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum == 8 && onceCheck)
            {
                onceCheck = false;
                GetComponent<AudioSource>().PlayOneShot(pictureBrokenSound, soundVolume);
                this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                thirdCheck = true;
            }

            if (thirdCheck)
            {
                timer = timer + Time.deltaTime;
                if (timer > fallingTime)
                {
                    StoryOff();
                    thirdCheck = false;
                }
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
            brokenPicture.SetActive(true);
            GameObject.Find("WallPrevent").SetActive(false);
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            childstoryObj.BroadcastMessage("StorySkipNum");
            childstoryObj.BroadcastMessage("StoryEnd");
            Debug.Log("StoryOff호출");
            this.gameObject.SetActive(false);
            
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            brokenPicture.SetActive(true);
            GameObject.Find("WallPrevent").SetActive(false);
            GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 5;
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 8);
            this.gameObject.SetActive(false);
        }
    }
}
