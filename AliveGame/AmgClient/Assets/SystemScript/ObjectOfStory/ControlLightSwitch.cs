using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JM.MyProject.MyGame
{
    public class ControlLightSwitch : MonoBehaviour
    {
        /// <summary>
        /// 내 Object
        /// </summary>
        private GameObject myObj;

        public GameObject leverObj;

        /// <summary>
        /// object 앞에 활성화되는 tip
        /// </summary>
        public GameObject activeTip;

        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        private Animation _animation;

        /// <summary>
        /// 스위치가 켜지면 1, 꺼져있으면 0
        /// </summary>
        public bool switchState;

        public GameObject lightPannelObj;

        /// <summary>
        /// 애니메이션이 겹쳐서 일어나지 않게
        /// </summary>
        private bool animPlayOK;

        //private GameObject initObj;

        //private GameObject[] lightSwitchList;

        private void Awake()
        {
            myObj = this.gameObject;
            _animation = leverObj.GetComponent<Animation>();

            //initObj = GameObject.FindGameObjectWithTag("Initiate");
            //lightSwitchList = GameObject.FindGameObjectsWithTag("LightSwitch");
        }
        // Use this for initialization
        void Start()
        {
            if (switchState)
            {
                _animation.Play("SwitchOn");
                //아래는 1016에 주석처리함. 1층씬에서 불이 꺼졌을 때 다시 차단기를 건들지 못하게 하려고.
                //switchState = !switchState;
                lightPannelObj.GetComponent<LightSwitchButton>().m_switchOK = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
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
            if (_animation.IsPlaying("SwitchOff") || _animation.IsPlaying("SwitchOn"))
            {
                animPlayOK = false;
            }
            else
            {
                animPlayOK = true;
            }

            if (other.gameObject.tag == "Player")
            {
                if (InputManager.XButton())
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8 && animPlayOK)
                    {
                        if (switchState) //켜져 있으면
                        {
                            /*
                            _animation.Play("SwitchOff");
                            //모든 전등이 꺼질 수 있게 그 object들의 bool 변수를 바꿈.
                            switchState = !switchState;

                            lightPannelObj.GetComponent<LightSwitchButton>().m_switchOK = false;

                            other.SendMessage("PlayerDead");
                            initObj.SendMessage("CreateDeadGUI");


                            Debug.Log("누전으로 사망");
                            */
                        }
                        else  //꺼져있으면
                        {
                            StoryOff();
                            
                            //모든 전등이 켜질 수 있게 그 object들의 bool 변수를 바꿈.
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
            _animation.Play("SwitchOn");
            switchState = !switchState;
            lightPannelObj.GetComponent<LightSwitchButton>().m_switchOK = true;

            Debug.Log("StoryOff호출");
            if (childstoryObj)
            {
                childstoryObj.GetComponent<MyStoryDetail>().sendOnce = false;
            }
            myObj.BroadcastMessage("StorySkipNum");
            myObj.BroadcastMessage("StoryEnd");
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            activeTip.GetComponent<CanvasGroup>().alpha = 0;
            _animation.Play("SwitchOn");
            switchState = !switchState;
            lightPannelObj.GetComponent<LightSwitchButton>().m_switchOK = true;
        }
    }
}
