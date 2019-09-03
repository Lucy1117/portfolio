#pragma strict
public var deathCheck : boolean;

function Start () {

}

function Update () {

}
function OnTriggerStay (col : Collider){
	if(deathCheck){
		//Debug.Log("Stop");
		GetComponent.<AudioSource>().Stop();
	}
}

function OnTriggerEnter (col : Collider){
	if(deathCheck){

	}
	else{
		if(col.gameObject.tag == "Chick"){
			//Debug.Log("Start");
    		GetComponent.<AudioSource>().Play();
		}
	}
}

function OnTriggerExit (col : Collider){
	if(col.gameObject.tag == "Chick"){
		//Debug.Log("Exit");
		GetComponent.<AudioSource>().Stop();
	}
}