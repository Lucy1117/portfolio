using UnityEngine;
using System.Collections;

public class AIGenerator2 : MonoBehaviour {
	
	private float offsx;
	private float offsz;
	private bool once = true;
	private Vector3 creatPosition;
	public GameObject prefabA;
	private bool aFlag = false;
	public float rndRange;
	
	// 다른 코드의 변수 가져오기.
	private GameObject otherObject;
	private ColorSelect other;
	private bool pauseCheck;
	private int deleteNum;
	
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		otherObject = GameObject.FindGameObjectWithTag ("ColorGUI");
		other = otherObject.GetComponent<ColorSelect>();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(once){
			pauseCheck = other.pause;
			if(!pauseCheck){
				deleteNum = other.buttonNum;
				
				if(Name(prefabA)!=deleteNum){
					aFlag =true;
				}
				if(aFlag){
					offsx = Random.Range (-rndRange, rndRange);
					offsz = Random.Range (-rndRange, rndRange);
					creatPosition = transform.position + new Vector3(offsx, 0.0f, offsz);
					Instantiate (prefabA, creatPosition, transform.rotation);
				}
				once = false;
				player.SendMessage("ChaseOK");
			}
		}
	}
	
	//플레이어가 무슨 색을 선택했는지에 따라 생성되는 AI가 다름.
	int Name(GameObject A){
		int num=0;
		if (A.tag == "Red"){
			num = 1;
		} 
		else if (A.tag == "Orange"){
			num = 2;
		} 
		else if (A.tag == "Yellow"){
			num = 3;
		} 
		else if (A.tag == "Green"){
			num = 4;
		} 
		else if (A.tag == "Blue"){
			num = 5;
		} 
		else if (A.tag == "SkyBlue"){
			num = 6;
		} 
		else if (A.tag == "Purple"){
			num = 7;
		}
		return num;
		
	}
}
