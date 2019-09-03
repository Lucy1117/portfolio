#pragma strict
public var bloodSound : AudioClip;
function Start(){
}

function OnTriggerEnter (col : Collider){

	if(col.gameObject.tag == "Chick"){
		col.gameObject.SendMessage("Death");
		GetComponent.<AudioSource>().PlayOneShot(bloodSound);
	}
}
