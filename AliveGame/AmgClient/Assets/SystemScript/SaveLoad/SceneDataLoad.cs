using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JM.MyProject.MyGame
{

    /// <summary>
    /// 모든 씬의 이름을 넣기
    /// </summary>
    public enum SceneName
    {
        Default,
        LoadScene,
        StartMenu,
        Prologue,
        Tutorial,
        ChapterOneFirstFloor,
        ChapterOneSecondFloor,
        ChapterOneThirdFloor,
        InputMain
    };
    

    /// <summary>
    /// scene이 바뀔 때, 그 전의 scene이 무엇이었는지 Data를 전송하는 Gameobject
    /// 각각의 scene마다 모두 가지고 있으며, 각 씬마다 sceneNumber가 다름
    /// LoadScene기능을 위해 만들어진 것.
    /// </summary>
    public class SceneDataLoad : MonoBehaviour
    {
        /// <summary>
        /// 현재 자신의 씬
        /// </summary>
        public SceneName myScene;
        /// <summary>
        /// 전의 씬(로드씬을 호출했던 씬)
        /// </summary>
        public SceneName fromScene;
        /// <summary>
        /// 바뀔 씬(로드씬을 호출할 씬
        /// </summary>
        public SceneName toScene;

        
        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
           
        }
        
        /// <summary>
        /// 씬을 바꿀 때, 그 바꿀 씬에 대한 데이터를 보냄.
        /// </summary>
        /// <param name="sceneNum"></param>
        public void LoadSceneData(SceneName goScene)
        {
            toScene = goScene;
            
            if(myScene != SceneName.LoadScene)
            {
                this.SendMessage("ChangeSceneInform", myScene);
                //내 씬이 지금 로드씬이 아니면
                //일단 로드씬으로 옮김
                SceneManager.LoadScene("LoadingScene");
                StartCoroutine(WaitLoadScene());
            }
            else
            {
                //내 씬이 지금 로드씬이면
                //전의 씬에서 받아온 데이터의 ToScene으로 옮김
                SceneChange(goScene);
            }
            fromScene = myScene;
        }

        /// <summary>
        /// 게임 데이터를 로드한 경우.
        /// </summary>
        /// <param name="sceneNum"></param>
        public void LoadGameData(SceneName goScene)
        {
            toScene = goScene;

            if (myScene != SceneName.LoadScene)
            {
                //내 씬이 지금 로드씬이 아니면
                //일단 로드씬으로 옮김
                SceneManager.LoadScene("LoadingScene");
                StartCoroutine(WaitLoadGame());
            }
            else
            {
                //내 씬이 지금 로드씬이면
                //전의 씬에서 받아온 데이터의 ToScene으로 옮김
                SceneChange(goScene);
            }
            fromScene = myScene;
        }

        /// <summary>
        /// 씬을 바꾸는 함수
        /// </summary>
        /// <param name="goScene"></param>
        public void SceneChange(SceneName goScene)
        {
            Debug.Log("현재 씬은" + myScene);
            toScene = goScene;
            switch (toScene)
            {
                case SceneName.Default:
                    Debug.Log("Default");
                    break;
                case SceneName.LoadScene:
                    Debug.Log("LoadScene");
                    break;
                case SceneName.StartMenu:
                    Debug.Log("StartMenu");
                    SceneManager.LoadScene("StartMenu");
                    break;
                case SceneName.Prologue:
                    Debug.Log("Prologue");
                    SceneManager.LoadScene("Prologue");
                    break;
                case SceneName.Tutorial:
                    Debug.Log("Tutorial");
                    SceneManager.LoadScene("Tutorial");
                    break;
                case SceneName.ChapterOneFirstFloor:
                    SceneManager.LoadScene("StorymodeFBFirstFloor");
                    Debug.Log("ChOneFirstFloor");
                    break;
                case SceneName.ChapterOneSecondFloor:
                    SceneManager.LoadScene("StorymodeFBSecondFloor");
                    Debug.Log("ChapterOneSecondFloor");
                    break;
                case SceneName.ChapterOneThirdFloor:
                    SceneManager.LoadScene("StorymodeFBThirdFloor");
                    Debug.Log("ChapterOneThirdFloor");
                    break;
                case SceneName.InputMain:
                    Debug.Log("InputMain");
                    SceneManager.LoadScene("inputmain");
                    break;
            }
        }

        private IEnumerator WaitLoadScene()
        {
            Debug.Log("코루틴시작");
            yield return new WaitForSeconds(3.0f);
            LoadSceneData(toScene);
        }

        private IEnumerator WaitLoadGame()
        {
            Debug.Log("코루틴시작");
            yield return new WaitForSeconds(3.0f);
            LoadGameData(toScene);
        }
    }
}
