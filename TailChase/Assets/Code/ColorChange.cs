using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

	public Material colorRed;
	public Material colorOrange;
	public Material colorYellow;
	public Material colorGreen;
	public Material colorBlue;
	public Material colorSky;
	public Material colorPurple;

	private bool once = true;
	private int colorNum = 0;
	private GameObject otherObject;
	private ColorSelect other;

	// Use this for initialization

	void Start () {
		otherObject = GameObject.FindGameObjectWithTag ("ColorGUI");
		other = otherObject.GetComponent<ColorSelect>();
	}
	
	// Update is called once per frame
	void Update () {
		colorNum = other.buttonNum;
		if(colorNum !=0){
			if(once){
				switch(colorNum){
				case 1 :
					GetComponent<Renderer>().material = colorRed;
					break;
				case 2 :
					GetComponent<Renderer>().material = colorOrange;
					break;
				case 3 :
					GetComponent<Renderer>().material = colorYellow;
					break;
				case 4 :
					GetComponent<Renderer>().material = colorGreen;
					break;
				case 5 :
					GetComponent<Renderer>().material = colorBlue;
					break;
				case 6 :
					GetComponent<Renderer>().material = colorSky;
					break;
				case 7 :
					GetComponent<Renderer>().material = colorPurple;
					break;
				}
				once = false;				
			}
		}
	
	}
}
