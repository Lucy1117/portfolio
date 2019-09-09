using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 ThirdFloor에서 시계바늘에 들어갈 코드.
    /// </summary>
    public class ThirdFloorNeedle : MonoBehaviour
    {

        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;


        private GameObject[] monitorObj;

        public GameObject otherMonitorObj;


        public bool turnOn;

        /// <summary>
        /// 바뀔 Material 목록
        /// </summary>
        public Material turnRedMat;
        public Material turnBlueMat;
        public Material defalutMat;

        public GameObject cctvKeyObj;

        // Use this for initialization
        void Start()
        {
            monitorObj = GameObject.FindGameObjectsWithTag("ChangeMonitor");
            turnOn = false;

            ChangeMaterial();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ChangeMaterial()
        {
            if (turnOn)
            {
                //ceilingLightObjList
                foreach (GameObject moLi in monitorObj)
                {
                    moLi.GetComponent<MeshRenderer>().material = turnBlueMat;
                }

                otherMonitorObj.GetComponent<MeshRenderer>().material = turnRedMat;
            }
            else
            {  //ceilingLightObjList
                foreach (GameObject moLi in monitorObj)
                {
                    moLi.GetComponent<MeshRenderer>().material = defalutMat;
                }

                otherMonitorObj.GetComponent<MeshRenderer>().material = defalutMat;
            }
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
            GameObject.Find("EffectSound").SendMessage("EffectSoundPlay", 2);
            turnOn = true;
            ChangeMaterial();
            cctvKeyObj.SetActive(true);

            childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            childstoryObj.BroadcastMessage("StorySkipNum");
            childstoryObj.BroadcastMessage("StoryEnd");
            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }
            Debug.Log("StoryOff호출");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            GameObject.Find("EffectSound").SendMessage("EffectSoundPlay", 2);
            turnOn = true;
            ChangeMaterial();
            cctvKeyObj.SetActive(true);

            if (this.gameObject.activeSelf)
            {
                this.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
