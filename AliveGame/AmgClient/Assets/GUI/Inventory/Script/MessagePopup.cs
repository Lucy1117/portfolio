using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class MessagePopup : MonoBehaviour
    {
        private static CanvasGroup canvasGroup;
        
        private bool fadingIn;
        private bool fadingOut;
        public float defaultTime;

        private float fadeTime;
        private float twinkleTime;

        public bool messageCall;

        private bool messageOn;
        private bool messageOff;

        private bool gamePlay;

        public AudioClip soundEffect;
        public float soundVolume;

        private void Awake()
        {
            messageOn = true;
            messageOff = false;
            fadeTime = defaultTime;
            twinkleTime = defaultTime;
            gamePlay = false;
        }

        // Use this for initialization
        void Start()
        {
            canvasGroup = transform.GetComponent<CanvasGroup>();
            fadingIn = true;
            ImActivate(8);
        }

        // Update is called once per frame
        void Update()
        {
            if (messageCall && gamePlay)
            {
                if (messageOn)
                {
                    twinkleTime = twinkleTime - Time.deltaTime;
                    if (twinkleTime < 0.0f)
                    {
                        messageOn = false;
                        messageOff = true;
                        StartCoroutine("FadeOut");
                    }
                }
                if (messageOff)
                {
                    twinkleTime = twinkleTime + Time.deltaTime;
                    if (twinkleTime > defaultTime)
                    {
                        this.gameObject.GetComponent<AudioSource>().PlayOneShot(soundEffect, soundVolume);
                        messageOn = true;
                        messageOff = false;
                        StartCoroutine("FadeIn");
                    }
                }
            }
            else
            {
                messageOn = false;
                messageOff = true;
                StartCoroutine("FadeOut");
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

        public void MessageCallDown()
        {
            messageCall = false;
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
