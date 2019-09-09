using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    public class Slot : InputJoystick2
    {
        private Stack<Item> items;

        public Text stackTxt;

        #region 이후 추가 내용 모두.

        /// <summary>
        /// UseSlot
        /// </summary>
        private GameObject useItemObject;

        /// <summary>
        /// 인벤토리가 열리면 game이 멈추므로, 이를 나타내주는 변수.
        /// </summary>
        private bool gamePlay;

        /// <summary>
        /// SlotUseItem으로 어떤 item이 선택되었는지 sendMessage를 보낼 때 같이 보내는 인자.
        /// </summary>
        public Item myItem;

        private bool slotUseItemCheck = false;

        /// <summary>
        /// 이 변수가 true일 때만 UseSlot으로 메세지 전송
        /// </summary>
        private bool slotUseItemOnce = false;

        private bool slotUsegamePlay = false;

        #endregion

        public Stack<Item> Items
        {
            get { return items; }
            set { items = value; }
        }

        public bool IsEmpty
        {
            get { return items.Count == 0; }
        }

        public bool IsAvailable
        {
            get { return CurrentItem.maxSize > items.Count; }
        }

        public Item CurrentItem
        {
            get { return items.Peek(); }
        }

        void Awake()
        {
            //아이템 stack Instantiate
            items = new Stack<Item>();
        }

        // Use this for initialization
        void Start()
        {

            RectTransform slotRect = GetComponent<RectTransform>();
            RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

            int txtScleFactor = (int)(slotRect.sizeDelta.x * 0.6);
            stackTxt.resizeTextMaxSize = txtScleFactor;
            stackTxt.resizeTextMinSize = txtScleFactor;

            txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
            txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);

            stackTxt.color = Color.red;

            //move관련
            myObject = this.gameObject;
            myNum = int.Parse(myObject.name);

            //스크립트의 순서가 sendMessage를 받고 start()함수를 실행하는 듯. 그러므로 InserNum()함수는 여기서 실행.
            InsertNum();

            parentObject = GameObject.Find("Inventory");
            useItemObject = GameObject.Find("UseSlot");

            Initiate();
            gamePlay = false;

            if (myNum == 1)
            {
                slotUseItemCheck = true;
                slotUseItemOnce = true;
            }
            else
            {
                slotUseItemCheck = false;
                slotUseItemOnce = false;
            }
            slotUsegamePlay = true;

            soundObj = GameObject.Find("ButtonSound");
        }

        /// <summary>
        /// 인벤토리 버튼을 누를때마다 모든 slot을 초기화.
        /// </summary>
        public void Initiate()
        {
            buttonOn = false;
            gamePlay = true;

            if (myNum == 1)
            {
                obs = slotState.highlight;
                buttonOn = true;
            }
            else
            {
                obs = slotState.empty;
                buttonOn = false;
            }

        }


        // Update is called once per frame
        void Update()
        {
            if ((InputManager.MainVertical() == 0.0f) && (InputManager.MainHorizontal() == 0.0f))
            {
                buttonOn = true;
            }

            if (buttonOn)
            {
                switch (obs)
                {
                    case slotState.empty:
                        PointerExit(fillIn);
                        break;
                    case slotState.highlight:
                        PointerOver(fillIn);
                        break;
                    case slotState.check:
                        PointerCheck(fillIn);
                        break;
                    case slotState.fillItem:
                        PointerFill(fillIn);
                        PointerExit(fillIn);
                        break;
                    case slotState.fillHighlight:
                        PointerOver(fillIn);
                        break;
                    case slotState.fillCheck:
                        PointerCheck(fillIn);
                        break;
                }
            }
            if (slotUseItemOnce)
            {
                if (InputManager.ItemSelectTrigger() == 0.0f)
                {
                    slotUseItemCheck = true;
                    slotUseItemOnce = false;
                    if (fillIn)
                    {
                        useItemObject.SendMessage("CurrentUseItem", myItem);
                    }
                    else
                    {
                        useItemObject.SendMessage("CurrentEmptyItem");
                    }
                }
            }
            if (slotUsegamePlay)
            {
                if (slotUseItemCheck)
                {
                    if (InputManager.ItemSelectTrigger() == -1.0f)
                    {
                        slotUseItemCheck = false;
                        leftObject.SendMessage("UseSlotSend");
                    }
                    else if (InputManager.ItemSelectTrigger() == 1.0f)
                    {
                        slotUseItemCheck = false;
                        rightObject.SendMessage("UseSlotSend");
                    }
                }
            }
        }

        #region 포인터 위치에 따라
        /// <summary>
        /// object가 highlight활성화 상태일때
        /// </summary>
        protected override void PointerOver(bool fIn)
        {
            if (fIn)
            {
                GetComponent<Image>().sprite = HighlightTexture;
                SendToolTip();
            }
            else
            {
                GetComponent<Image>().sprite = slotHighlight;
            }

            if (gamePlay)
            {
                //up & down
                if (InputManager.MainVertical() == 1.0f)
                {
                    upObject.SendMessage("CallButtonOn");
                    obs = slotState.empty;
                    buttonOn = false;
                    //parentObject.SendMessage("HideToolTip");
                }
                else if (InputManager.MainVertical() == -1.0f)
                {
                    downObject.SendMessage("CallButtonOn");
                    obs = slotState.empty;
                    buttonOn = false;
                    //parentObject.SendMessage("HideToolTip");
                }

                //left & right
                if (InputManager.MainHorizontal() == 1.0f)
                {
                    rightObject.SendMessage("CallButtonOn");
                    obs = slotState.empty;
                    buttonOn = false;
                    //parentObject.SendMessage("HideToolTip");
                }
                else if (InputManager.MainHorizontal() == -1.0f)
                {
                    leftObject.SendMessage("CallButtonOn");
                    obs = slotState.empty;
                    buttonOn = false;
                    //parentObject.SendMessage("HideToolTip");
                }

                if (InputManager.AButton())
                {
                    soundObj.SendMessage("ButtonEffectPlay", 2);
                    parentObject.BroadcastMessage("UseSlotCheckFun");
                    if (fIn)
                    {
                        obs = slotState.fillCheck;
                        parentObject.SendMessage("ADown");
                    }
                    else
                    {
                        obs = slotState.check;
                        parentObject.SendMessage("ADown");
                    }
                    UseSlotSend();
                }

                if (myItem != null)
                {
                    if (InputManager.XButton())
                    {
                        if (myItem.type == ItemType.Phone)
                        {
                            //핸드폰의 경우
                            parentObject.SendMessage("ShowPhoneMessage");

                        }
                        if (myItem.type == ItemType.Map)
                        {
                            parentObject.SendMessage("ShowMapImage");
                            Debug.Log("Dd");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// object를 선택했을 때
        /// </summary>
        protected override void PointerCheck(bool fIn)
        {
            if (fIn)
            {
                GetComponent<Image>().sprite = CheckTexture;
            }
            else
            {
                GetComponent<Image>().sprite = slotCheck;
            }
        }

        protected override void SendToolTip()
        {
            parentObject.SendMessage("ShowToolTip", myObject);
        }

        /// <summary>
        /// Slot에 아이템이 있을 경우, SlotUseItem으로 SendMessage를 보냄
        /// fillIn이 true이면 아이템이 있는 경우.
        /// </summary>
        public void UseSlotSend()
        {
            parentObject.BroadcastMessage("UseSlotCheckFun");
            if (fillIn)
            {
                useItemObject.SendMessage("CurrentUseItem", myItem);
            }
            else
            {
                UseNotSlotSend();
            }
            slotUseItemOnce = true;
        }

        /// <summary>
        /// Slot에 아이템이 없을 경우, SlotUseItem으로 SendMessage를 보냄
        /// fillIn이 false 아이템이 없는 경우.
        /// </summary>
        public void UseNotSlotSend()
        {
            parentObject.BroadcastMessage("UseSlotCheckFun");
            if (fillIn)
            {
                UseSlotSend();
            }
            else
            {
                useItemObject.SendMessage("CurrentEmptyItem");
            }
            slotUseItemOnce = true;
        }

        #endregion


        public void AddItem(Item item)
        {
            //아이템 추가.
            items.Push(item);

            if (items.Count > 1)
            {
                stackTxt.text = items.Count.ToString();
            }

            obs = slotState.fillItem;

            FillTexture = item.spriteOriginal;
            HighlightTexture = item.spriteHighlighted;
            CheckTexture = item.spriteChecked;

            myItem = item;

            Debug.Log("아이템 추가" + myItem.itemName);
            fillIn = true;
            if (slotUseItemCheck)
            {
                slotUseItemOnce = true;
            }
        }

        public void AddItems(Stack<Item> items)
        {
            this.items = new Stack<Item>(items);

            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;


            obs = slotState.fillItem;

            FillTexture = CurrentItem.spriteOriginal;
            HighlightTexture = CurrentItem.spriteHighlighted;
            CheckTexture = CurrentItem.spriteChecked;
        }

        private void UseItem()
        {
            ///아이템 사용.
            ///아이템이 일회성인 경우의 코딩에 들어감.
            if (!IsEmpty)
            {
                items.Pop().Use();

                stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

                if (IsEmpty)
                {
                    obs = slotState.empty;
                    Inventory.EmptySlots++;
                }
                if (slotUseItemCheck)
                {
                    fillIn = false;
                    slotUseItemOnce = true;
                }
            }
        }

        public void ClearSlot()
        {
            items.Clear();
            obs = slotState.empty;
            stackTxt.text = string.Empty;
        }

        public Stack<Item> RemoveItems(int amount)
        {
            Stack<Item> tmp = new Stack<Item>();

            for (int i = 0; i < amount; i++)
            {
                tmp.Push(items.Pop());
            }

            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            return tmp;
        }


        public Item RemoveItem()
        {
            Item tmp;

            tmp = items.Pop();

            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            return tmp;
        }

        public void InventoryOff()
        {
            gamePlay = false;
        }

        public void UseSlotCheckFun()
        {
            slotUseItemCheck = false;
        }

        /// <summary>
        /// 활성화해도 좋은지
        /// </summary>
        /// <param name="guiStateNum"></param>
        public void ImActivate(int guiStateNum)
        {
            if (guiStateNum <= 5)
            {
                gamePlay = false;
            }
            else if (guiStateNum == 7)
            {
                gamePlay = true;
            }

            //이 아래는 useslot 체크
            if(guiStateNum <= 7)
            {
                slotUsegamePlay = false;
            }
            else if(guiStateNum == 8)
            {
                slotUsegamePlay = true;
            }
        }
    }
}