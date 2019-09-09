using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class GameOverGUI : MonoBehaviour
    {
        private static CanvasGroup canvasGroup;
        
        private bool fadingIn;
        private bool fadingOut;

        public float fadeTime;
        
        /// <summary>
        /// DeadAnimation이 끝난 후에 DeadGUI의 timer가 끝난 후의 초까지 계산.
        /// </summary>
        public float timer;
        private bool timerCheck = true;

        /// <summary>
        /// GameOver 화면에서 잠깐 멈췄다가 LoadScene 호출.
        /// </summary>
        public float changeTimer;


        // Use this for initialization
        void Start()
        {
            canvasGroup = transform.GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            if (timerCheck)
            {
                timer = timer - Time.deltaTime;
                if (timer < 0.0f)
                {
                    StartCoroutine("FadeIn");
                    timerCheck = false;
                }
            }

            if(canvasGroup.alpha == 1)
            {
                changeTimer = changeTimer - Time.deltaTime;
                if(changeTimer < 0.0f)
                {
                    this.gameObject.SendMessage("ChangeScene", SceneName.StartMenu);
                }
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
            }
        }
        #endregion
    }
}
