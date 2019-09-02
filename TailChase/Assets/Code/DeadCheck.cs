using UnityEngine;
using System.Collections;

public class DeadCheck : MonoBehaviour {

	//private bool groundCollisionCheck;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Player"){
			Debug.Log ("충돌했다!!!!");
			col.gameObject.SendMessage("DeadOK");
		}
	}

	void OnCollisionStay(Collision col){
		if(col.gameObject.tag == "Player"){
			Debug.Log ("충돌하는 중이다!!!!");
		}
		
	}
	/*
	void onCollisionExit(Collision col){
		if(col.gameObject.tag == "Player"){
			groundCollisionCheck = false;
			Debug.Log ("빠져나왔다!!!!!");
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			groundCollisionCheck = true;
			Debug.Log ("트리거다!!!!!");
		}
		
	}
	
	void onTriggerExit(Collider col){
		if(col.gameObject.tag == "Player"){
			groundCollisionCheck = false;
			Debug.Log ("트리거 나왔다!!!!");
		}
	}
	*/

}