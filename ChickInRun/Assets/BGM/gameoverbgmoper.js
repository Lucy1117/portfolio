#pragma strict

private var deathBGMA : GameObject;
private var deathBGMB : GameObject;
private var deathBGMC : GameObject;
public var overCheckA : boolean;
public var overCheckB : boolean;
public var overCheckC : boolean;

public var gameoverCheck : boolean;
private var playCheck : boolean;

//GUI에 필요한 변수 
private var menuappear : boolean;

public var again: GUISkin;
public var d_exit: GUISkin;


function Start () {
	deathBGMA = GameObject.FindGameObjectWithTag("Backgroundmusic1");
	deathBGMB = GameObject.FindGameObjectWithTag("Backgroundmusic2");
	deathBGMC = GameObject.FindGameObjectWithTag("Backgroundmusic3");
	
	playCheck = true;
}

function Update () {
	
	overCheckA = deathBGMA.GetComponent(bgmoper).deathCheck;
	overCheckB = deathBGMB.GetComponent(bgmoper).deathCheck;
	overCheckC = deathBGMC.GetComponent(bgmoper).deathCheck;
	
	if(overCheckA || overCheckB || overCheckC){
		gameoverCheck = true;
	}
	
	if(playCheck){
		Deathmusic();
	}
}

function Deathmusic(){
	if(gameoverCheck){
		//Debug.Log("deadmusicq");
		GetComponent.<AudioSource>().Play();
		Waitmenu();
		playCheck = false;	
	}
}


//여기부터 GUI
function Waitmenu(){
	yield WaitForSeconds(1.0f);
	menuappear = true;
	
}

function OnGUI () {
	var sw : int = Screen.width;
	var sh : int = Screen.height;

	if(menuappear){

		GUI.skin=again;
		if(GUI.Button(new Rect(sw/3+50,sh*2/3-10,100,140),"button")==true){
			Application.LoadLevel("Main");
		}
		
		GUI.skin=d_exit;
		if(GUI.Button(new Rect(sw/3+180,sh*2/3-10,100,140),"button")==true){
			Application.Quit();
		}
    }
}

