using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// background에 적용
    /// </summary>
    public class EndingCollection : MonoBehaviour
    {
        #region EndingCollection 변수
        private RectTransform collectionRect;
        private float collectionWidth, collectionHeight;

        private RectTransform canvasRectTransformm;

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
        
        public static int EmptySlots
        {
            get { return emptySlots; }
            set { emptySlots = value; }
        }

        private int collectionNum = 1;

        #endregion

        #region EndingScene 변수
        //private RectTransform sceneRect;
        //private float sceneWidth, sceneHeight;

        public float scenePaddingLeft, scenePaddingTop;

        public GameObject scenePrefab;

        /// <summary>
        /// 모든 슬롯 리스트의 instantiate
        /// </summary>

        private int sceneNum = 1;

        #endregion


        private GameObject myObject;
        private bool ecOn = false;

        // Use this for initialization
        void Start()
        {
            myObject = this.gameObject;
            canvasRectTransformm = gameObject.GetComponent<RectTransform>();
            if (ecOn)
            {
                CreateLayout();
                myObject.BroadcastMessage("RowsNum", rows);
                myObject.BroadcastMessage("SlotsNum", slots);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Button_EndingCollect이 눌릴 때 함수 호출.
        /// </summary>
        public void ecOnFun()
        {
            if (ecOn)
            {
                myObject.BroadcastMessage("Initiate");
            }
            ecOn = true;
            //Debug.Log(ecOn);
        }

        /// <summary>
        /// EndingCollectionSlot을 각각 생성하는 함수
        /// </summary>
        private void CreateLayout()
        {
            collectionWidth = canvasRectTransformm.rect.width;
            collectionHeight = canvasRectTransformm.rect.height;

            collectionRect = GetComponent<RectTransform>();
            
            collectionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, collectionWidth + slotPaddingLeft);
            collectionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, collectionHeight + slotPaddingTop);

            allSlots = new List<GameObject>();
            emptySlots = slots;

            int columns = slots / rows;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                    newSlot.name = collectionNum.ToString();

                    newSlot.transform.SetParent(this.transform.parent);

                    slotRect.localPosition = collectionRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    newSlot.transform.SetParent(this.transform);

                    allSlots.Add(newSlot);
                    slotRect.localScale = new Vector3(1.0f,1.0f,1.0f);

                    collectionNum++;
                }
            }
        }


        /// <summary>
        /// EndingCollectionScene을 각각 생성하는 함수
        /// </summary>
        private void CreateScene(int sNum)
        {
            GameObject newSlot = (GameObject)Instantiate(scenePrefab);

            RectTransform slotRect = newSlot.GetComponent<RectTransform>();

            newSlot.name = "Scene" + sceneNum.ToString();

            newSlot.transform.SetParent(this.transform.parent);

            slotRect.localPosition = collectionRect.localPosition + new Vector3(slotRect.rect.height/2, -slotRect.rect.width/4);

            newSlot.transform.SetParent(this.transform);

            slotRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            newSlot.SendMessage("SceneNumber", sNum);
        }
    }
}