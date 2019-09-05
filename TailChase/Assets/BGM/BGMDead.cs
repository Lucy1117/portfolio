using UnityEngine;
using System.Collections;

public class BGMDead : MonoBehaviour {
		
	public GameObject prefab;

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().Stop ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DeadBGM(){
		GameObject.Find("BGMFieldSpring").SendMessage("StopBGM");
		GameObject.Find("BGMFieldSummer").SendMessage("StopBGM");
		GameObject.Find("BGMFieldAutumn").SendMessage("StopBGM");
		GameObject.Find("BGMFieldWinter").SendMessage("StopBGM");
		GetComponent<AudioSource>().Play ();
		Instantiate (prefab, transform.position, transform.rotation);
	}
}
