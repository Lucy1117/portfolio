using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JM.MyProject.MyGame
{
    /// <summary>
    /// UseSlot에 적용되는 코드.
    /// 현재 활성화되고 있는 아이템을 가시적으로 보여주는 역할을 한다.
    /// </summary>
    public class SlotUseItem : MonoBehaviour
    {
        private Sprite EmptyTexture;
        private Sprite FillTexture;

        private static CanvasGroup canvasGroup;

        public float fadeTime;

        private void Awake()
        {
            canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        }
        // Use this for initialization
        void Start()
        {
            EmptyTexture = GetComponent<Image>().sprite;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Slot.cs에서 호출
        /// </summary>
        /// <param name="item"></param>
        public void CurrentUseItem(Item item)
        {
            FillTexture = item.spriteOriginal;
            Debug.Log("아이템이 있음");
            GameObject.Find("UseItemGroup").SendMessage("HandleItemActivate", item.type);
            GetComponent<Image>().sprite = FillTexture;
        }

        /// <summary>
        /// Slot.cs에서 호출
        /// </summary>
        public void CurrentEmptyItem()
        {
            GetComponent<Image>().sprite = EmptyTexture;
            Debug.Log("비어있음");
            GameObject.Find("UseItemGroup").SendMessage("HandleItemUnActivate");
        }

        /// <summary>
        /// 활성화해도 좋은지
        /// </summary>
        /// <param name="guiStateNum"></param>
        public void ImActivate(int guiStateNum)
        {
            if (guiStateNum == 1)
            {
                canvasGroup.alpha = 0;
            }
            else if (guiStateNum >= 2)
            {
                canvasGroup.alpha = 1;
            }
        }
    }
}
