#pragma strict
public var elecSound : AudioClip;
function Start(){
}

function OnTriggerEnter (col : Collider){

	if(col.gameObject.tag == "Chick"){
		col.gameObject.GetComponent(PlayerInst).electricVar++;
		col.gameObject.SendMessage("ElecDeath");
		GetComponent.<AudioSource>().PlayOneShot(elecSound);

	}
}
