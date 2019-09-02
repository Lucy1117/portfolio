#pragma strict
private var goalNum : int;
public var gate : GameObject;


function Start () {

}

function Update () {

	goalNum = gate.gameObject.GetComponent(GateOpen).openNum;

}

function OnTriggerEnter (col : Collider){

	if(col.gameObject.tag == "Chick"){
		if(goalNum == 12){
			yield WaitForSeconds(1.0f);
			Application.LoadLevel("Ending");
		}
	}
}