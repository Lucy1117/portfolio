using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 게임 내에서 B버튼을 누르면 활성화되는 게임메뉴
    /// </summary>
    public class InGameMenu : MonoBehaviour
    {
        private static CanvasGroup canvasGroup;
        
        private GameObject myObject;
        private GameObject initObj;

        private bool fadingIn;
        private bool fadingOut;
        public float fadeTime;

        public GUIState guiStat = GUIState.off;

        /// <summary>
        /// Chapter 열리면 game이 멈추므로, 이를 나타내주는 변수.
        /// </summary>
        private bool gamePlay = false;

        // Use this for initialization
        void Start()
        {
            canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            myObject = this.gameObject;
            initObj = GameObject.Find("Initiate");
        }

        // Update is called once per frame
        void Update()
        {
            if (gamePlay)
            {
                if (InputManager.BButton())
                {
                    GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 4);
                    //B버튼을 누르면
                    if (canvasGroup.alpha > 0)
                    {
                        guiStat = GUIState.off;
                        myObject.BroadcastMessage("InGameMenuOff");
                        StartCoroutine("FadeOut");
                        
                        GameObject.Find("BackGroundSound").GetComponent<AudioSource>().UnPause();
                        if (GameObject.Find("EffectSound"))
                        {
                            GameObject.Find("EffectSound").GetComponent<AudioSource>().UnPause();
                        }
                        AudioSource audiosource = GetComponent<AudioSource>();
                        audiosource.Stop();
                    }
                    else
                    {
                        guiStat = GUIState.on;
                        StartCoroutine("FadeIn");
                        initObj.SendMessage("VisibleInformRead");
                        GameObject.Find("BackGroundSound").GetComponent<AudioSource>().Pause();
                        if (GameObject.Find("EffectSound"))
                        {
                            GameObject.Find("EffectSound").GetComponent<AudioSource>().Pause();
                        }
                        AudioSource audiosource = GetComponent<AudioSource>();
                        audiosource.Play();
                    }
                    initObj.SendMessage("GUIOnCheck");
                }
            }
        }

        /// <summary>
        /// InGameMenuButton.cs에서 호출하는 함수
        /// </summary>
        public void BDown()
        {
            GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 4);
            guiStat = GUIState.off;
            myObject.BroadcastMessage("InGameMenuOff");
            StartCoroutine("FadeOut");
            initObj.SendMessage("GUIOnCheck");
            GameObject.Find("BackGroundSound").GetComponent<AudioSource>().UnPause();
            if (GameObject.Find("EffectSound"))
            {
                GameObject.Find("EffectSound").GetComponent<AudioSource>().UnPause();
            }
        }

        /// <summary>
        /// 활성화해도 좋은지
        /// </summary>
        /// <param name="guiStateNum"></param>
        public void ImActivate(int guiStateNum)
        {
            if(guiStateNum == 1)
            {
                gamePlay = false;
            }
            else if(guiStateNum >= 2)
            {
                gamePlay = true;
            }
        }

        #region InGameMenu Fade In/Out

        /// <summary>
        /// InGameMenu가 천천히 사라지는 것처럼 보이도록
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
        /// InGameMenu가 천천히 생겨나는 것처럼 보이도록
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
                myObject.BroadcastMessage("Initiate");
            }
        }
        #endregion

    }
}