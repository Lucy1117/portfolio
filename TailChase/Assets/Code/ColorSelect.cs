using UnityEngine;
using System.Collections;



public class ColorSelect : MonoBehaviour {
	private bool isAvailable = true;
	public int buttonNum = 0;
	public bool pause = true;
	private GameObject player;
	private bool once = true;

	public Texture RedButton;
	public Texture OrangeButton;
	public Texture YellowButton;
	public Texture GreenButton;
	public Texture BlueButton;
	public Texture SkyButton;
	public Texture PurpleButton;

	public GUISkin RedB;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(pause){
			Time.timeScale = 0;
		}
		else{
			Time.timeScale = 1;
			if(once){
				player.SendMessage("TagName");
				once=false;
				GetComponent<AudioSource>().Stop();
			}
		}

	}

	void OnGUI(){

		if(isAvailable){
			GUI.skin = RedB;

			if(GUI.Button (new Rect (100, 50, 100, 60), RedButton)){
				buttonNum = 1;
				isAvailable = false;
				pause = false;
			}
			if(GUI.Button (new Rect (190, 50, 100, 60), OrangeButton)){
				buttonNum = 2;
				isAvailable = false;
				pause = false;
			}
			if(GUI.Button (new Rect (280, 50, 100, 60), YellowButton)){
				buttonNum = 3;
				isAvailable = false;
				pause = false;
			}
			if(GUI.Button (new Rect (370, 50, 100, 60), GreenButton)){
				buttonNum = 4;
				isAvailable = false;
				pause = false;
			}
			if(GUI.Button (new Rect (460, 50, 100, 60), BlueButton)){
				buttonNum = 5;
				isAvailable = false;
				pause = false;
			}
			if(GUI.Button (new Rect (550, 50, 100, 60), SkyButton)){
				buttonNum = 6;
				isAvailable = false;
				pause = false;
			}
			if(GUI.Button (new Rect (640, 50, 100, 60), PurpleButton)){
				buttonNum = 7;
				isAvailable = false;
				pause = false;
			}
		}

	}
}
