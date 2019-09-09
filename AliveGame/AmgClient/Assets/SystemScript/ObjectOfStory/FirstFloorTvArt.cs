using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 FirstFloor에서 티비아트에 들어갈 코드.
    /// </summary>
    public class FirstFloorTvArt : MonoBehaviour
    {
        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        /// <summary>
        /// 한번만 실행되도록
        /// </summary>
        private bool onceCheck;

        private bool storyOnCheck;

        /// <summary>
        /// TVArt가 켜지는 obj모음.
        /// </summary>
        private GameObject[] tvArtObjList;

        // Use this for initialization
        void Start()
        {
            onceCheck = true;
            tvArtObjList = GameObject.FindGameObjectsWithTag("TVVideoPlayer");
            storyOnCheck = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (storyOnCheck && other.gameObject.tag == "Player" && onceCheck)
            {
                
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
            storyOnCheck = true;
            Debug.Log("StoryOn호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOff()
        {
            foreach (GameObject tvartLi in tvArtObjList)
            {
                //렉 너무 잘 걸려서 잠시 꺼둠. 
                tvartLi.transform.GetChild(2).gameObject.SetActive(true);
            }
            GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 6;
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 6);
           
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            childstoryObj.BroadcastMessage("StorySkipNum");
            childstoryObj.BroadcastMessage("StoryEnd");
            storyOnCheck = false;
            Debug.Log("StoryOff호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 6;
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 6);
            storyOnCheck = false;
        }
    }
}
