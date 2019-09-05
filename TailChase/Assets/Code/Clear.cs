﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour {
	
	public GUISkin ClearMe;
	public Texture RetryBu;
	public Texture ExitBu;
	public Texture ClearTex;
	public bool look = false;
	private int sw = Screen.width;
	private int sh = Screen.height;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ClearMenu(){
		look = true;
	}
	
	void OnGUI(){
		if (look) {
			GUI.skin = ClearMe;
			GUI.Button(new Rect((sw/3), (sh/4), (sw/3), (sh/2-100)), ClearTex);
			if(GUI.Button(new Rect ((sw/4+80),((sh/2)+85), (sw/5), (sh/5)), RetryBu)){
                SceneManager.LoadScene("Main");
            }
			if(GUI.Button(new Rect ((sw/2),((sh/2)+85),(sw/5), (sh/5)), ExitBu)){
				Application.Quit ();
			}
		}
	}
}