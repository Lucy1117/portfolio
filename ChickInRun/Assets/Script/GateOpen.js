#pragma strict
public var openNum : int;
public var lsoundCheck : boolean;
public var lSound : AudioClip;

function Start () {
	GetComponent.<Animation>().Play("Stop");
	openNum = 0;
}

function Update () {
	if(lsoundCheck){
		GetComponent.<AudioSource>().PlayOneShot(lSound);
		lsoundCheck = false;	
	}
}

function OnTriggerEnter (col : Collider){

	if(col.gameObject.tag == "Chick"){
		if(openNum == 12){
			GetComponent.<Animation>().Play("Move");
		}
	}
}