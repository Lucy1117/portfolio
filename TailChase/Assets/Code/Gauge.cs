using UnityEngine;
using System.Collections;

public class Gauge: MonoBehaviour {
	public GUISkin textButton;
	public GUISkin textBox;
	
	public float posX;
	public float posY;
	public float alt;
	public float lar;
	public float larl;
	
	public float bar;
	public float maxGauge;
	
	public float tempo;
	
	private GameObject eatState;

	private bool once = false;
	
	int a = 0;
	
	// Use this for initialization
	void Start () {
		maxGauge = 0;
		bar = maxGauge;
		eatState = GameObject.Find ("Character_prefab");
	}
	
	// Update is called once per frame
	void Update () {
		
		larl = 200;
		lar = 200 * (bar / maxGauge);
		posX = 0;
		posY = 20;
		alt = 20;
		
		if (a == 1) {
			if (bar > 0.0f) {
				bar = bar - 0.1f;
				tempo = -1;
			}
			else{
				if(once && eatState)
                {
					eatState.SendMessage("EatNO");
					once = false;
				}
			}
			
		}
		
		if(tempo >= 0){
			if(bar < maxGauge){
				if(tempo > 0.01f){
					bar += Time.deltaTime;
					tempo = 0;
				}
			}
		}
		tempo = tempo + Time.deltaTime;
		
	}
	void OnGUI(){
		GUI.skin = textBox;
		GUI.Box (new Rect (posX, posY, larl, alt), " " );
		
		GUI.skin = textButton;
		GUI.Button (new Rect(posX, posY, lar, alt)," ");
	}
	
	void Guagestart(){
		//버섯 먹으면 게이지 차서 닳기 시작
		a = 1; //게이지 닳게 하는 코딩으로 이동
		maxGauge = 100;
		bar = maxGauge;
		once = true;
	}
}
