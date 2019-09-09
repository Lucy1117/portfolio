using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class LightSwitchButton : MonoBehaviour
    {
        private bool switchOK;

        public bool m_switchOK
        {
            get { return switchOK; }
            set { switchOK = value; }
        }

        public bool turnOn;
        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        public GameObject activeTip;

        /// <summary>
        /// 불이 켜졌을 때의 Material
        /// </summary>
        public Material turnOnMat;
        public Material turnOffMat;
        public Material lightTurnOnMat;
        public Material lightTurnOffMat;

        /// <summary>
        /// 연결해서 켤 라이트 모음.
        /// 라이트를 모두 모아놓고 setActive(true)로 하면 될 듯
        /// </summary>
        public GameObject lightObj;

        public GameObject doorObjA;
        public GameObject doorObjB;

        private GameObject[] ceilingLightObjList;

        /// <summary>
        /// 맨 처음 눌렀을 때, 문이 닫히도록 한 번만 체크.
        /// </summary>
        private bool doorOnce;


        // Use this for initialization
        void Start()
        {
            ceilingLightObjList = GameObject.FindGameObjectsWithTag("CeilingLightColor");
            if (turnOn)
            {
                LightMatChange(turnOnMat);
                CeilingLightMatChange(lightTurnOnMat);
            }
            else
            {
                LightMatChange(turnOffMat);
                CeilingLightMatChange(lightTurnOffMat);
                doorOnce = true;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 형광등을 켜는 함수
        /// </summary>
        private void LightTurnOn()
        {
            //딸깍 소리.
            //라이트 켜짐.
            lightObj.SetActive(true);
            LightMatChange(turnOnMat);
            CeilingLightMatChange(lightTurnOnMat);
            turnOn = true;
            if (doorOnce)
            {
                if (doorObjA.GetComponent<DoorAnim>().opening)
                {
                    doorObjA.SendMessage("RemoteDoorControl");
                }
                if (doorObjB.GetComponent<DoorAnim>().opening)
                {
                    doorObjB.SendMessage("RemoteDoorControl");
                }
                doorObjA.GetComponent<DoorAnim>().doorLockCheck = true;
                doorObjB.GetComponent<DoorAnim>().doorLockCheck = true;
                doorOnce = false;
            }
        }

        /// <summary>
        /// 형광등을 끄는 함수
        /// </summary>
        private void LightTurnOff()
        {
            //딸깍 소리.
            //라이트 꺼짐.
            lightObj.SetActive(false);
            LightMatChange(turnOffMat);
            CeilingLightMatChange(lightTurnOffMat);
            turnOn = false;
        }

        private void LightMatChange(Material changeMat)
        {
            for (int i = 1; i < 17; i++)
            {
                this.transform.GetChild(i).GetComponent<MeshRenderer>().material = changeMat;
            }
        }
        private void CeilingLightMatChange(Material changeMat)
        {
            //ceilingLightObjList
            foreach (GameObject ceLi in ceilingLightObjList)
            {
                ceLi.GetComponent<MeshRenderer>().material = changeMat;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
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
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8 && switchOK)
                    {
                        if (lightObj.activeSelf)
                        {
                            LightTurnOff();
                        }
                        else
                        {
                            StoryOff();
                        }
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
            LightTurnOn();
            if (childstoryObj)
            {
                childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
                childstoryObj.BroadcastMessage("StorySkipNum");
                childstoryObj.BroadcastMessage("StoryEnd");
            }
            Debug.Log("StoryOff호출");

        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            activeTip.GetComponent<CanvasGroup>().alpha = 0;
            LightTurnOn();
        }
    }
}
