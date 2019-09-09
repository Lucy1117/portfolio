using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// slot의 현재 상태를 나타낸다.
    /// empty, highlight, check : slot이 비어있을 경우
    /// fillItem, fillHighlight, fillCheck : slot에 Item이 있을 경우
    /// </summary>
    public enum slotState
    {
        empty,
        highlight,
        check,
        fillItem,
        fillHighlight,
        fillCheck
    }

    /// <summary>
    /// JoyStick에 따른 Input. 상하좌우 네 방향 모두로의 이동이 해당한다.
    /// </summary>
    public class InputJoystick2 : MonoBehaviour
    {
        //slot이 비어있을 때의 텍스쳐
        public Sprite slotEmpty;
        public Sprite slotHighlight;
        public Sprite slotCheck;

        //slot에 Item이 차 있을 때의 텍스쳐
        protected Sprite FillTexture;
        protected Sprite HighlightTexture;
        protected Sprite CheckTexture;

        protected GameObject myObject;
        protected GameObject upObject;
        protected GameObject downObject;
        protected GameObject leftObject;
        protected GameObject rightObject;

        protected int upNum;
        protected int downNum;
        protected int leftNum;
        protected int rightNum;

        /// <summary>
        /// 자신의 현재 slot Number
        /// </summary>
        protected int myNum = 0;

        //전체 slot에 대한 개수, 열, 행
        protected int slots = 0;
        protected int rows = 0;
        protected int columns = 0;

        /// <summary>
        /// 현재 highlight상태인 object만 A&B 버튼의 영향을 받도록.
        /// </summary>
        public bool buttonOn = false;

        /// <summary>
        /// 슬롯의 Default상태
        /// </summary>
        protected slotState obs = slotState.empty;

        /// <summary>
        /// 현재 slot에 아이템이 addItem된 상태이면 true / 아니면 false
        /// </summary>
        protected bool fillIn = false;

        /// <summary>
        /// 이 cs를 사용할 slot의 부모가 되는 object
        /// </summary>
        protected GameObject parentObject;

        protected GameObject soundObj;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        #region move관련 다른 곳에서 sendMessage로 받아오는 변수
        /// <summary>
        /// 만들어진 총 slot의 열 수를 받아옴.
        /// </summary>
        /// <param name="aNum"></param>
        public void RowsNum(int aNum)
        {
            rows = aNum;
        }

        /// <summary>
        /// 만들어진 총 slot의 개수를 받아옴.
        /// </summary>
        /// <param name="bNum"></param>
        public void SlotsNum(int bNum)
        {
            slots = bNum;
        }
        #endregion

        /// <summary>
        /// 내 번호를 기준으로 상하좌우 slot의 이름으로 연결
        /// 이를 통해 slot간의 이동이 가능해짐.
        /// </summary>
        protected void InsertNum()
        {
            columns = slots / rows;

            if (myNum - columns > 0)
            {
                upNum = myNum - columns;
            }
            else
            {
                upNum = myNum - columns + slots;
            }
            if (myNum + columns <= slots)
            {
                downNum = myNum + columns;
            }
            else
            {
                downNum = myNum + columns - slots;
            }
            if (myNum - 1 > 0)
            {
                leftNum = myNum - 1;
            }
            else
            {
                leftNum = myNum - 1 + slots;
            }
            if (myNum + 1 <= slots)
            {
                rightNum = myNum + 1;
            }
            else
            {
                rightNum = myNum + 1 - slots;
            }

            upObject = GameObject.Find(upNum.ToString());
            downObject = GameObject.Find(downNum.ToString());
            leftObject = GameObject.Find(leftNum.ToString());
            rightObject = GameObject.Find(rightNum.ToString());
            //Debug.Log(myNum + " " + upNum + " " + downNum + " " + leftNum + " " + rightNum);
        }

        /// <summary>
        /// object가 highlight활성화 상태일때의 가상함수
        /// </summary>
        protected virtual void PointerOver(bool fIn) { }

        /// <summary>
        /// object를 선택했을 때
        /// </summary>
        protected virtual void PointerCheck(bool fIn) { }

        /// <summary>
        /// 해당 slot에 대한 tooltip이나 설명을 보낼 때 쓰는 함수
        /// </summary>
        protected virtual void SendToolTip() { }

        /// <summary>
        /// item이 add되어 item이 차 있을 때
        /// </summary>
        protected void PointerFill(bool fIn)
        {
            fillIn = true;
            GetComponent<Image>().sprite = FillTexture;
        }

        /// <summary>
        /// object를 비활성화할 때
        /// </summary>
        protected void PointerExit(bool fIn)
        {
            if (fIn)
            {
                GetComponent<Image>().sprite = FillTexture;
            }
            else
            {
                GetComponent<Image>().sprite = slotEmpty;
                fillIn = false;
            }

            if ((InputManager.MainVertical() == 0.0f) && (InputManager.MainHorizontal() == 0.0f))
            {
                buttonOn = false;
            }
        }

        /// <summary>
        /// 다른 slot에서 이동해서 이 object가 활성화될 때 SendMessage로 활성화
        /// 이 cs안에서만 사용.
        /// </summary>
        public void CallButtonOn()
        {
            soundObj = GameObject.Find("ButtonSound");
            soundObj.SendMessage("ButtonEffectPlay", 1);
            obs = slotState.highlight;
            SendToolTip();
        }

    }
}
