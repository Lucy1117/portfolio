#pragma strict
var labelStyle : GUIStyle;


private var guiAlpha : float;
//public var myTexture : Texture;

function Start(){
	
	//myTexture = Resources.Load("title03");

}

function Update () {
	if(Input.GetButtonDown("Jump")){
			Destroy(gameObject);
	}
	//guiAlpha = Mathf.Lerp(0,1.0f,Time.deltaTime);
	//혼자하다 실패한 코드 
	lerpAlpha();
}

function OnGUI () {

	
	GUI.color.a = guiAlpha;
	var sw : int = Screen.width;
	var sh : int = Screen.height;
	//GUI.DrawTexture(Rect(0,0, sw/2, sh/2),myTexture);
    GUI.Label(Rect(0, 2*sh/3, sw, sh/4), "시작하려면 Space 키를 누르세요", labelStyle);
}

//아래가 참고한 것 
function lerpAlpha(){
	var lerp : float = Mathf.PingPong(Time.time, 1.0f)/1.0f;
	guiAlpha = Mathf.Lerp(0.0,1.0,lerp);
}











