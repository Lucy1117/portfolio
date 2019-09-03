#pragma strict

//GUI에 필요한 변수 
private var menuappear : boolean;

public var again: GUISkin;
public var d_exit: GUISkin;

public var myTexture : Texture;

function Start () {
	Waitmenu();
	//myTexture = Resources.Load("clear02");
}

function Update () {
	
}

//여기부터 GUI
function Waitmenu(){
	yield WaitForSeconds(2.0f);
	menuappear = true;
	
}

function OnGUI () {
	var sw : int = Screen.width;
	var sh : int = Screen.height;

	if(menuappear){
    	// Make a background box
    	/*
		GUI.Box (Rect (sw/2, 2*sh/3,100,90), "메뉴");

    	// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if (GUI.Button (Rect ((sw/2)+20,(2*sh/3)+40,80,20), "다시하기")) {
			Application.LoadLevel("Main");
		}

		// Make the second button.
		if (GUI.Button (Rect ((sw/2)+20,(2*sh/3)+70,80,20), "끝내기")) {
			Application.Quit();
		}
		*/
		GUI.DrawTexture(Rect(sw/3-20,sh/10, 365, 318),myTexture);
		
		GUI.skin=again;
		if(GUI.Button(new Rect(sw/3+30,sh*2/3-10,100,140),"button")==true){
			Application.LoadLevel("Main");
		}
		
		GUI.skin=d_exit;
		if(GUI.Button(new Rect(sw/3+170,sh*2/3-10,100,140),"button")==true){
			Application.Quit();
		}
    }
}

