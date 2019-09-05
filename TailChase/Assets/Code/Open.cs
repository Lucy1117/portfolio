using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Open : MonoBehaviour {
	public GUISkin Menu;

	public Texture StartB;
	public Texture ExitB;

	public GameObject mainCam;
	private Vector3 campos;

	private int sw = Screen.width;
	private int sh = Screen.height;
	// Use this for initialization
	void Start () {
		GetComponent<Animation>().Play ("Title");
		GetComponent<Animation>() ["Title"].speed = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		campos = mainCam.transform.position;
		transform.position = new Vector3(campos.x - 1.0f, campos.y - 25.0f, campos.z +20.0f);
	}
	

	void OnGUI(){
		GUI.skin = Menu;

        if (GUI.Button(new Rect((sw / 10), (sh / 3), (sw * 2 / 5), (sh * 4 / 5)), StartB)){
            SceneManager.LoadScene("Main");
        }		    
		if(GUI.Button (new Rect ((sw/2),(sh/3),(sw*2/5),(sh*4/5)), ExitB)){
			Application.Quit();
		}
	}


}
