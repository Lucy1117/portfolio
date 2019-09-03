#pragma strict
function Start () {

}

function Update () {
	if(Input.GetButtonDown("Jump")){
			GetComponent.<Animation>().Play("CamIntro");

	}
}

