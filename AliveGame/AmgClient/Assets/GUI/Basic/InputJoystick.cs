using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{   
    /// <summary>
     /// 버튼의 현재 상태를 나타낸다.
     /// 0=Default, 1=PointEnter, 2=PointClick
     /// </summary>
    public enum buttonState
    {
        original,
        highlight,
        check
    };

    /// <summary>
    /// 버튼 순서에 따른 이름.
    /// 버튼이 많아지면 여기서 추가하면 됨.
    /// </summary>
    public enum buttonNameTag
    {
        zero,
        first,
        second,
        third,
        fourth,
        fifth,
        sixth
    };

    /// <summary>
    /// JoyStick에 따른 Input. 상하로의 이동만 해당한다.
    /// </summary>
    public class InputJoystick : MonoBehaviour
    {

        public Sprite OriginalTexture;
        public Sprite HighlightTexture;
        public Sprite CheckTexture;
        protected GameObject myButton;
        public GameObject upButton;
        public GameObject downButton;

        private GameObject soundObj;

        protected bool moveButton = false;
        /// <summary>
        /// 현재 메뉴에 있는 상태이면
        /// </summary>
        protected bool menuIn = false;

        protected buttonState bs = buttonState.original;

        public buttonNameTag bNameTag = buttonNameTag.zero;

        private void Awake()
        {
            myButton = this.gameObject;
            menuIn = true;
            soundObj = GameObject.Find("ButtonSound");
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 이 버튼 object가 활성화될 때
        /// </summary>
        public void PointerOver()
        {
            if (InputManager.MainVertical() == 1.0f)
            {
                upButton.SendMessage("CallButtonOn");
                bs = buttonState.original;
                moveButton = false;
                soundObj.SendMessage("ButtonEffectPlay", 1);
            }
            else if (InputManager.MainVertical() == -1.0f)
            {
                downButton.SendMessage("CallButtonOn");
                bs = buttonState.original;
                moveButton = false;
                soundObj.SendMessage("ButtonEffectPlay", 1);
            }
            if (menuIn)
            {
                if (InputManager.AButton())
                {
                    bs = buttonState.check;
                    menuIn = false;
                    soundObj.SendMessage("ButtonEffectPlay", 2);
                }
            }
        }

        /// <summary>
        /// 이 버튼 object를 비활성화할 때
        /// </summary>
        public void PointerExit()
        {
            if (InputManager.MainVertical() == 0.0f)
            {
                moveButton = false;
            }
        }

        /// <summary>
        /// 이 버튼 object를 선택했을 때
        /// AButton을 눌렀다가 버튼에서 손을 놓을 때 활성화.
        /// </summary>
        public void PointerCheck()
        {
            if (Input.GetButtonUp("A_Button"))
            {
                switch(bNameTag)
                {
                    case buttonNameTag.zero:
                        break;
                    case buttonNameTag.first:
                        FirstButtonFun();
                        break;
                    case buttonNameTag.second:
                        SecondButtonFun();
                        break;
                    case buttonNameTag.third:
                        ThirdButtonFun();
                        break;
                    case buttonNameTag.fourth:
                        ForthButtonFun();
                        break;
                    case buttonNameTag.fifth:
                        FifthButtonFun();
                        break;
                    case buttonNameTag.sixth:
                        SixthButtonFun();
                        break;
                }
                moveButton = false;
                bs = buttonState.original;
            }
        }


        #region 가상함수 모음
        /// <summary>
        /// 첫번째 버튼에 적용되어야 할 내용을 담은 가상함수
        /// </summary>
        protected virtual void FirstButtonFun() { }
        /// <summary>
        /// 두번째 버튼에 적용되어야 할 내용을 담은 가상함수
        /// </summary>
        protected virtual void SecondButtonFun() { }
        /// <summary>
        /// 세번째 버튼에 적용되어야 할 내용을 담은 가상함수
        /// </summary>
        protected virtual void ThirdButtonFun() { }
        /// <summary>
        /// 네번째 버튼에 적용되어야 할 내용을 담은 가상함수
        /// </summary>
        protected virtual void ForthButtonFun() { }
        /// <summary>
        /// 다섯번째 버튼에 적용되어야 할 내용을 담은 가상함수
        /// </summary>
        protected virtual void FifthButtonFun() { }
        /// <summary>
        /// 여섯번째 버튼에 적용되어야 할 내용을 담은 가상함수
        /// </summary>
        protected virtual void SixthButtonFun() { }
        #endregion


        /// <summary>
        /// 다른 버튼 object에서 이동해서 이 object가 활성화될 때 SendMessage로 활성화
        /// 이 cs에서만 사용
        /// </summary>
        public void CallButtonOn()
        {
            bs = buttonState.highlight;
        }
        
        /// <summary>
        /// 각각의 메뉴 화면 버튼이 활성화되었다가 다시 메뉴로 돌아왔을 때, 다시 그 버튼이 활성화 되도록.
        /// 다른 cs에서 호출하는 함수(그 버튼의 세부내용에서 호출)
        /// </summary>
        public void ToMenu()
        {
            bs = buttonState.highlight;
            menuIn = true;
        }
    }
}
