using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 문 열리는 애니메이션
    /// </summary>
    public class DoorAnim : MonoBehaviour
    {
        /// <summary>
        /// 캐릭터가 문 앞에 있을 때 피벗의 위치
        /// </summary>
        public enum DoorState
        {
            Left,
            Right
        };

        /// <summary>
        /// 캐릭터가 문 앞에 있을 때 문이 열려야 하는 방향
        /// </summary>
        public enum OpenDirection
        {
            Indoor,
            Outdoor
        };

        public bool doorLockCheck;
        /// <summary>
        /// 문 열리는 시간, 닫히는 시간(slerp변화 시간)
        /// </summary>
        public float changeTime = 0;

        /// <summary>
        /// 기즈모가 포함된, 현실적으로 각도가 변하는 오브젝트
        /// </summary>
        private Transform parentObj;
        
        public DoorState dState;
        public OpenDirection opendirect;

        /// <summary>
        /// 문이 열려있으면 true
        /// </summary>
        public bool opening;

        /// <summary>
        /// 원래의 각도
        /// </summary>
        private float defaultYRot;
        /// <summary>
        /// 바뀔 각도
        /// </summary>
        private float changeYRot;

        private bool onceCheck;

        private bool moveCheck;

        /// <summary>
        /// 문 본체 Mesh
        /// </summary>
        public GameObject doorMesh;

        /// <summary>
        /// 문고리 본체 Mesh
        /// </summary>
        public GameObject doorHandlerMesh;
        

        /// <summary>
        /// object 앞에 활성화되는 tip
        /// </summary>
        public GameObject activeTipIn;
        public GameObject activeTipOut;

        public AudioClip doorOpenSound;
        public AudioClip doorCloseSound;
        public AudioClip doorLockSound;
        public float soundVolume;

        /// <summary>
        /// 문이 열리는 각도
        /// </summary>
        public float openAngle;

        /// <summary>
        /// 한번만 플레이되도록.
        /// </summary>
        private bool playOnce;


        private void Awake()
        {
            parentObj = this.gameObject.transform.parent;
            //localEulerAngles을 써야 degree로 표시됨.
            defaultYRot = parentObj.localEulerAngles.y;
            if(defaultYRot == 0 )
            {
                if((opendirect == OpenDirection.Outdoor) && (dState == DoorState.Right))
                {

                }
                else
                {
                    defaultYRot = 360.0f;
                    parentObj.localRotation = Quaternion.Euler(0, defaultYRot, 0);
                }
            }
            onceCheck = false;
            opening = false;
            moveCheck = false;
        }

        // Use this for initialization
        void Start()
        {
            DoorBasic();
        }

        // Update is called once per frame
        void Update()
        {

            if (InputManager.XButtonUp())
            {
                playOnce = true;
            }

            if (moveCheck)
            {
                if (!opening)
                {
                    //닫힘 -> 열림
                    parentObj.localRotation = Quaternion.Slerp(parentObj.localRotation, Quaternion.Euler(0, changeYRot, 0), Time.time * changeTime);
                }
                else
                {
                    //열림 -> 닫힘

                    parentObj.localRotation = Quaternion.Slerp(parentObj.localRotation, Quaternion.Euler(0, defaultYRot, 0), Time.time * changeTime);
                }
            }

            OpenCrashException();

           //Debug.Log(parentObj.localRotation.y + " " + Quaternion.Euler(0, defaultYRot, 0).y + " " + Quaternion.Euler(0, changeYRot, 0).y);
            //Debug.Log(parentObj.localEulerAngles.y + " " + defaultYRot + " " + changeYRot);
            //Debug.Log(Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f);

            if (Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f == changeYRot)
            {
                if (onceCheck)
                {
                    Debug.Log("열림");
                    opening = true;
                    onceCheck = false;
                    moveCheck = false;
                }
                //Debug.Log(opening + " lllll" + onceCheck);
            }
            else if(Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f == defaultYRot)
            {
                if (onceCheck)
                {
                    Debug.Log("닫힘");
                    opening = false;
                    onceCheck = false;
                    moveCheck = false;
                }
                //Debug.Log(opening + " kkkkkkk " + onceCheck);
            }
            else if(Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f == 0.0f)
            {
                if (onceCheck)
                {
                    if (defaultYRot == 360.0f)
                    {
                        if ((opendirect == OpenDirection.Indoor) && (dState == DoorState.Right))
                        {
                            parentObj.localRotation = Quaternion.Euler(0, defaultYRot, 0);
                        }
                    }
                    Debug.Log("닫힘");
                    opening = false;
                    onceCheck = false;
                    moveCheck = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                activeTipIn.GetComponent<CanvasGroup>().alpha = 0.4f;
                activeTipOut.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
            playOnce = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (playOnce)
                {
                    if (InputManager.XButton())
                    {
                        if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                        {
                            onceCheck = true;

                            if (doorLockCheck)
                            {
                                SoundEffectPlay(3);
                            }
                            else
                            {
                                if (opening)
                                {
                                    SoundEffectPlay(2);
                                }
                                else
                                {
                                    SoundEffectPlay(1);
                                }
                                activeTipIn.GetComponent<CanvasGroup>().alpha = 0;
                                activeTipOut.GetComponent<CanvasGroup>().alpha = 0;
                                moveCheck = true;
                            }
                        }
                        playOnce = false;
                    }
                }
            }
        }

        /// <summary>
        /// 다른 obj에서 문을 제어할 때 쓰는 함수.
        /// 현재는 Tutorial Scene의  LightSwitchButton.cs에서 제어.
        /// </summary>
        private void RemoteDoorControl()
        {
            if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
            {
                if (opening)
                {
                    onceCheck = true;
                    SoundEffectPlay(2);
                    moveCheck = true;
                }
                else
                {
                    onceCheck = true;
                    SoundEffectPlay(1);
                    moveCheck = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                activeTipIn.GetComponent<CanvasGroup>().alpha = 0;
                activeTipOut.GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        /// <summary>
        /// 문 이펙트 사운드.
        /// 1 = open,  2= close, 3 = lock;
        /// </summary>
        /// <param name="num"></param>
        public void SoundEffectPlay(int num)
        {
            if (moveCheck)
            {
                //Debug.Log("움직이는 중엔 효과음 재생 안 함");
            }
            else
            {
                AudioSource audiosource = this.gameObject.GetComponent<AudioSource>();

                switch (num)
                {
                    case 1:
                        audiosource.PlayOneShot(doorOpenSound, soundVolume);
                        break;
                    case 2:
                        audiosource.PlayOneShot(doorCloseSound, soundVolume);
                        break;
                    case 3:
                        audiosource.PlayOneShot(doorLockSound, soundVolume);
                        break;
                }
            }
        }

        private void DoorBasic()
        {
            switch (dState)
            {
                case DoorState.Left:
                    if(opendirect == OpenDirection.Indoor)
                    {
                        changeYRot = defaultYRot + openAngle;
                    }
                    else if (opendirect == OpenDirection.Outdoor)
                    {
                        changeYRot = defaultYRot - openAngle;
                    }
                    break;
                case DoorState.Right:
                    if (opendirect == OpenDirection.Indoor)
                    {
                        changeYRot = defaultYRot - openAngle;
                    }
                    else if (opendirect == OpenDirection.Outdoor)
                    {
                        changeYRot = defaultYRot + openAngle;
                    }
                    break;
            }
        }

        /// <summary>
        /// 문 열리는 동안 doorMesh와 충돌하지 않도록.
        /// </summary>
        private void OpenCrashException()
        {
            if (((dState == DoorState.Left) && (opendirect == OpenDirection.Indoor)) || ((dState == DoorState.Right) && (opendirect == OpenDirection.Outdoor)))
            {
                if (Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f > defaultYRot && Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f < changeYRot)
                {
                    doorMesh.GetComponent<BoxCollider>().isTrigger = true;
                    doorHandlerMesh.GetComponent<BoxCollider>().isTrigger = true;
                }
                else
                {
                    doorMesh.GetComponent<BoxCollider>().isTrigger = false;
                    doorHandlerMesh.GetComponent<BoxCollider>().isTrigger = false;
                }
            }
            else if (((dState == DoorState.Left) && (opendirect == OpenDirection.Outdoor)) || ((dState == DoorState.Right) && (opendirect == OpenDirection.Indoor)))
            {
                if (Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f < defaultYRot && Mathf.Round(parentObj.localEulerAngles.y * 10.0f) / 10.0f > changeYRot)
                {
                    doorMesh.GetComponent<BoxCollider>().isTrigger = true;
                    doorHandlerMesh.GetComponent<BoxCollider>().isTrigger = true;
                }
                else
                {
                    doorMesh.GetComponent<BoxCollider>().isTrigger = false;
                    doorHandlerMesh.GetComponent<BoxCollider>().isTrigger = false;
                }
            }
        }
    }
}
