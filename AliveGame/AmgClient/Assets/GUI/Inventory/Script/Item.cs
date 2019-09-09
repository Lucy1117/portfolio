using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace JM.MyProject.MyGame
{
	public enum ItemType
	{
		Key,
		Map,
		Phone,
		CardKey,
		Needle,
		Hammer,
		CctvKey
	};

	/// <summary>
	/// 필요 없는 것.
	/// </summary>
	public enum Quality
	{
		Common,
		Uncommon,
	};

	public class Item : MonoBehaviour
	{

		public ItemType type;

		//public Sprite spriteNeutral;


		// public Sprite spriteHighlighted;
		public Sprite spriteOriginal;
		public Sprite spriteHighlighted;
		public Sprite spriteChecked;

		public int maxSize;

		//아이템 설명
		public string itemName;
		public string description;

		/// <summary>
		/// 필드에 있는 아이템이 아닌, 손에 들려있는 아이템만 AButton활성화 되도록 분기를 둠.
		/// </summary>
		public bool inHand;

		/// <summary>
		/// GUI가 아무것도 없으면 true, 있으면 false
		/// GUIStopCheck.cs의 whichGUIOn가 8이면 true
		/// </summary>
		private bool inactive;


		private void Update()
		{
			if (InputManager.AButton())
			{
				UseOK();

				if (inactive && inHand)
				{
					Use();
				}
			}
		}

		/// <summary>
		/// 아이템을 사용할 수 있는지. 현재 활성화된 GUI가 있으면 사용할 수 없음.
		/// AButton을 누를 때의 분기를 나누는 걸 돕는 함수.
		/// </summary>
		private void UseOK()
		{
			GameObject initObj = GameObject.Find("Initiate");
			if(initObj.GetComponent<GUIStopCheck>().whichGUIOn == 8)
			{
				inactive = true;
			}
			else
			{
				if(GameObject.Find("MapImageActive").GetComponent<MapPamphlet>().guiStat == GUIState.on)
				{
					inactive = true;
				}
				else
				{
					inactive = false;
				}
			}
		}

		/// <summary>
		/// 아이템을 사용하면 실행되는 함수
		/// </summary>
		public void Use()
		{
			switch (type)
			{
			//3층 창고 열쇠
			case ItemType.Key:
				GameObject.Find("WarehouseKeyTrigger").SendMessage("ItemUse");
				Debug.Log("열쇠 사용");
				break;
				//팜플렛의 지도
			case ItemType.Map:
				Debug.Log("지도 사용");
				GameObject.Find("MapImageActive").SendMessage("ItemUse");
				///지도가 팝업으로 뜨게
				break;
			case ItemType.Phone:
				Debug.Log("핸드폰 사용");
				Debug.Log(this.transform.GetChild(2).name);
				//Phone prefab안의 child(flash)순서가 바뀌면 에러남. 현재 flash는 2번인자(3번째 위치)에 있음.
				Transform childObjA = this.transform.GetChild(2);
				Transform childObjB = this.transform.GetChild(3);
				childObjA.gameObject.GetComponent<Light>().enabled = !childObjA.gameObject.GetComponent<Light>().enabled;
				childObjB.gameObject.GetComponent<Light>().enabled = !childObjB.gameObject.GetComponent<Light>().enabled;
				break;
			case ItemType.CardKey:
				Debug.Log("카드키 사용");
				break;
			case ItemType.Needle:
				Debug.Log("초침 사용");
				break;
			case ItemType.Hammer:
				GameObject.Find("PlasterFigureTrigger").SendMessage("ItemUse");
				Debug.Log("망치 사용");
				break;
			case ItemType.CctvKey:
				GameObject.Find("CCTVRoomKeyTrigger").SendMessage("ItemUse");
				Debug.Log("cctv실 열쇠 사용");
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// item.cs
		/// 툴팁에 적혀야 할 것들을 return값으로 보냄.
		/// </summary>
		/// <returns>string</returns>
		public string GetToolTip()
		{
			string newLine = string.Empty;

			if (description != string.Empty)
			{
				newLine = "\n";
			}

			//첫번째 인자는 이름, 두번째 인자는 설명. 태그를 써서 각각 다른 폰트로 표현 {0} {1}로 인자 번호를 맞춤.
			return string.Format("<size=16>{0}</size>" + newLine + "<size=14><i>{1}</i></size>", itemName, description);
		}
	}
}