using UnityEngine;
using System.Collections;

public class MenuCam : MonoBehaviour {

	private int rndNum=0;

	// Use this for initialization
	void Start () {
		rndNum = Random.Range (1, 5);
	}
	
	// Update is called once per frame
	void Update () {
		if(rndNum ==1){
			transform.position = new Vector3(-10.0f, 23.0f, -35.0f);
		}
		else if(rndNum==2){
			transform.position = new Vector3(-75.0f, 20.0f, 25.0f);
		}
		else if(rndNum==3){
			transform.position = new Vector3(-178.0f, 128.0f, -38.0f);
		}
		else if(rndNum==4){
			transform.position = new Vector3(102.0f, 95.0f, -22.0f);
		}
	
	}
}
