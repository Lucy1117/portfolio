using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// Chapter1. FirstBuilding의 SecondFloor에서 날아오는 붓에 들어갈 코드.
    /// </summary>
    public class SecondFloorBrush : MonoBehaviour
    {
        /// <summary>
        /// 스토리 순서를 강제적으로 바꿔주기 위함.
        /// </summary>
        public GameObject childstoryObj;

        /// <summary>
        /// 실행 될 애니메이션
        /// </summary>
        private Animation animationclip;

        /// <summary>
        /// 관계자외 출입금지 문(오른쪽 끝에 있는)
        /// </summary>
        public GameObject openDoorObj;

        /// <summary>
        /// 깨지는 유리
        /// </summary>
        public GameObject brokenGlass;

        /// <summary>
        /// 붓 날아오는 애니메이션이 실행되면 true, 아니면 false
        /// </summary>
        private bool animPlay;

        /// <summary>
        /// 붓 날아오는 효과음
        /// </summary>
        public AudioClip brushFlySound;

        /// <summary>
        /// 유리 깨지는 효과음
        /// </summary>
        public AudioClip glassBrokenSound;

        /// <summary>
        /// 효과음 소리 크기
        /// </summary>
        public float soundVolume;

        private bool skipCheck;

        // Use this for initialization
        void Start()
        {
            animationclip = this.gameObject.GetComponent<Animation>();
            animPlay = false;
        }

        // Update is called once per frame
        void Update()
        {
            
			if (animationclip.IsPlaying("brush_animation") && !skipCheck) //애니메이션 실행되면
            {
                animPlay = true; //애니메이션 실행되면 true
            }
            if (animPlay)
            { //유리벽에 박힌 채로 붓 애니메이션이 멈추면 바로 유리 애니메이션 실행
				if(!animationclip.IsPlaying("brush_animation")) //애니메이션 실행이 멈추면
                {
                    brokenGlass.SetActive(true);
                    GameObject.Find("Glass Wall").SetActive(false);
                    animPlay = false;
                    AudioSource audiosource = GetComponent<AudioSource>();
                    audiosource.PlayOneShot(glassBrokenSound, soundVolume);
                    brokenGlass.GetComponent<Animation>()["Take 001"].speed = 2.0f;
                    brokenGlass.GetComponent<Animation>().Play("Take 001");
                    GameObject.Find("BloodSpriteUnit").SetActive(false);

                    openDoorObj.GetComponent<DoorAnim>().doorLockCheck = false;
                    openDoorObj.SendMessage("RemoteDoorControl");
                    StoryOff();
                }
            }
            if (GameObject.Find("BloodSpriteUnit") && skipCheck)
            {
                GameObject.Find("BloodSpriteUnit").SetActive(false);
            }

        }

        /// <summary>
        /// 이 object가 가진 스토리가 시작할 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOn()
        {
            Debug.Log("StoryOn호출");
            StartCoroutine(WaitStory());
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj에서 호출
        /// </summary>
        public void StoryOff()
        {
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
            skipCheck = true;
            Debug.Log("스토리스킵");
			animationclip.Play("brush_animation");
            brokenGlass.SetActive(true);
            GameObject.Find("Glass Wall").SetActive(false);
            brokenGlass.GetComponent<Animation>().Play("Take 001");
            openDoorObj.GetComponent<DoorAnim>().doorLockCheck = false;
            openDoorObj.SendMessage("RemoteDoorControl");

            GameObject.Find("EffectSound").GetComponent<AudioSource>().volume = 0.0f;

        }

        /// <summary>
        /// 이전의 스토리 진행 이후, 2초 딜레이 후에 진행
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitStory()
        {
            Debug.Log("코루틴시작");
            yield return new WaitForSeconds(2.0f);
            animationclip.Play("brush_animation");
            AudioSource audiosource = GetComponent<AudioSource>();
            audiosource.PlayOneShot(brushFlySound, soundVolume);
        }
    }
}
