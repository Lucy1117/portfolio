using UnityEngine;
using System.Collections;

public class BGMOper : MonoBehaviour {

	private GameObject colorselec;
	private ColorSelect cose;

	private bool once = true;

	private GameObject spfi;
	private GameObject sufi;
	private GameObject aufi;
	private GameObject wifi;


	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().Stop ();
		colorselec = GameObject.Find ("SelectColor");
		cose = colorselec.GetComponent <ColorSelect> ();

		spfi = GameObject.Find ("BGMFieldSpring");
		sufi = GameObject.Find ("BGMFieldSummer");
		aufi = GameObject.Find ("BGMFieldAutumn");
		wifi = GameObject.Find ("BGMFieldWinter");
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.name == "BGMFieldSpring"){
			if(!cose.pause){
				if(once){
					//Debug.Log ("audio play!!!!!");
					GetComponent<AudioSource>().Play();
					once=false;
				}
			}
		}
	
	}

	public void StopBGM(){
		//playerDead = true;
		GetComponent<AudioSource>().Stop ();
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Character_prefab"){
			if(!cose.pause){
				//if(once){
					//Debug.Log ("audio play!!!!!");
					GetComponent<AudioSource>().Play();
					//once=false;
				//}
			}
			if(gameObject.name == "BGMFieldSpring"){
				aufi.SendMessage("StopBGM");
				sufi.SendMessage("StopBGM");
				wifi.SendMessage("StopBGM");
			}
			else if(gameObject.name == "BGMFieldSummer"){
				spfi.SendMessage("StopBGM");
				aufi.SendMessage("StopBGM");
				wifi.SendMessage("StopBGM");
			}
			else if(gameObject.name == "BGMFieldAutumn"){
				spfi.SendMessage("StopBGM");
				sufi.SendMessage("StopBGM");
				wifi.SendMessage("StopBGM");
			}
			else if(gameObject.name == "BGMFieldWinter"){
				spfi.SendMessage("StopBGM");
				sufi.SendMessage("StopBGM");
				aufi.SendMessage("StopBGM");
			}
		}
	}

	void OnTriggerStay(Collider col){

	}
	void onTriggerExit(Collider col){
		if(col.gameObject.name == "Character_prefab"){
			GetComponent<AudioSource>().Stop();
			Debug.Log ("트리거 나왔다!!!!");
			once=true;
		}
	}

	/*
	void OnCollisionEnter(Collision col){
		if(col.gameObject.name == "Character_prefab"){
			Debug.Log ("충돌했다!!!!");
			//audio.Play();
		}
	}
	
	void OnCollisionStay(Collision col){
		if(playFlag){
			if(!playerDead){
				if(col.gameObject.name == "Character_prefab"){
					//audio.Play();
				}
			}
			else{
				audio.Stop();
			}
		}
		
	}

	void onCollisionExit(Collision col){
		if(col.gameObject.name == "Character_prefab"){
			audio.Stop();
			Debug.Log ("빠져나왔다!!!!!");
		}
	}
	*/

}
