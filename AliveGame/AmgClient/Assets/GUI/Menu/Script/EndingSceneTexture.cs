using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// EndingColllect의 Background에서 각각의 EndingSceneSlot이 확대되어 크게 보일 때 쓰임.
    /// 알맞은 텍스쳐를 지정해주고 bButton이 눌렸을 때 뒤로가기 기능.
    /// 모은 엔딩에 따라 활성화되고 안되고 차이를 줘서 코딩하면 될 듯.
    /// </summary>
    public class EndingSceneTexture : MonoBehaviour
    {
        private int myNum;
        private GameObject toSlot;

        public Sprite sceneTexture1;
        public Sprite sceneTexture2;
        public Sprite sceneTexture3;
        public Sprite sceneTexture4;
        public Sprite sceneTexture5;
        public Sprite sceneTexture6;
        public Sprite sceneTexture7;
        public Sprite sceneTexture8;
        public Sprite sceneTexture9;
        public Sprite sceneTexture10;
        public Sprite sceneTexture11;
        public Sprite sceneTexture12;


        // Use this for initialization
        void Start()
        {
            toSlot = GameObject.Find(myNum.ToString());

            switch (myNum)
            {
                case 1:
                    GetComponent<Image>().sprite = sceneTexture1;
                    break;
                case 2:
                    GetComponent<Image>().sprite = sceneTexture2;
                    break;
                case 3:
                    GetComponent<Image>().sprite = sceneTexture3;
                    break;
                case 4:
                    GetComponent<Image>().sprite = sceneTexture4;
                    break;
                case 5:
                    GetComponent<Image>().sprite = sceneTexture5;
                    break;
                case 6:
                    GetComponent<Image>().sprite = sceneTexture6;
                    break;
                case 7:
                    GetComponent<Image>().sprite = sceneTexture7;
                    break;
                case 8:
                    GetComponent<Image>().sprite = sceneTexture8;
                    break;
                case 9:
                    GetComponent<Image>().sprite = sceneTexture9;
                    break;
                case 10:
                    GetComponent<Image>().sprite = sceneTexture10;
                    break;
                case 11:
                    GetComponent<Image>().sprite = sceneTexture11;
                    break;
                case 12:
                    GetComponent<Image>().sprite = sceneTexture12;
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (InputManager.BButton())
            {
                toSlot.SendMessage("FromScene");
                GameObject.Destroy(this.gameObject);
            }
        }

        public void SceneNumber(int sNum)
        {
            myNum = sNum;
           // Debug.Log(sNum);
        }
    }
}
