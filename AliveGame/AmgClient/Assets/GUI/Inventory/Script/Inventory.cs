using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace JM.MyProject.MyGame
{
    public class Inventory : MonoBehaviour
    {

        private RectTransform inventoryRect;

        private float inventoryWidth, inventoryHeight;

        /// <summary>
        /// 생성할 슬롯의 수.
        /// </summary>
        public int slots;

        /// <summary>
        /// 생성할 슬롯의 열.
        /// </summary>
        public int rows;

        public float slotPaddingLeft, slotPaddingTop;

        public float slotSize;

        public GameObject slotPrefab;

        /// <summary>
        /// 모든 슬롯 리스트의 instantiate
        /// </summary>
        private List<GameObject> allSlots;

        private static int emptySlots;

        public Canvas canvas;
        
        private static CanvasGroup canvasGroup;

        /// <summary>
        /// 인벤토리가 천천히 떴다가 천천히 사라지도록
        /// </summary>
        private bool fadingIn;
        private bool fadingOut;
        public float fadeTime;

        private static GameObject clicked;
        
        private int splitAmount;

        private int maxStackCount;

        #region 사용하는 아이템 목록
        public GameObject map;
        public GameObject key;
        public GameObject phone;
        public GameObject cardKey;
        public GameObject needle;
        public GameObject hammer;
        public GameObject cctvKey;
        #endregion


        private static Inventory instance;
        
        /// <summary>
        /// 하단에 적힐 설명
        /// </summary>
        public GameObject tooltipObject;
        private static GameObject tooltip;
        public GameObject tooltiptextObject;
        private static GameObject tooltiptext;
        
        public Text visualTextObject;
        private static Text visualText;

        public static CanvasGroup CanvasGroup
        {
            get { return Inventory.canvasGroup; }
        }

        public static Inventory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<Inventory>();
                }
                return Inventory.instance;
            }
        }


        public static int EmptySlots
        {
            get { return emptySlots; }
            set { emptySlots = value; }
        }


        #region 0719

        private int slotNum = 1;
        private GameObject myObject;

        public GameObject player;

        #endregion

        #region 0812

        public GUIState guiStat = GUIState.off;

        /// <summary>
        /// 인벤토리가 열리면 game이 멈추므로, 이를 나타내주는 변수.
        /// </summary>
        private bool gamePlay;

        /// <summary>
        /// 핸드폰메세지 이미지 object
        /// </summary>
        public GameObject phoneMessgeObj;

        /// <summary>
        /// 맵 이미지 object
        /// </summary>
        public GameObject mapImageObj;

        private GameObject initObj;

        #endregion

        #region Save관련

        public string itemDetailContent;
        #endregion

        private void Awake()
        {
            //static변수 = public변수
            tooltiptext = tooltiptextObject;
            tooltip = tooltipObject;
            visualText = visualTextObject;
            canvasGroup = transform.parent.GetComponent<CanvasGroup>();
            CreateLayout();
        }
        // Use this for initialization
        void Start()
        {

            //인벤토리 내 방향키 이동을 위한 broadcase
            myObject = this.gameObject;
            myObject.BroadcastMessage("RowsNum", rows);
            myObject.BroadcastMessage("SlotsNum", slots);
            phoneMessgeObj.GetComponent<CanvasGroup>().alpha = 0;
            mapImageObj.GetComponent<CanvasGroup>().alpha = 0;
            initObj = GameObject.Find("Initiate");

            SaveInventory();
            gamePlay = true;

            //PhoneAdd();
        }

        // Update is called once per frame
        void Update()
        {
            if (gamePlay)
            {
                if (InputManager.YButton())
                {
                    GameObject.Find("ButtonSound").SendMessage("ButtonEffectPlay", 3);
                    //PhoneAdd();
                    //Y버튼을 누르면
                    if (canvasGroup.alpha > 0)
                    {
                        guiStat = GUIState.off;
                        myObject.BroadcastMessage("InventoryOff");
                        StartCoroutine("FadeOut");
                        if (phoneMessgeObj.activeSelf)
                        {
                            phoneMessgeObj.SendMessage("ObjectNotActive");
                        }
                    }
                    else
                    {
                        guiStat = GUIState.on;
                        myObject.BroadcastMessage("Initiate");
                        StartCoroutine("FadeIn");
                    }
                    initObj.SendMessage("GUIOnCheck");
                }
            }
        }

        /// <summary>
        /// slot.cs에서 호출하는 함수
        /// </summary>
        public void ADown()
        {
            guiStat = GUIState.off;
            myObject.BroadcastMessage("InventoryOff");
            StartCoroutine("FadeOut");
            initObj.SendMessage("GUIOnCheck");
            if (phoneMessgeObj.activeSelf)
            {
                phoneMessgeObj.SendMessage("ObjectNotActive");
            }
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
            else if (guiStateNum >= 6)
            {
                gamePlay = true;
            }
        }

        public void SaveInventory()
        {
            itemDetailContent = string.Empty;
            for (int i = 0; i < allSlots.Count; i++)
            {
                Slot tmp = allSlots[i].GetComponent<Slot>();

                if (!tmp.IsEmpty)
                {
                    itemDetailContent += i + "-" + tmp.CurrentItem.type.ToString() + ";";
                }
            }
            Debug.Log(itemDetailContent);
        }

        /// <summary>
        /// StoryImform.cs의 LoadStoryOrder에서 호출
        /// </summary>
        public void LoadInventory()
        {
            itemDetailContent = GameObject.Find("Initiate").GetComponent<InitInterface>().itemDetailStr;

            if(itemDetailContent == null)
            {
                return;
            }
            // [0]"0-MAP; [1]1-KEY;"
            string[] splitContent = itemDetailContent.Split(';'); //"0-MAP"

            for (int x = 0; x < splitContent.Length - 1; x++)
            {
                string[] splitValues = splitContent[x].Split('-');

                int index = Int32.Parse(splitValues[0]); //"0"

                ItemType type = (ItemType)Enum.Parse(typeof(ItemType), splitValues[1]); //"MAP"

                switch (type)
                {
                    case ItemType.Map:
                        allSlots[index].GetComponent<Slot>().AddItem(map.GetComponent<Item>());
                        break;
                    case ItemType.Key:
                        allSlots[index].GetComponent<Slot>().AddItem(key.GetComponent<Item>());
                        break;
                    case ItemType.Phone:
                        allSlots[index].GetComponent<Slot>().AddItem(phone.GetComponent<Item>());
                        break;
                    case ItemType.CardKey:
                        allSlots[index].GetComponent<Slot>().AddItem(cardKey.GetComponent<Item>());
                        break;
                    case ItemType.Needle:
                        allSlots[index].GetComponent<Slot>().AddItem(needle.GetComponent<Item>());
                        break;
                    case ItemType.Hammer:
                        allSlots[index].GetComponent<Slot>().AddItem(hammer.GetComponent<Item>());
                        break;
                    case ItemType.CctvKey:
                        allSlots[index].GetComponent<Slot>().AddItem(cctvKey.GetComponent<Item>());
                        break;
                }
            }
        }

        /// <summary>
        /// 인벤토리를 생성하는 함수
        /// </summary>
        private void CreateLayout()
        {
            //inventory를 새로 생성할 때 allSlots를 초기화.
            if (allSlots != null)
            {
                foreach (GameObject go in allSlots)
                {
                    Destroy(go);
                }
            }

            allSlots = new List<GameObject>();

            emptySlots = slots;

            inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft);

            inventoryHeight = rows * (slotSize + slotPaddingTop);

            inventoryRect = GetComponent<RectTransform>();

            inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth + slotPaddingLeft);
            inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight + slotPaddingTop);

            //인벤토리의 행
            int columns = slots / rows;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {

                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                    newSlot.name = slotNum.ToString();

                    newSlot.transform.SetParent(this.transform.parent);

                    slotRect.localPosition = inventoryRect.localPosition +
                        new Vector3((slotPaddingLeft * (x + 1) + (slotSize * x)), (-slotPaddingTop * (y + 1) - (slotSize * y)));


                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    newSlot.transform.SetParent(this.transform);

                    allSlots.Add(newSlot);

                    slotNum++;

                }
            }

            tooltip.SetActive(true);

            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();

            tooltip.transform.SetParent(this.transform.parent);

            tooltipRect.localPosition = inventoryRect.localPosition +
                new Vector3(0.0f, (-slotPaddingTop * (rows + 1) - (slotSize * rows)));

            //tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * (columns-1) * canvas.scaleFactor + (slotPaddingLeft * (columns+2)));
            //tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * (columns - 1) * canvas.scaleFactor + (slotPaddingLeft * 2.0f * columns + slotPaddingLeft));
            //tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * (columns) * canvas.scaleFactor + (slotPaddingLeft * 3.9f * columns + slotPaddingLeft));
            //tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * (columns) * canvas.scaleFactor + (slotPaddingLeft  * columns));
            tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * (columns) + (slotPaddingLeft * columns));
            tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 1.5f * canvas.scaleFactor);
            tooltip.transform.SetParent(this.transform);

        }

        /// <summary>
        /// 아이템 설명을 보여주는 곳.
        /// </summary>
        /// <param name="slot"></param>
        public void ShowToolTip(GameObject slot)
        {
            Slot tmpslot = slot.GetComponent<Slot>();

            if(!tmpslot.IsEmpty)
            {
                visualText.text = tmpslot.CurrentItem.GetToolTip();
                tooltiptext.SetActive(true);
            }
            else
            {
                visualText.text = null;
                tooltiptext.SetActive(true);
            }

            tooltiptext.SetActive(true);
        }

        public void HideToolTip()
        {
            tooltip.SetActive(false);
            tooltiptext.SetActive(false);
        }


        public bool AddItem(Item item)
        {
            if (item.maxSize == 1)
            {
                PlaceEmpty(item);
                SaveInventory();
                return true;
            }
            else
            {
                foreach (GameObject slot in allSlots)
                {
                    Slot tmp = slot.GetComponent<Slot>();

                    if (!tmp.IsEmpty)
                    {
                        if (tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                        {
                            tmp.AddItem(item);
                            SaveInventory();
                            return true;
                        }
                    }
                }
            }
            if (emptySlots > 0)
            {
                PlaceEmpty(item);
                SaveInventory();
            }

            return false;
        }
        
        private bool PlaceEmpty(Item item)
        {
            if (emptySlots > 0)
            {
                foreach (GameObject slot in allSlots)
                {
                    Slot tmp = slot.GetComponent<Slot>();

                    if (tmp.IsEmpty)
                    {
                        tmp.AddItem(item);
                        emptySlots--;
                        SaveInventory();
                        return true;
                    }
                }
            }
            SaveInventory();
            return false;
        }


        #region 인벤토리 Fade In/Out

        /// <summary>
        /// 인벤토리가 천천히 사라지는 것처럼 보이도록
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
        /// 인벤토리가 천천히 생겨나는 것처럼 보이도록
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

        private void ShowPhoneMessage()
        {
            phoneMessgeObj.SendMessage("ObjectActive");
        }

        private void ShowMapImage()
        {
            mapImageObj.SendMessage("ObjectActive");
        } 

        public void PhoneAdd()
        {
            AddItem(phone.GetComponent<Item>());
        }
    }
}