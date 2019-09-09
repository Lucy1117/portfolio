using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    public class ItemHandler : MonoBehaviour
    {
        public GameObject phoneObj;
        public GameObject keyObj;
        public GameObject mapObj;
        public GameObject cardKeyObj;
        public GameObject needleObj;
        public GameObject hammerObj;
        public GameObject cctvKeyObj;

        private GameObject[] childObj;
    
        // Use this for initialization
        void Start()
        {
            childObj = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                childObj[i] = transform.GetChild(i).gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public void HandleItemActivate(ItemType item)
        {
            HandleItemUnActivate();

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            switch (item)
            {
                case ItemType.Key:
                    player.GetComponent<FirstPersonController>().itemAnim = false;
                    keyObj.SetActive(true);
                    break;
                case ItemType.Map:
                    player.GetComponent<FirstPersonController>().itemAnim = false;
                    mapObj.SetActive(true);
                    break;
                case ItemType.Phone:
                    player.GetComponent<FirstPersonController>().itemAnim = true;
                    phoneObj.SetActive(true);
                    break;
                case ItemType.CardKey:
                    player.GetComponent<FirstPersonController>().itemAnim = true;
                    cardKeyObj.SetActive(true);
                    break;
                case ItemType.Needle:
                    player.GetComponent<FirstPersonController>().itemAnim = true;
                    needleObj.SetActive(true);
                    break;
                case ItemType.Hammer:
                    player.GetComponent<FirstPersonController>().itemAnim = true;
                    hammerObj.SetActive(true);
                    break;
                case ItemType.CctvKey:
                    player.GetComponent<FirstPersonController>().itemAnim = true;
                    cctvKeyObj.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void HandleItemUnActivate()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            for (int i = 0; i < transform.childCount; i++)
            {
                if (childObj[i].activeSelf)
                {
                    childObj[i].SetActive(false);
                }
            }
            player.GetComponent<FirstPersonController>().itemAnim = false;
        }
    }
}
