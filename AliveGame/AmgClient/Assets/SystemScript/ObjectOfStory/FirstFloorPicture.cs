using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 FirstFloor에서 액자에 들어갈 코드.
    /// </summary>
    public class FirstFloorPicture : MonoBehaviour
    {
		private Animation animationclip;
        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;
        public GameObject activeTip;

        public GameObject keyObj;

        /// <summary>
        /// 연결해서 켤 라이트 모음.
        /// 라이트를 모두 모아놓고 setActive(true)로 하면 될 듯
        /// </summary>
        public GameObject lightObj;

        /// <summary>
        /// 불이 켜졌을 때의 Material
        /// </summary>
        public Material lightTurnOffMat;
        public Material lightTurnOnMat;

        private GameObject[] ceilingLightObjList;

        private GameObject sceneDataObj;

        /// <summary>
        /// 열쇠 떨어질 때 나는 소리
        /// </summary>
        public AudioClip keySound;

        /// <summary>
        /// 배경음 소리 크기
        /// </summary>
        public float soundVolume;


        /// <summary>
        /// 한번만 실행되도록
        /// </summary>
        private bool onceCheck;

        private bool onceOK;

        // Use this for initialization
        void Start()
        {
            if (GameObject.Find("SceneData"))
            {
                sceneDataObj = GameObject.Find("SceneData");
            }
            else
            {
                if (GameObject.Find("SceneData(Clone)"))
                {
                    sceneDataObj = GameObject.Find("SceneData(Clone)");
                }
            }
            animationclip = this.gameObject.GetComponent<Animation>();
            onceCheck = true;

            ceilingLightObjList = GameObject.FindGameObjectsWithTag("CeilingLightColor");
            onceOK = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (onceOK && sceneDataObj.GetComponent<StoryInform>().m_loadingEnd)
            {
                if (childstoryObj.GetComponent<MyStoryDetail>().currentStoryNum >
                    childstoryObj.GetComponent<MyStoryDetail>().myStoryNum + 1)
                {
                    lightObj.SetActive(false);
                    foreach (GameObject ceLi in ceilingLightObjList)
                    {
                        ceLi.GetComponent<MeshRenderer>().material = lightTurnOffMat;
                    }
                    onceOK = false;
                }
                else
                {
                    lightObj.SetActive(true);
                    foreach (GameObject ceLi in ceilingLightObjList)
                    {
                        ceLi.GetComponent<MeshRenderer>().material = lightTurnOnMat;
                    }
                    onceOK = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && onceCheck)
            {
                activeTip.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (InputManager.XButton())
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8 && onceCheck)
                    {
                        //액자 흔들리는 애니메이션
                        animationclip.Play("frame");
                        StoryOff();
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
            GetComponent<AudioSource>().PlayOneShot(keySound, soundVolume);
            keyObj.SetActive(true);
            onceCheck = false;
            activeTip.GetComponent<CanvasGroup>().alpha = 0;
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
            keyObj.SetActive(true);
            onceCheck = false;
            activeTip.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
