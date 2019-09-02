using UnityEngine;
using System.Collections;

public class BGMDeadAfter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (DeadAfter());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator DeadAfter(){
		yield return new WaitForSeconds(5.0f);
		GetComponent<AudioSource>().Play ();
	}
}
