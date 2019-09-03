﻿using UnityEngine;
using System.Collections;

public class TeleportPad_fwwf : MonoBehaviour {
	
	
	public int code;
	float disableTimer=0;
	
	//public tele;
	
	void Update () {
		if (disableTimer > 0)
			disableTimer -= Time.deltaTime;
	}
	
	
	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.name == "Character_prefab" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Blue_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Green_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Orange_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Purple_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Red_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Sky_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Yellow_prefab(Clone)" && disableTimer <= 0 ) {
			

			foreach (TeleportPad_fwwf tp in FindObjectsOfType<TeleportPad_fwwf>()) {
				if (tp.code == code && tp != this) {
					tp.disableTimer = 2;
					Vector3 position = tp.gameObject.transform.position;
					position.x += 2;

					collider.gameObject.transform.position = position;


					}
					
					if(collider.gameObject.name == "AI_Blue_prefab(Clone)"){
						GameObject.FindWithTag("Blue").SendMessage("fw");
					}
					if(collider.gameObject.name == "AI_Green_prefab(Clone)"){
						GameObject.FindWithTag("Green").SendMessage("fw");
					}
					if(collider.gameObject.name == "AI_Orange_prefab(Clone)"){
						GameObject.FindWithTag("Orange").SendMessage("fw");
					}
					if(collider.gameObject.name == "AI_Purple_prefab(Clone)"){
						GameObject.FindWithTag("Purple").SendMessage("fw");
					}
					if(collider.gameObject.name == "AI_Red_prefab(Clone)"){
						GameObject.FindWithTag("Red").SendMessage("fw");
					}
					if(collider.gameObject.name == "AI_Sky_prefab(Clone)"){
						GameObject.FindWithTag("SkyBlue").SendMessage("fw");
					}
					if(collider.gameObject.name == "AI_Yellow_prefab(Clone)"){
						GameObject.FindWithTag("Yellow").SendMessage("fw");
					}
			}
					
				}
			}
		
	

	// Use this for initialization
	void Start () {
		
	}
	
	
	
	// Update is called once per frame
	
}

