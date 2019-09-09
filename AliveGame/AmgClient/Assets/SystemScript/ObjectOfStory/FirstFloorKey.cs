using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 FirstFloor에서 열쇠에 들어갈 코드.
    /// </summary>
    public class FirstFloorKey : MonoBehaviour
    {
        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        /// <summary>
        /// 연결해서 켤 라이트 모음.
        /// 라이트를 모두 모아놓고 setActive(true)로 하면 될 듯
        /// </summary>
        public GameObject lightObj;

        /// <summary>
        /// 불이 켜졌을 때의 Material
        /// </summary>
        public Material lightTurnOffMat;

        private GameObject[] ceilingLightObjList;
        
        // Use this for initialization
        void Start()
        {
            ceilingLightObjList = GameObject.FindGameObjectsWithTag("CeilingLightColor");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (InputManager.XButton())
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                    {
                        StoryOff();
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
            Debug.Log("StoryOn호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOff()
        {
            lightObj.SetActive(false);
            foreach (GameObject ceLi in ceilingLightObjList)
            {
                ceLi.GetComponent<MeshRenderer>().material = lightTurnOffMat;
            }
            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            childstoryObj.BroadcastMessage("StorySkipNum");
            childstoryObj.BroadcastMessage("StoryEnd");
            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }
            GameObject.Find("LightControl").GetComponent<LightSwitchButton>().m_switchOK = false;
            GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 5;
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 5);
            Debug.Log("StoryOff호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            lightObj.SetActive(false);
            foreach (GameObject ceLi in ceilingLightObjList)
            {
                ceLi.GetComponent<MeshRenderer>().material = lightTurnOffMat;
            }
            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }
            GameObject.Find("LightControl").GetComponent<LightSwitchButton>().m_switchOK = false;
            GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 5;
            GameObject.Find("BackGroundSound").SendMessage("SoundBGMPlay", 5);
        }
    }
}
