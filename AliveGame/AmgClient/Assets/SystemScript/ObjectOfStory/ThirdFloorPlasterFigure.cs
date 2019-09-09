using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 ThirdFloor에서 석고상을 부수는 부분에 들어갈 코드.
    /// </summary>
    public class ThirdFloorPlasterFigure : MonoBehaviour
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
        /// 아이템을 사용하면 true
        /// </summary>
        private bool useItemCheck;

        /// <summary>
        /// 잠금이 풀릴 대상
        /// </summary>
        public GameObject openDoorObj;

        public GameObject openOfficeDoorObjA;
        public GameObject openOfficeDoorObjB;

        /// <summary>
        /// 문 잠금 풀릴 때 나는 소리
        /// </summary>
        public AudioClip unlockSound;

        /// <summary>
        /// 석고상 깨질 때 나는 소리
        /// </summary>
        public AudioClip plasterBrokenSound;

        /// <summary>
        /// 배경음 소리 크기
        /// </summary>
        public float soundVolume;

        /// <summary>
        /// 실행 될 애니메이션
        /// </summary>
        private Animation animationclip;



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 아이템을 사용하면 Item.cs에서 호출
        /// </summary>
        private void ItemUse()
        {
            useItemCheck = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                useItemCheck = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (useItemCheck && onceCheck)
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                    {
                        GetComponent<AudioSource>().PlayOneShot(plasterBrokenSound, soundVolume); //석고상 깨지는 소리

                        StoryOff();

                        GetComponent<AudioSource>().PlayOneShot(unlockSound, soundVolume); //잠금 푸는 소리
                    }
                }
            }
        }

        /// <summary>
        /// 이 object가 가진 스토리가 시작할 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOn()
        {
			animationclip = this.transform.parent.gameObject.GetComponent<Animation>();
			onceCheck = true;
			Debug.Log("StoryOn호출");
			/*
            onceCheck = true;
            Debug.Log("StoryOn호출");*/
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOff()
        {
			animationclip = this.transform.parent.gameObject.GetComponent<Animation>();
			animationclip.Play("frame");

            openDoorObj.GetComponent<DoorAnim>().doorLockCheck = false;

            openOfficeDoorObjA.GetComponent<DoorAnim>().doorLockCheck = false;
            openOfficeDoorObjB.GetComponent<DoorAnim>().doorLockCheck = false;

            onceCheck = false;
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
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
            animationclip = this.transform.parent.gameObject.GetComponent<Animation>();
            animationclip.Play("frame");

            onceCheck = false;

            openOfficeDoorObjA.GetComponent<DoorAnim>().doorLockCheck = false;
            openOfficeDoorObjB.GetComponent<DoorAnim>().doorLockCheck = false;
            openDoorObj.GetComponent<DoorAnim>().doorLockCheck = false;
        }

    }
}
