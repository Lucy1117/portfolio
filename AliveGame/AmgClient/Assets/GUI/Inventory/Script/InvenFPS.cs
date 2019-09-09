using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class InvenFPS : MonoBehaviour
    {
       
        private Inventory inventory;

        private GameObject parentItem;

        /// <summary>
        /// object 앞에 활성화되는 tip
        /// </summary>
        public GameObject activeTip;

        /// <summary>
        /// 이 아이템이 storyInformObj를 가지고 있으면 true. 없으면 false
        /// </summary>
        public bool isStory;
        
        // Use this for initialization
        void Start()
        {
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            parentItem = this.transform.parent.gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                activeTip.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (InputManager.XButton())
                {
                    if (GameObject.Find("Initiate").GetComponent<GUIStopCheck>().whichGUIOn == 8)
                    {
                        inventory.AddItem(parentItem.GetComponent<Item>());
                        //Destroy(parentItem);
                        ///destroy를 쓰면 inventory slot의 myitem에 missing값이 들어감.
                        ///그러므로 없애면 안되고, 아래처럼 비활성화를 시켜줘야 함.
                        ///없애는 게 캐시를 줄이는 방법인데 이에 관해선 추후에 다시 생각해 봐야 할 듯
                        if (!isStory)
                        {
                            parentItem.SetActive(false);
                        }
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                activeTip.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }
}
