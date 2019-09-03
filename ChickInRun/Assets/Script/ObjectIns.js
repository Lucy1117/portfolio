#pragma strict

function Start(){
}
/*
function OnTriggerEnter (col : Collider){

	if(col.gameObject.tag == "Chick"){
		col.GetComponent(PlayerInst).crashVar++;
		col.gameObject.SendMessage("Split");
	}
}
*/

function OnCollisionEnter (col : Collision){

	if(col.gameObject.tag == "Chick"){
		col.gameObject.GetComponent(PlayerInst).crashVar++;
		col.gameObject.SendMessage("Split");
	}
}
