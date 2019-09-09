using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class ChapterIntro : MonoBehaviour
    {
        private static CanvasGroup canvasGroup;
        
        private bool fadingIn;
        private bool fadingOut;
        public float fadeTime;


        public float timer = 4.0f;
        public bool timerCheck;

        public GameObject textObject;

        private Text textSize;

        public string tDetail;

        /// <summary>
        /// chapter를 나타내는 canvas의 alpha값이 0이면 false
        /// chapter canvas가 사라지면 false이다.
        /// </summary>
        public bool chapterCanvasState;

        public GUIState guiStat;

        private GameObject initObj;

        private bool checkOnce;

        private void Awake()
        {
            timerCheck = true;
            canvasGroup = transform.GetComponent<CanvasGroup>();
            textSize = textObject.GetComponent<Text>();
            chapterCanvasState = true;
            initObj = GameObject.Find("Initiate");
            checkOnce = true;
        }
        // Use this for initialization
        void Start()
        {
            textSize.text = tDetail;
        }

        // Update is called once per frame
        void Update()
        {
            if (guiStat == GUIState.on)
            {
                if (timerCheck)
                {
                    timer = timer - Time.deltaTime;
                    if (timer < 0.0f)
                    {
                        timerCheck = false;
                        StartCoroutine("FadeOut");
                        chapterCanvasState = false;
                        guiStat = GUIState.off;
                    }
                    if (GameObject.Find("Inventory"))
                    {
                        initObj.SendMessage("GUIOnCheck");
                    }
                }
                else
                {
                    canvasGroup.alpha = 0;
                    chapterCanvasState = false;
                    guiStat = GUIState.off;

                    if (GameObject.Find("Inventory"))
                    {
                        initObj.SendMessage("GUIOnCheck");
                    }
                }
            }
            else if(guiStat == GUIState.off && checkOnce)
            {
                canvasGroup.alpha = 0;
                chapterCanvasState = false;
                if (GameObject.Find("Inventory"))
                {
                    initObj.SendMessage("GUIOnCheck");
                }
                timerCheck = false;
                checkOnce = false;
                //Destroy(this.gameObject);
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
