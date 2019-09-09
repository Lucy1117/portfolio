using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public enum TipButtonStat
    {
        AButton,
        BButton,
        YButton,
        XButton
    };

    public class TipButtonUI : MonoBehaviour
    {
        public float xPos;
        public float yPos;

        private static CanvasGroup canvasGroup;
        private GameObject myObject;
        private RectTransform myTransform;

        private bool canvasonce;

        private bool fadingIn;
        private bool fadingOut;
        public float defaultTime;

        private float fadeTime;
        private float twinkleTime;

        /// <summary>
        /// tip이 켜져도 되는지.
        /// </summary>
        public bool tipCall;

        public bool tipOn;
        public bool tipOff;

        private bool gamePlay;

        public TipButtonStat myTipbt;

        private void Awake()
        {
            myObject = this.gameObject;
            tipCall = false;
            tipOn = false;
            tipOff = true;
            fadeTime = defaultTime;
            twinkleTime = defaultTime;
            gamePlay = false;
            canvasonce = true;
            //canvasRenderer.SetAlpha(0);
        }

        // Use this for initialization
        void Start()
        {
            myTransform = myObject.GetComponent<RectTransform>();
            fadingIn = true;
            ImActivate(8);
            myTransform.localPosition = new Vector3(xPos, yPos);
        }

        // Update is called once per frame
        void Update()
        {
            if (tipCall && gamePlay)
            {
                if (canvasonce)
                {
                    canvasGroup = myObject.GetComponent<CanvasGroup>();
                    canvasonce = false;
                }
                if (tipOn)
                {
                    twinkleTime = twinkleTime - Time.deltaTime;
                    if (twinkleTime < 0.0f)
                    {
                        StartCoroutine("FadeOut");
                        tipOn = false;
                        tipOff = true;
                    }
                }
                if (tipOff)
                {
                    twinkleTime = twinkleTime + Time.deltaTime;
                    if (twinkleTime > defaultTime)
                    {
                        StartCoroutine("FadeIn");
                        tipOn = true;
                        tipOff = false;
                    }
                }

                if (InputManager.AButton())
                {
                    ButtonInputDown(1);
                }
                else if (InputManager.BButton())
                {
                    ButtonInputDown(2);
                }
                else if (InputManager.YButton())
                {
                    ButtonInputDown(3);
                }
                else if (InputManager.XButton())
                {
                    ButtonInputDown(4);
                }
            }
            else
            {
                if (!canvasonce)
                {
                    StartCoroutine("FadeOut");
                }
                tipOn = false;
                tipOff = true;
            }
        }

        /// <summary>
        /// 누르는 버튼과 내 버튼의 이름이 맞으면 TipUI를 끈다.
        /// </summary>
        /// <param name="num"></param>
        private void ButtonInputDown(int num)
        {
            switch (myTipbt)
            {
                case TipButtonStat.AButton:
                    if(num == 1)
                    {
                        StoryOff();
                    }
                    break;
                case TipButtonStat.BButton:
                    if (num == 2)
                    {
                        StoryOff();
                    }
                    break;
                case TipButtonStat.YButton:
                    if (num == 3)
                    {
                        StoryOff();
                    }
                    break;
                case TipButtonStat.XButton:
                    if (num == 4)
                    {
                        StoryOff();
                    }
                    break;
            }
        }


        /// <summary>
        /// 활성화해도 좋은지
        /// </summary>
        /// <param name="guiStateNum"></param>
        public void ImActivate(int guiStateNum)
        {
            if (guiStateNum <= 3)
            {
                gamePlay = false;
            }
            else if (guiStateNum >= 4)
            {
                gamePlay = true;
            }
        }

        /// <summary>
        /// 이 object가 가진 스토리가 시작할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StoryOn()
        {
            Debug.Log("StoryOn호출");
            tipCall = true;
            tipOn = true;
            tipOff = false;
        }

        /// <summary>
        /// 이 object가 가진 스토리를 스킵할 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StorySkip()
        {
            if(this.transform.GetChild(0).GetComponent<MyStoryDetail>().myStoryNum == 2)
            {
                GameObject messagePopObj = GameObject.Find("MessagePop");
                messagePopObj.SendMessage("MessageCallDown");
            }
            Debug.Log("여기1");
            Destroy(myObject);
        }

        /// <summary>
        /// 이 object가 가진 스토리가 끝났을 때.
        /// 자식인 StoryInformObj의 MyStoryDetail.cs에서 호출
        /// </summary>
        public void StoryOff()
        {
            Debug.Log("StoryOff호출");
            tipCall = false;
            tipOn = false;
            tipOff = true;
            myObject.BroadcastMessage("StoryEnd");
            Destroy(myObject);
            Debug.Log("여기2");
        }


        #region MessagePopup Fade In/Out

        /// <summary>
        /// MessagePopup FadeOut
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOut()
        {
            if (!fadingOut)
            {
                fadingOut = true;
                fadingIn = false;
                StopCoroutine("FadeIn");

                float startAlpha = canvasGroup.alpha;

                float rate = 1.0f / fadeTime;

                float progress = 0.0f;

                while (progress < 1.0)
                {
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);

                    progress += rate * Time.deltaTime;

                    yield return null;
                }
                canvasGroup.alpha = 0;
                fadingOut = false;
            }
        }


        /// <summary>
        /// MessagePopup FadeIn
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeIn()
        {
            if (!fadingIn)
            {
                fadingOut = false;
                fadingIn = true;
                StopCoroutine("FadeOut");

                float startAlpha = canvasGroup.alpha;

                float rate = 1.0f / fadeTime;

                float progress = 0.0f;

                while (progress < 1.0)
                {
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);

                    progress += rate * Time.deltaTime;

                    yield return null;
                }
                canvasGroup.alpha = 1;
                fadingIn = false;
            }
        }
        #endregion
    }
}