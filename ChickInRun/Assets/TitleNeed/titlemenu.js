#pragma strict
private var menuCheck : boolean;
private var menuappear : boolean;


public var start: GUISkin;
public var exit: GUISkin;

function Start(){
}

function Update () {
	if(Input.GetButtonDown("Jump")){
		menuCheck = true;			
	}
	if(menuCheck){
		Waitmenu();
		menuCheck = false;
	}
	
}

function Waitmenu(){
	yield WaitForSeconds(5.0f);
	menuappear = true;
	
}

function OnGUI () {

	var sw : int = Screen.width;
	var sh : int = Screen.height;

	if(menuappear){
    	// Make a background box
    	/* 지영언니코드,
		GUI.Box (Rect (sw/2, 2*sh/3,100,90), "메뉴");

    	// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if (GUI.Button (Rect ((sw/2)+20,(2*sh/3)+40,80,20), "시작하기")) {
			Application.LoadLevel("Main");
		}

		// Make the second button.
		if (GUI.Button (Rect ((sw/2)+20,(2*sh/3)+70,80,20), "끝내기")) {
			Application.Quit();
		}
		*/
		
		GUI.skin=start;
		if(GUI.Button(new Rect(sw/3+50,sh*2/3-10,240,110),"button")==true){
			Application.LoadLevel("Main");
		}
		
		GUI.skin=exit;
		if(GUI.Button(new Rect(sw/3+50,sh*2/3+90,240,110),"button")==true){
			Application.Quit();
		}
    }
}

