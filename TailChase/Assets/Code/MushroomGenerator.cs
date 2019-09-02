using UnityEngine;
using System.Collections;

public class MushroomGenerator : MonoBehaviour {
	
	public float interval;
	public GameObject redMushroom;
	public GameObject orangeMushroom;
	public GameObject yellowMushroom;
	public GameObject greenMushroom;
	public GameObject blueMushroom;
	public GameObject skyMushroom;
	public GameObject purpleMushroom;
	
	public int red;
	public int orange;
	public int yellow;
	public int green;
	public int blue;
	public int sky;
	public int purple;
	
	//밖에서 지정해주는 변수.
	public float rndRange;
	public float mushNum;
	public int fieldNum;
	
	private float timer;
	private bool timerFlag = true;
	
	private bool waitTime = false;
	
	private bool redOk = true;
	private bool orangeOk = true;
	private bool yellowOk = true;
	private bool greenOk = true;
	private bool blueOk = true;
	private bool skyOk = true;
	private bool purpleOk = true;
	
	// Use this for initialization
	void Start () {
		interval = 2;
		red = 0;
		orange = 0;
		yellow = 0;
		green = 0;
		blue = 0;
		sky = 0;
		purple = 0;
		
		StartCoroutine (WaitMoment ());
	}
	
	// Update is called once per frame
	void Update () {
		
		if(waitTime){
			if(!GameObject.FindGameObjectWithTag ("Red"))
				redOk = false;
			if(!GameObject.FindGameObjectWithTag ("Orange"))
				orangeOk = false;
			if(!GameObject.FindGameObjectWithTag ("Yellow"))
				yellowOk = false;
			if(!GameObject.FindGameObjectWithTag ("Green"))
				greenOk = false;
			if(!GameObject.FindGameObjectWithTag ("Blue"))
				blueOk = false;
			if(!GameObject.FindGameObjectWithTag ("SkyBlue"))
				skyOk = false;
			if(!GameObject.FindGameObjectWithTag ("Purple"))
				purpleOk = false;
			
			if(timerFlag){
				timer -=Time.deltaTime;
			}
			
			if(timer<0.0f){
				if(redOk){
					if (red < mushNum) {
						createMush (redMushroom);
						red += 1;
					}
				}
				if(orangeOk){
					if (orange < mushNum) {
						createMush (orangeMushroom);
						orange += 1;
						
					}
				}
				if(yellowOk){
					if (yellow < mushNum) {
						createMush (yellowMushroom);
						yellow += 1;
						
					}
				}
				if(greenOk){
					if (green < mushNum) {
						createMush (greenMushroom);
						green += 1;
					}
				}
				if(blueOk){
					if (blue < mushNum) {
						createMush (blueMushroom);
						blue += 1;
					}
				}
				if(skyOk){
					if (sky < mushNum) {
						createMush (skyMushroom);
						sky += 1;
					}
				}
				if(purpleOk){
					if (purple < mushNum) {
						createMush (purpleMushroom);
						purple += 1;
					}
					
				}
				timer = interval;
				
				//timerStop();
			}
		}
	}
	
	private void createMush(GameObject mushName){
		float offsx = Random.Range (-rndRange, rndRange);
		float offsz = Random.Range (-rndRange, rndRange);
		Vector3 position = transform.position + new Vector3 (offsx, 0, offsz);
		Instantiate (mushName, position, transform.rotation);
	}
	
	private void timerStop(){
		if (red == mushNum && orange == mushNum && yellow == mushNum && green == mushNum 
		    && blue == mushNum && sky == mushNum && purple == mushNum) {
			timerFlag = false;
		}
		else{
			timerFlag = true;
		}
	}
	
	
	IEnumerator WaitMoment(){
		
		yield return new WaitForSeconds(2.0f);
		//Debug.Log ("코루틴 확인이여");
		waitTime = true;
		
	}
	
	
	void redmu(){
		timerFlag = true;
		timer = interval;
		red = red - 1;
	}
	
	void orangemu(){
		timerFlag = true;
		timer = interval;
		orange -= 1;
	}
	
	void yellowmu(){
		timerFlag = true;
		timer = interval;
		yellow -= 1;
	}
	
	void greenmu(){
		timerFlag = true;
		timer = interval;
		green -= 1;
	}
	
	void bluemu(){
		timerFlag = true;
		timer = interval;
		blue -= 1;
	}
	
	void skymu(){
		timerFlag = true;
		timer = interval;
		sky -= 1;	
	}
	
	void purplemu(){
		timerFlag = true;
		timer = interval;
		purple -= 1;
	}
}
